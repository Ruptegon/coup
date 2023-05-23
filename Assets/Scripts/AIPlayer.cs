using Coup.GameLogic;
using Coup.GameLogic.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Coup
{
    public class AIPlayer
    {
        private const int CHANCE_TO_CHALLENGE_ACTION = 30;
        private const int CHANCE_TO_CHALLENGE_COUNTER = 50;
        private const int CHANCE_TO_COUNTER = 50;

        private const int MIN_AI_DELAY = 3000;
        private const int MAX_AI_DELAY = 5000;

        private PlayerController _playerController;
        private Random _random;
        private GameEngine _engine;

        public AIPlayer(PlayerController playerController, GameEngine engine)
        {
            _playerController = playerController;
            _random = new Random();
            _engine = engine;

            _engine.OnGamePhaseChanged += GameEngine_OnGamePhaseChanged;
            _engine.OnPlayerMustPayInfluence += GameEngine_OnPlayerMustPayInfluence;
        }

        private void GameEngine_OnPlayerMustPayInfluence(Guid idOfPlayerThatHasToPay)
        {
            if (IsPlayerDefeated()) return;

            OnPlayerMustPayInfluenceAsync(idOfPlayerThatHasToPay);
        }

        private async void OnPlayerMustPayInfluenceAsync(Guid idOfPlayerThatHasToPay)
        {

            if (idOfPlayerThatHasToPay == _playerController.Player.Id)
            {
                await DelayAction();

                InfluenceSlot influence;
                int randomIndex = _random.Next(0, 2);
                influence = _playerController.Player.Influence[randomIndex];
                if (influence.IsRevealed)
                {
                    influence = _playerController.Player.Influence[1 - randomIndex];
                }

                Guid cardId = influence.Card.Id;
                _playerController.PayInfluence(cardId);
            }
        }

        private void GameEngine_OnGamePhaseChanged(GamePhase phase)
        {
            if (IsPlayerDefeated()) return;

            OnGamePhaseChangedAsync(phase);
        }

        private async void OnGamePhaseChangedAsync(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.PickAction:
                    if (_playerController.Player.Id == _engine.CurrentPlayer.Id)
                    {
                        await DelayAction();
                        PickRandomAction();
                    }
                    break;

                case GamePhase.ChallengeAction:

                    if (_engine.CurrentPlayer == _playerController.Player) return;

                    await DelayAction();

                    if (_engine.GamePhase != GamePhase.ChallengeAction || _engine.IsChallengeInProgress) return;

                    if (_random.Next(0, 100) < CHANCE_TO_CHALLENGE_ACTION)
                    {
                        _playerController.ChallengeAction();
                    }
                    else
                    {
                        _playerController.SkipChallengeOrCounter();
                    }
                    break;

                case GamePhase.Counter:

                    if (_engine.CurrentPlayer == _playerController.Player) return;

                    await DelayAction();

                    if (_engine.GamePhase != GamePhase.Counter) return;

                    if (_random.Next(0, 100) < CHANCE_TO_COUNTER)
                    {
                        _playerController.CounterAction();
                    }
                    else
                    {
                        _playerController.SkipChallengeOrCounter();
                    }
                    break;

                case GamePhase.ChallengeCounter:

                    if (_engine.CounteringPlayer == _playerController.Player) return;

                    await DelayAction();

                    if (_engine.GamePhase != GamePhase.ChallengeCounter || _engine.IsChallengeInProgress) return;

                    if (_random.Next(0, 100) < CHANCE_TO_CHALLENGE_COUNTER)
                    {
                        _playerController.ChallengeCounter();
                    }
                    else
                    {
                        _playerController.SkipChallengeOrCounter();
                    }
                    break;

            }
        }

        private async Task DelayAction()
        {
            await Task.Delay(_random.Next(MIN_AI_DELAY, MAX_AI_DELAY));
        }

        private bool IsPlayerDefeated()
        {
            return _playerController.Player.IsPlayerDefeated();
        }

        private void PickRandomAction()
        {
            if(_playerController.Player.Coins >= 10)
            {
                //Per rules, if player has 10 or more colors he has to coup
                _playerController.PickTargetedAction(TargetedGameActionType.Coup, PickRandomTarget());
                return;
            }

            List<PersonalGameActionType> personalActions = new List<PersonalGameActionType>();
            foreach (PersonalGameActionType action in Enum.GetValues(typeof(PersonalGameActionType))) 
            {
                personalActions.Add(action);
            }

            List<TargetedGameActionType> allowedTargetedActions = new List<TargetedGameActionType>();
            foreach(TargetedGameActionType action in Enum.GetValues(typeof(TargetedGameActionType)))
            {
                if (_playerController.CanAffordTargetedAction(action))
                {
                    allowedTargetedActions.Add(action);
                }
            }

            int randomActionId = _random.Next(0, personalActions.Count + allowedTargetedActions.Count);

            if(randomActionId >= personalActions.Count)
            {
                randomActionId -= personalActions.Count;
                _playerController.PickTargetedAction(allowedTargetedActions[randomActionId], PickRandomTarget());
            }
            else
            {
                _playerController.PickPersonalAction(personalActions[randomActionId]);
            }
        }

        private Guid PickRandomTarget()
        {
            Player player;
            do
            {
                player = _engine.GameState.Players[_random.Next(0, _engine.GameState.Players.Count)];
            }
            while (player.IsPlayerDefeated() || player == _playerController.Player);
            return player.Id;
        }
    }
}
