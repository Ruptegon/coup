using System;
using UnityEngine;

namespace Coup.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            _gameManager.GameEngine.OnGameFinished += GameEngine_OnGameFinished;
        }

        private void GameEngine_OnGameFinished(Guid winnerPlayer)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }
    }
}
