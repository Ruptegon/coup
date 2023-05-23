using Coup.GameLogic.GameActions;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Coup.GameLogic
{
    public class GameLog
    {
        public event Action<string> OnGameLogUpdated;

        private GameEngine _engine;
        private string _logMessage;

        public GameLog(GameEngine engine) 
        {
            _engine = engine;
            _engine.OnPlayerPickedAction += GameEngine_OnPlayerPickedAction;
            _engine.OnPlayerChallengedAction += GameEngine_OnPlayerChallengedAction;
            _engine.OnPlayerChallengedCounter += GameEngine_OnPlayerChallengedCounter;
            _engine.OnPlayerDeclaredCounter += GameEngine_OnPlayerDeclaredCounter;
            _engine.OnGameFinished += GameEngine_OnGameFinished;
        }

        private void GameEngine_OnGameFinished(Guid winner)
        {
            string winnerName = _engine.GameState.GetPlayerById(winner).PlayerName;
            _logMessage = $"{winnerName} won!";
            OnGameLogUpdated?.Invoke(_logMessage);
        }

        private void GameEngine_OnPlayerDeclaredCounter(Guid playerId)
        {
            string counteringPlayerName = _engine.CounteringPlayer.PlayerName;
            string actingPlayerName = _engine.CurrentPlayer.PlayerName;
            GameAction currentAction = _engine.CurrentAction;

            if (currentAction.TargetPlayerID == currentAction.PlayerTakingActionID)
            {
                _logMessage = $"{counteringPlayerName} declared counter against {currentAction.Name} of {actingPlayerName}";
            }
            else
            {
                string targetPlayerName = _engine.GameState.GetPlayerById(currentAction.TargetPlayerID).PlayerName;
                _logMessage = $"{counteringPlayerName} declared counter against {currentAction.Name} of {actingPlayerName} targeted at {targetPlayerName}";
            }

            OnGameLogUpdated?.Invoke(_logMessage);
        }

        private void GameEngine_OnPlayerPickedAction(Guid playerId, GameAction action)
        {
            string playerName = _engine.GameState.GetPlayerById(playerId).PlayerName;
            if(action.TargetPlayerID == action.PlayerTakingActionID)
            {
                _logMessage = $"{playerName} tries to do {action.Name}";
            }
            else
            {
                string targetPlayerName = _engine.GameState.GetPlayerById(action.TargetPlayerID).PlayerName;
                _logMessage = $"{playerName} tries to do {action.Name} to {targetPlayerName}";
            }

            OnGameLogUpdated?.Invoke(_logMessage);
        }

        private void GameEngine_OnPlayerChallengedAction(Guid playerId, bool challengeWon)
        {
            string challengingPlayerName = _engine.GameState.GetPlayerById(playerId).PlayerName;
            string actingPlayerName = _engine.CurrentPlayer.PlayerName;
            GameAction currentAction = _engine.CurrentAction;
            string result = challengeWon ? "won" : "lost";

            if (currentAction.TargetPlayerID == currentAction.PlayerTakingActionID)
            {
                _logMessage = $"{challengingPlayerName} {result} challenge against {currentAction.Name} of {actingPlayerName}";
            }
            else
            {
                string targetPlayerName = _engine.GameState.GetPlayerById(currentAction.TargetPlayerID).PlayerName;
                _logMessage = $"{challengingPlayerName} {result} challenge against {currentAction.Name} of {actingPlayerName} targeted at {targetPlayerName}";
            }

            OnGameLogUpdated?.Invoke(_logMessage);
        }

        private void GameEngine_OnPlayerChallengedCounter(Guid playerId, bool challengeWon)
        {
            string challengingPlayerName = _engine.GameState.GetPlayerById(playerId).PlayerName;
            string counteringPlayerName = _engine.CounteringPlayer.PlayerName;
            string actingPlayerName = _engine.CurrentPlayer.PlayerName;
            GameAction currentAction = _engine.CurrentAction;
            string result = challengeWon ? "won" : "lost";

            if (currentAction.TargetPlayerID == currentAction.PlayerTakingActionID)
            {
                _logMessage = $"{challengingPlayerName} {result} challenge of counter of {counteringPlayerName} against {currentAction.Name} of {actingPlayerName}";
            }
            else
            {
                string targetPlayerName = _engine.GameState.GetPlayerById(currentAction.TargetPlayerID).PlayerName;
                _logMessage = $"{challengingPlayerName} {result} challenge of counter of {counteringPlayerName} against {currentAction.Name} of {actingPlayerName} targeted at {targetPlayerName}";
            }

            OnGameLogUpdated?.Invoke(_logMessage);
        }
    }
}
