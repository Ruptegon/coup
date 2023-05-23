using Coup.GameLogic;
using Coup.GameLogic.Enums;
using Coup.UI.Helpers;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coup.UI
{
    public class TargetedPanel : MonoBehaviour
    {
        [SerializeField]
        private CharacterToColorHelper _characterToColor;

        [SerializeField]
        private Image _currentPlayerBackground;

        [SerializeField]
        private TextMeshProUGUI _playerName;

        [SerializeField]
        private Image _influenceImage1;

        [SerializeField]
        private Image _influenceImage2;

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

        public void UpdatePanel(string playerName, InfluenceSlot[] influence, int coinCount, bool isCurrentPlayer)
        {
            _playerName.text = playerName;

            if (influence[0].IsRevealed) 
            {
                _influenceImage1.color = _characterToColor.GetCharacterColor(influence[0].Card.Character);
            }
            else
            {
                _influenceImage1.color = Color.white;
            }

            if (influence[1].IsRevealed)
            {
                _influenceImage2.color = _characterToColor.GetCharacterColor(influence[1].Card.Character);
            }
            else
            {
                _influenceImage2.color = Color.white;
            }

            _coinCount.text = coinCount.ToString();

            _currentPlayerBackground.enabled = isCurrentPlayer;
        }

        public void CreateAction(TargetedGameActionType gameAction)
        {
            _playerActionPanel.CreateAction(gameAction, _targetId);
        }
    }
}
