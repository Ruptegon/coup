using Coup.GameLogic.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Coup.UI
{
    [RequireComponent(typeof(Button))]
    public class PersonalActionButton : MonoBehaviour
    {
        [SerializeField]
        private PlayerActionPanel _playerActionPanel;

        [SerializeField]
        private PersonalGameActionType _gameAction;

        private Button _button;

        void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _playerActionPanel.CreateAction(_gameAction);
        }
    }
}
