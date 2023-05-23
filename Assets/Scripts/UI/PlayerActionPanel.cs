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
        private PersonalActionPanel _personalActionPanel;

        [SerializeField]
        private TargetedPanel _targetedPanelPrefab;

        [SerializeField]
        private CanvasGroup _targetedPanelsGroup;

        private List<TargetedPanel> _targetedPanels;

        private void Start()
        {
            _targetedPanels = new List<TargetedPanel>();
            for(int i = 1; i < _gameManager.GameEngine.GameState.Players.Count; i++)
            {
                TargetedPanel targetedPanel = Instantiate(_targetedPanelPrefab, _targetedPanelsGroup.transform);
                targetedPanel.Init(_gameManager.GameEngine.GameState.Players[i].Id, this);
                _targetedPanels.Add(targetedPanel);
            }

            _personalActionPanel.RegisterPlayerActionPanel(this);

            _gameManager.GameEngine.OnPlayerMustPayInfluence += GameEngine_OnPlayerMustPayInfluence;
            _gameManager.GameEngine.OnGameStateUpdated += GameEngine_OnGameStateUpdated;
            _gameManager.GameEngine.OnGamePhaseChanged += GameEngine_OnGamePhaseChanged;
            GameEngine_OnGameStateUpdated(_gameManager.GameEngine.GameState);
        }

        public void CreateAction(PersonalGameActionType gameAction)
        {
            _gameManager.GetHumanPlayerController().PickPersonalAction(gameAction);
        }

        public void CreateAction(TargetedGameActionType gameAction, Guid targetId)
        {
            if (_gameManager.GetHumanPlayerController().CanAffordTargetedAction(gameAction))
            {
                _gameManager.GetHumanPlayerController().PickTargetedAction(gameAction, targetId);
            }
        }

        private void GameEngine_OnGameStateUpdated(GameState gameState)
        {
            UpdatePanel();
        }

        private void GameEngine_OnGamePhaseChanged(GamePhase newGamePhase)
        {
            UpdatePanel();
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

        private void UpdatePanel()
        {
            Player humanPlayer = _gameManager.GetHumanPlayerController().Player;
            bool isTurnOfHumanPlayer = _gameManager.GameEngine.CurrentPlayer == humanPlayer;
            _personalInfoPanel.UpdatePanel(humanPlayer.PlayerName, humanPlayer.Influence, humanPlayer.Coins, isTurnOfHumanPlayer);
            _personalActionPanel.SetInteractions(isTurnOfHumanPlayer && _gameManager.GameEngine.GamePhase == GamePhase.PickAction);
            _targetedPanelsGroup.interactable = isTurnOfHumanPlayer && _gameManager.GameEngine.GamePhase == GamePhase.PickAction;

            foreach (TargetedPanel panel in _targetedPanels)
            {
                Player targetPlayer = _gameManager.GameEngine.GameState.GetPlayerById(panel.TargetId);
                panel.UpdatePanel(targetPlayer.PlayerName, targetPlayer.Influence, targetPlayer.Coins, targetPlayer == _gameManager.GameEngine.CurrentPlayer);
            }
        }
    }
}
