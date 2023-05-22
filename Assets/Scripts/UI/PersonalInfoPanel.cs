using Coup.GameLogic;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Coup.UI
{
    public class PersonalInfoPanel : MonoBehaviour
    {
        private const string COIN_COUNT_TEXT = "Coins: ";

        [SerializeField]
        private TextMeshProUGUI _playerName;

        [SerializeField]
        private TextMeshProUGUI _coinCount;

        public void UpdatePanel(string playerName, List<Card> influence, int coinCount)
        {
            _playerName.text = playerName;
            _coinCount.text = COIN_COUNT_TEXT + coinCount;
        }
    }
}
