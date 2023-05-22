using Coup.GameLogic.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Coup.UI
{
    [RequireComponent(typeof(Button))]
    public class TargetedActionButton : MonoBehaviour
    {
        [SerializeField]
        private TargetedPanel _targetedPanel;

        [SerializeField]
        private TargetedGameActionType _gameAction;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _targetedPanel.CreateAction(_gameAction);
        }
    }
}
