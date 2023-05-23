using Coup.GameLogic.GameActions;
using Coup.GameLogic.Enums;
using System;
using System.Linq;
using Coup.GameLogic.GameActions.GeneralGameActions;
using System.Collections.Generic;

namespace Coup.GameLogic
{
    public class GameEngine
    {
        private const string DEFAULT_PLAYER_NAME = "Player";

        public event Action<Guid, GameAction> OnPlayerPickedAction;
        public event Action<Guid> OnPlayerDeclaredCounter;
        public event Action<Guid, bool> OnPlayerChallengedAction;
        public event Action<Guid, bool> OnPlayerChallengedCounter;
        public event Action<Guid> OnPlayerMustPayInfluence;
        public event Action<GameState> OnGameStateUpdated;
        public event Action<GamePhase> OnGamePhaseChanged;
        public event Action<Guid> OnGameFinished;

        public GamePhase GamePhase { get => _gamePhase; private set { _gamePhase = value; OnGamePhaseChanged?.Invoke(value); } }
        public GameState GameState { get => _gameState; }
        public Player CurrentPlayer { get => _currentPlayer; }
        public Player CounteringPlayer { get => _gameState.GetPlayerById(_counteringPlayerId); }
        public GameAction CurrentAction { get => _currentAction; }
        public GameLog GameLog { get; private set; }
        public bool IsChallengeInProgress { get; private set; }

        private GameState _gameState;
        private GamePhase _gamePhase;
        private Player _currentPlayer;
        private GameAction _currentAction;
        private Dictionary<Guid, bool> _waitingForPlayersResponse;
        private Guid _counteringPlayerId;

        public GameEngine()
        {
            OnGamePhaseChanged += GamePhaseChanged;
        }

        public void SetupNewGame(int playerCount)
        {
            _gameState = new GameState();
            _waitingForPlayersResponse = new Dictionary<Guid, bool>();
            GameLog = new GameLog(this);

            for (int i = 0; i < playerCount; i++)
            {
                Player player = new Player(DEFAULT_PLAYER_NAME + i);
                player.GiveCoins(2);
                player.InitInfluence(_gameState.Court.TakeCard(), _gameState.Court.TakeCard());
                _gameState.Players.Add(player);
                _waitingForPlayersResponse.Add(player.Id, false);
            }

            _currentPlayer = _gameState.Players.First();
            _currentAction = null;
            GamePhase = GamePhase.PickAction;
        }

        public void ActionPicked(GameAction action)
        {
            if(GamePhase != GamePhase.PickAction)
            {
                throw new Exception("Action picked during wrong game phase.");
            }

            if(action.PlayerTakingActionID != _currentPlayer.Id)
            {
                throw new Exception("Wrong player tried to take action.");
            }

            _currentAction = action;
            _waitingForPlayersResponse[action.PlayerTakingActionID] = false;
            OnPlayerPickedAction?.Invoke(_currentPlayer.Id, action);

            if (action is GeneralGameAction)
            {
                if (action.IsActionCounterable())
                {
                    GamePhase = GamePhase.Counter;
                }
                else
                {
                    GamePhase = GamePhase.ExecuteAction;
                }
            }
            else
            {
                GamePhase = GamePhase.ChallengeAction;
            }
        }

        public void ChallengeAction(Guid challengingPlayerId)
        {
            if (GamePhase != GamePhase.ChallengeAction)
            {
                throw new Exception("Action challenged during wrong game phase.");
            }

            IsChallengeInProgress = true;
            StopWaitingForPlayersResponse();

            Player challengedPlayer = _gameState.GetPlayerById(_currentAction.PlayerTakingActionID);
            Guid cardGuid = Guid.Empty;
            if (_currentAction.CharacterEnablingAction == Character.Null || challengedPlayer.IsInfluencingCharacter(_currentAction.CharacterEnablingAction, out cardGuid))
            {
                OnPlayerChallengedAction?.Invoke(challengingPlayerId, false);
                OrderPlayerToPayInfluence(challengingPlayerId);
                SwapShownCharacter(_currentAction.PlayerTakingActionID, cardGuid);
            }
            else
            {
                OnPlayerChallengedAction?.Invoke(challengingPlayerId, true);
                _currentAction.IsChallengedOrCountered = true;
                OrderPlayerToPayInfluence(_currentAction.PlayerTakingActionID);
            }
        }

