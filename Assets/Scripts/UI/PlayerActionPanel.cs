using Coup.GameLogic;
using Coup.GameLogic.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Coup.UI
{
    public class PlayerActionPanel : MonoBehaviour
    {
        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private PersonalInfoPanel _personalInfoPanel;

        [SerializeField]
        private Transform _targetedPanelsHolder;

        [SerializeField]
        private TargetedPanel _targetedPanelPrefab;

        private List<TargetedPanel> _targetedPanels;

        private void Start()
        {
            _targetedPanels = new List<TargetedPanel>();
            for(int i = 1; i < _gameManager.GameEngine.GameState.Players.Count; i++)
            {
                TargetedPanel targetedPanel = Instantiate(_targetedPanelPrefab, _targetedPanelsHolder);
                targetedPanel.Init(_gameManager.GameEngine.GameState.Players[i].Id, this);
                _targetedPanels.Add(targetedPanel);
            }

            _gameManager.GameEngine.OnPlayerMustPayInfluence += GameEngine_OnPlayerMustPayInfluence;
            _gameManager.GameEngine.OnGameStateUpdated += GameEngine_OnGameStateUpdated;
            GameEngine_OnGameStateUpdated(_gameManager.GameEngine.GameState);
        }

        public void CreateAction(PersonalGameActionType gameAction)
        {
            _gameManager.GetHumanPlayerController().PickPersonalAction(gameAction);
        }

        public void CreateAction(TargetedGameActionType gameAction, Guid targetId)
        {
            _gameManager.GetHumanPlayerController().PickTargetedAction(gameAction, targetId);
        }

        private void GameEngine_OnGameStateUpdated(GameState gameState)
        {
            Player humanPlayer = _gameManager.GetHumanPlayerController().Player;
            _personalInfoPanel.UpdatePanel(humanPlayer.PlayerName, humanPlayer.Influence, humanPlayer.Coins);
            
            foreach(TargetedPanel panel in _targetedPanels)
            {
                Player targetPlayer = gameState.GetPlayerById(panel.TargetId);
                panel.UpdatePanel(targetPlayer.PlayerName, targetPlayer.Influence, targetPlayer.Coins);
            }
        }

        private void GameEngine_OnPlayerMustPayInfluence(Guid playerId)
        {
            if(playerId == _gameManager.GetHumanPlayerController().Player.Id)
            {
                _personalInfoPanel.EnablePayingInfluence(PayInfluence);
            }
        }

        private void PayInfluence(int influenceIndex)
        {
            PlayerController playerController = _gameManager.GetHumanPlayerController();
            Guid cardId = playerController.Player.Influence[influenceIndex].Card.Id;
            playerController.PayInfluence(cardId);
        }
    }
}
