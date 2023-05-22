using Coup.GameLogic;
using Coup.UI.Helpers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coup.UI
{
    public class PersonalInfoPanel : MonoBehaviour
    {
        private const string COIN_COUNT_TEXT = "Coins: ";

        [SerializeField]
        private CharacterToColorHelper _characterToColor;

        [SerializeField]
        private TextMeshProUGUI _playerName;

        [SerializeField]
        private Button _influenceImage1;

        [SerializeField]
        private Button _influenceImage2;

        [SerializeField]
        private TextMeshProUGUI _coinCount;

        public void UpdatePanel(string playerName, InfluenceSlot[] influence, int coinCount)
        {
            _playerName.text = playerName;

            Color normalColor = Color.white;
            Color revealedColor = _characterToColor.GetCharacterColor(influence[0].Card.Character);
            if (influence[0].IsRevealed)
            {
                normalColor = revealedColor;
            }
            _influenceImage1.colors = new ColorBlock { normalColor = normalColor, selectedColor = normalColor, highlightedColor = revealedColor, pressedColor = revealedColor, colorMultiplier = 1 };

            normalColor = Color.white;
            revealedColor = _characterToColor.GetCharacterColor(influence[1].Card.Character);
            if (influence[1].IsRevealed)
            {
                normalColor = revealedColor;
            }
            _influenceImage2.colors = new ColorBlock { normalColor = normalColor, selectedColor = normalColor, highlightedColor = revealedColor, pressedColor = revealedColor, colorMultiplier = 1 };

            _coinCount.text = COIN_COUNT_TEXT + coinCount;
        }
    }
}