        public void CounterAction(Guid counteringPlayerId)
        {
            if (GamePhase != GamePhase.Counter)
            {
                throw new Exception("Action countered during wrong game phase.");
            }

            if (!CurrentAction.IsActionCounterable()) 
            {
                throw new Exception("Non counterable action countered");
            }

            if (CurrentAction.TargetPlayerID != CurrentAction.PlayerTakingActionID && CurrentAction.TargetPlayerID != counteringPlayerId) 
            {
                throw new Exception("Player attempted to counter action only target can attempt to counter");
            }

            StopWaitingForPlayersResponse();

            _currentAction.IsChallengedOrCountered = true;
            this._counteringPlayerId = counteringPlayerId;
            OnPlayerDeclaredCounter?.Invoke(counteringPlayerId);
            GamePhase = GamePhase.ChallengeCounter;

            MoveToNextPhaseIfNotWaitingForPlayers();
        }

        public void ChallengeCounter(Guid challengingPlayerId)
        {
            if (GamePhase != GamePhase.ChallengeCounter)
            {
                throw new Exception("Counter challenged during wrong game phase.");
            }

            IsChallengeInProgress = true;
            StopWaitingForPlayersResponse();


            Player challengedPlayer = _gameState.GetPlayerById(_counteringPlayerId);

            Guid cardGuid = Guid.Empty;
            if (_currentAction.CharactersCounteringAction.Any(character => challengedPlayer.IsInfluencingCharacter(character, out cardGuid)))
            {
                OnPlayerChallengedCounter?.Invoke(challengingPlayerId, false);
                OrderPlayerToPayInfluence(challengingPlayerId);
                SwapShownCharacter(_counteringPlayerId, cardGuid);
            }
            else
            {
                OnPlayerChallengedCounter?.Invoke(challengingPlayerId, true);
                _currentAction.IsChallengedOrCountered = false;
                OrderPlayerToPayInfluence(_counteringPlayerId);
            }
        }

        public void SkipChallengeOrCounter(Guid playerSkippingId)
        {
            _waitingForPlayersResponse[playerSkippingId] = false;
            MoveToNextPhaseIfNotWaitingForPlayers();
        }

        public void OrderPlayerToPayInfluence(Guid payingPlayerId)
        {
            Player player = _gameState.GetPlayerById(payingPlayerId);
            if (player.IsPlayerDefeated())
            {
                MoveToNextPhaseIfNotWaitingForPlayers();
            }
            else
            {
                _waitingForPlayersResponse[payingPlayerId] = true;
                OnPlayerMustPayInfluence?.Invoke(payingPlayerId);
            }
        }

        public void PayInfluence(Guid payingPlayerId, Guid cardId)
        {
            _waitingForPlayersResponse[payingPlayerId] = false;

            Player payingPlayer = _gameState.GetPlayerById(payingPlayerId);
            payingPlayer.RevealInfluence(cardId);
            OnGameStateUpdated?.Invoke(_gameState);

            CheckWinCondition();
            MoveToNextPhaseIfNotWaitingForPlayers();
        }

