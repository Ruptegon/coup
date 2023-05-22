using Coup.GameLogic;
using Coup.GameLogic.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Coup.UI
{
    public class TargetedPanel : MonoBehaviour
    {
        private const string COIN_COUNT_TEXT = "Coins: ";

        [SerializeField]
        private TextMeshProUGUI _playerName;

        [SerializeField]
        private TextMeshProUGUI _coinCount;

        public Guid TargetId { get => _targetId; }

        private PlayerActionPanel _playerActionPanel;
        private Guid _targetId;

        public void Init(Guid targetGuid, PlayerActionPanel playerActionPanel)
        {
            _targetId = targetGuid;
            _playerActionPanel = playerActionPanel;
        }

        public void UpdatePanel(string playerName, List<Card> influence, int coinCount)
        {
            _playerName.text = playerName;
            _coinCount.text = COIN_COUNT_TEXT + coinCount;
        }

        public void CreateAction(TargetedGameActionType gameAction)
        {
            _playerActionPanel.CreateAction(gameAction, _targetId);
        }
    }
}
