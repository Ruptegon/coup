using TMPro;
using UnityEngine;

namespace Coup.UI
{
    public class ActionLogPanel : MonoBehaviour
    {
        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private TextMeshProUGUI _log;

        private void Start()
        {
            _log.text = "";
            _gameManager.GameEngine.GameLog.OnGameLogUpdated += UpdateActionLog;
        }

        public void UpdateActionLog(string newLogMessage)
        {
            _log.text = newLogMessage;
        }
    }
}
