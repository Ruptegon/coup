using Coup.GameLogic;
using Coup.GameLogic.Enums;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Coup
{
    public class AIPlayer
    {
        private const int CHANCE_TO_CHALLENGE_ACTION = 30;
        private const int CHANCE_TO_CHALLENGE_COUNTER = 50;
        private const int CHANCE_TO_COUNTER = 50;

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

            UnityEngine.Debug.Log($"OnPlayerMustPayInfluence {_playerController.Player.PlayerName}");

            OnPlayerMustPayInfluenceAsync(idOfPlayerThatHasToPay);
        }

        private async void OnPlayerMustPayInfluenceAsync(Guid idOfPlayerThatHasToPay)
        {

            if (idOfPlayerThatHasToPay == _playerController.Player.Id)
            {
                await Task.Delay(1000);

                Guid cardId = _playerController.Player.Influence[_random.Next(0, _playerController.Player.Influence.Count)].Id;
                _playerController.PayInfluence(cardId);
            }
        }

        private void GameEngine_OnGamePhaseChanged(GamePhase phase)
        {
            if (IsPlayerDefeated()) return;

            UnityEngine.Debug.Log($"OnGamePhaseChanged {phase} {_playerController.Player.PlayerName}");

            OnGamePhaseChangedAsync(phase);
        }

        private async void OnGamePhaseChangedAsync(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.PickAction:
                    if (_playerController.Player.Id == _engine.CurrentPlayer.Id)
                    {
                        await Task.Delay(1000);
                        PickRandomAction();
                    }
                    break;

                case GamePhase.ChallengeAction:
                    await Task.Delay(1000);
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
                    await Task.Delay(1000);
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
                    await Task.Delay(1000);
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

        private bool IsPlayerDefeated()
        {
            return _playerController.Player.IsPlayerDefeated();
        }

        private void PickRandomAction()
        {
            _playerController.PickPersonalAction(PersonalGameActionType.Income);
        }
    }
}
