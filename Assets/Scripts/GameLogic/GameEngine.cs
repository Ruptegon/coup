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
        public event Action<Guid> OnPlayerMustPayInfluence;
        public event Action<GameState> OnGameStateUpdated;
        public event Action<GamePhase> OnGamePhaseChanged;
        public event Action OnGameFinished;

        public GamePhase GamePhase { get => _gamePhase; private set { _gamePhase = value; OnGamePhaseChanged?.Invoke(value); } }
        public GameState GameState { get => _gameState; }

        private GameState _gameState;
        private GamePhase _gamePhase;
        private Player _currentPlayer;
        private GameAction _currentAction;
        private Dictionary<Guid, bool> _waitingForPlayersResponse;

        public GameEngine()
        {
            OnGamePhaseChanged += GamePhaseChanged;
        }

        public void SetupNewGame(int playerCount)
        {
            _gameState = new GameState();
            _waitingForPlayersResponse = new Dictionary<Guid, bool>();

            for (int i = 0; i < playerCount; i++)
            {
                Player player = new Player(DEFAULT_PLAYER_NAME + i);
                player.GiveCoins(2);
                player.GiveInfluence(_gameState.Court.TakeCard());
                player.GiveInfluence(_gameState.Court.TakeCard());
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

            StopWaitingForPlayersResponse();

            Player challengedPlayer = _gameState.GetPlayerById(_currentAction.PlayerTakingActionID);

            if (_currentAction.CharacterEnablingAction == null || challengedPlayer.Influence.Any(c => c.Character == _currentAction.CharacterEnablingAction))
            {
                OrderPlayerToPayInfluence(challengingPlayerId);
            }
            else
            {
                _currentAction.WasChallengedOrCountered = true;
                OrderPlayerToPayInfluence(_currentAction.PlayerTakingActionID);
            }
        }

        public void CounterAction(Guid counteringPlayerId)
        {
            if (GamePhase != GamePhase.Counter)
            {
                throw new Exception("Action countered during wrong game phase.");
            }

            StopWaitingForPlayersResponse();

            _currentAction.WasChallengedOrCountered = true;
            GamePhase = GamePhase.ChallengeCounter;
            OnPlayerDeclaredCounter?.Invoke(counteringPlayerId);
        }

        public void ChallengeCounter(Guid challengingPlayerId, Guid counteringPlayerId)
        {
            if (GamePhase != GamePhase.ChallengeCounter)
            {
                throw new Exception("Counter challenged during wrong game phase.");
            }

            StopWaitingForPlayersResponse();

            Player challengedPlayer = _gameState.GetPlayerById(counteringPlayerId);

            if (_currentAction.CharactersCounteringAction.Any(character => challengedPlayer.Influence.Any(card => card.Character == character)))
            {
                _currentAction.WasChallengedOrCountered = false;
                OrderPlayerToPayInfluence(challengingPlayerId);
            }
            else
            {
                OrderPlayerToPayInfluence(counteringPlayerId);
            }
        }

        public void SkipChallengeOrCounter(Guid playerSkippingId)
        {
            _waitingForPlayersResponse[playerSkippingId] = false;
            MoveToNextPhaseIfNotWaitingForPlayers();
        }

        public void OrderPlayerToPayInfluence(Guid payingPlayerId)
        {
            OnPlayerMustPayInfluence?.Invoke(payingPlayerId);
            _waitingForPlayersResponse[payingPlayerId] = true;
        }

        public void PayInfluence(Guid payingPlayerId, Guid cardId)
        {
            _waitingForPlayersResponse[payingPlayerId] = false;

            Player payingPlayer = _gameState.GetPlayerById(payingPlayerId);
            Card cardToTake = payingPlayer.FindCardById(cardId);
            payingPlayer.TakeInfluence(cardToTake);

            MoveToNextPhaseIfNotWaitingForPlayers();
        }

        private void GamePhaseChanged(GamePhase newGamePhase) 
        {
            switch (newGamePhase)
            {
                case GamePhase.PickAction:
                    _waitingForPlayersResponse[_currentPlayer.Id] = true;
                    break;
                case GamePhase.ChallengeAction:
                    WaitForPlayersResponse();
                    break;
                case GamePhase.Counter:
                    WaitForPlayersResponse();
                    break;
                case GamePhase.ChallengeCounter:
                    WaitForPlayersResponse();
                    break;
                case GamePhase.ExecuteAction:
                    _currentAction.ExecuteAction();
                    OnGameStateUpdated?.Invoke(_gameState);
                    _currentPlayer = GetNextPlayer(_currentPlayer);
                    _gamePhase = GamePhase.PickAction;
                    break;
            }
        }

        private void CheckWinCondition()
        {
            int playersLeft = _gameState.Players.Count(p => !p.IsPlayerDefeated());
            if(playersLeft == 1)
            {
                OnGameFinished?.Invoke();
            }
        }

        private Player GetNextPlayer(Player currentPlayer)
        {
            int currentPlayerIndex = _gameState.Players.IndexOf(currentPlayer);
            int nextPlayerIndex = (currentPlayerIndex + 1) % _gameState.Players.Count;
            return _gameState.Players[nextPlayerIndex];
        }

        private void MoveToNextPhaseIfNotWaitingForPlayers() 
        {
            if (IsWaitingForPlayersResponse()) return;

            switch (GamePhase)
            {
                case GamePhase.ChallengeAction:
                    GamePhase = GamePhase.Counter;
                    break;
                case GamePhase.Counter:
                    GamePhase = GamePhase.ChallengeCounter;
                    break;
                case GamePhase.ChallengeCounter:
                    GamePhase = GamePhase.ExecuteAction;
                    break;
            }
        }

        private void WaitForPlayersResponse()
        {
            foreach(var pair in _waitingForPlayersResponse)
            {
                _waitingForPlayersResponse[pair.Key] = true;
            }
        }

        private void StopWaitingForPlayersResponse()
        {
            foreach (var pair in _waitingForPlayersResponse)
            {
                _waitingForPlayersResponse[pair.Key] = false;
            }
        }

        private bool IsWaitingForPlayersResponse()
        {
            return _waitingForPlayersResponse.Any(p => p.Value == true);
        }
    }
}