        private void GamePhaseChanged(GamePhase newGamePhase) 
        {
            IsChallengeInProgress = false;

            switch (newGamePhase)
            {
                case GamePhase.PickAction:
                    _waitingForPlayersResponse[_currentPlayer.Id] = true;
                    break;
                case GamePhase.ChallengeAction:
                    WaitForPlayersResponse();
                    _waitingForPlayersResponse[_currentPlayer.Id] = false;
                    break;
                case GamePhase.Counter:
                    if (CurrentAction.TargetPlayerID == CurrentAction.PlayerTakingActionID) 
                    {
                        WaitForPlayersResponse();
                        _waitingForPlayersResponse[_currentPlayer.Id] = false;
                    }
                    else
                    {
                        StopWaitingForPlayersResponse();
                        _waitingForPlayersResponse[CurrentAction.TargetPlayerID] = true;
                    }
                    break;
                case GamePhase.ChallengeCounter:
                    WaitForPlayersResponse();
                    _waitingForPlayersResponse[_counteringPlayerId] = false;
                    break;
                case GamePhase.ExecuteAction:
                    _currentAction.ExecuteAction();
                    OnGameStateUpdated?.Invoke(_gameState);
                    MoveToNextPhaseIfNotWaitingForPlayers();
                    break;
            }
        }

        private void MoveToNextPhaseIfNotWaitingForPlayers() 
        {
            if (IsWaitingForPlayersResponse()) return;

            switch (GamePhase)
            {
                case GamePhase.ChallengeAction:
                    if (CurrentAction.IsChallengedOrCountered || !CurrentAction.IsActionCounterable())
                    {
                        GamePhase = GamePhase.ExecuteAction;
                    }
                    else
                    {
                        GamePhase = GamePhase.Counter;
                    }
                    break;

                case GamePhase.Counter:
                    GamePhase = GamePhase.ChallengeCounter;
                    break;

                case GamePhase.ChallengeCounter:
                    GamePhase = GamePhase.ExecuteAction;
                    break;

                case GamePhase.ExecuteAction:
                    _currentPlayer = GetNextPlayer(_currentPlayer);
                    GamePhase = GamePhase.PickAction;
                    break;
            }
        }

        private void SwapShownCharacter(Guid playerGuid, Guid cardShown)
        {
            InfluenceSlot influenceSlot = _gameState.GetPlayerById(playerGuid).Influence.First(i => i.Card.Id == cardShown);
            Card card = influenceSlot.Card;
            _gameState.Court.ReturnCardToCourt(card);
            _gameState.Court.Reshuffle();
            influenceSlot.Card = _gameState.Court.TakeCard();
        }

        private void CheckWinCondition()
        {
            int playersLeft = _gameState.Players.Count(p => !p.IsPlayerDefeated());
            if (playersLeft == 1)
            {
                OnGameFinished?.Invoke(_gameState.Players.First(p => !p.IsPlayerDefeated()).Id);
                _gamePhase = GamePhase.GameFinished;
            }
        }

        private Player GetNextPlayer(Player currentPlayer)
        {
            bool isNextPlayerAlive;
            int currentPlayerIndex = _gameState.Players.IndexOf(currentPlayer);

            do
            {
                currentPlayerIndex = (currentPlayerIndex + 1) % _gameState.Players.Count;
                isNextPlayerAlive = !_gameState.Players[currentPlayerIndex].IsPlayerDefeated();
            }
            while (!isNextPlayerAlive);

            return _gameState.Players[currentPlayerIndex];
        }

        private void WaitForPlayersResponse()
        {
            var itemsToUpdate = new List<KeyValuePair<Guid, bool>>();
            foreach (var pair in _waitingForPlayersResponse)
            {
                bool updatedValue = !_gameState.GetPlayerById(pair.Key).IsPlayerDefeated();
                itemsToUpdate.Add(new KeyValuePair<Guid, bool>(pair.Key, updatedValue));
            }

            foreach (var pair in itemsToUpdate)
            {
                _waitingForPlayersResponse[pair.Key] = pair.Value;
            }
        }

        private void StopWaitingForPlayersResponse()
        {
            for(int i = 0; i < _waitingForPlayersResponse.Count; i++)
            {
                Guid key = _waitingForPlayersResponse.ElementAt(i).Key;
                _waitingForPlayersResponse[key] = false;
            }
        }

        private bool IsWaitingForPlayersResponse()
        {
            return _waitingForPlayersResponse.Any(p => p.Value == true);
        }
    }
}
