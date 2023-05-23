using Coup.GameLogic.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Coup.UI
{
    [RequireComponent(typeof(Button))]
    public class PersonalActionButton : MonoBehaviour
    {
        [SerializeField]
        private PersonalGameActionType _gameAction;

        private PersonalActionPanel _personalActionPanel;
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        public void RegisterPersonalActionPanel(PersonalActionPanel personalActionPanel)
        {
            _personalActionPanel = personalActionPanel;
        }

        public void SetInteractions(bool setInteractionsOn)
        {
            _button.interactable = setInteractionsOn;
        }

        private void OnClick()
        {
            _personalActionPanel.CreateAction(_gameAction);
        }
    }
}
