using Coup.GameLogic.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Coup.UI
{
    public class ChallengeChoicePanel : MonoBehaviour
    {
        private const string CHALLENGE_TEXT = "Challenge!";
        private const string COUNTER_TEXT = "Counter!";
        private const string HEADER_CHALLENGE = "Do you want to challenge {0}?";
        private const string HEADER_COUNTER = "Do you want to counter {0}?";

        [SerializeField]
        private GameManager _gameManager;

        [SerializeField]
        private Button _challengeButton;

        [SerializeField]
        private Button _passButton;

        [SerializeField]
        private TextMeshProUGUI _challengeButtonText;

        [SerializeField]
        private TextMeshProUGUI _headerText;

        [SerializeField]
        private CanvasGroup _canvasGroup;

        private void Start()
        {
            HidePanel();
            _gameManager.GameEngine.OnGamePhaseChanged += GameEngine_OnGamePhaseChanged;
            _gameManager.GameEngine.OnPlayerChallengedAction += (id) => { HidePanel(); };
            _gameManager.GameEngine.OnPlayerChallengedCounter += (id) => { HidePanel(); };
            _gameManager.GameEngine.OnPlayerDeclaredCounter += (id) => { HidePanel(); };
            _passButton.onClick.AddListener(Pass);
        }

        private void GameEngine_OnGamePhaseChanged(GamePhase gamePhase)
        {
            if (gamePhase == GamePhase.ChallengeAction && _gameManager.GameEngine.CurrentPlayer != _gameManager.GetHumanPlayerController().Player)
            {
                _challengeButton.onClick.RemoveAllListeners();
                _challengeButton.onClick.AddListener(ChallengeAction);
                _challengeButtonText.text = CHALLENGE_TEXT;
                _headerText.text = string.Format(HEADER_CHALLENGE, _gameManager.GameEngine.CurrentPlayer.PlayerName);
                ShowPanel();
            }
            else if (gamePhase == GamePhase.ChallengeCounter && _gameManager.GameEngine.CounteringPlayer != _gameManager.GetHumanPlayerController().Player)
            {
                _challengeButton.onClick.RemoveAllListeners();
                _challengeButton.onClick.AddListener(ChallengeCounter);
                _challengeButtonText.text = CHALLENGE_TEXT;
                _headerText.text = string.Format(HEADER_CHALLENGE, _gameManager.GameEngine.CounteringPlayer.PlayerName);
                ShowPanel();
            }
            else if (gamePhase == GamePhase.Counter && _gameManager.GameEngine.CurrentPlayer != _gameManager.GetHumanPlayerController().Player)
            {
                _challengeButton.onClick.RemoveAllListeners();
                _challengeButton.onClick.AddListener(CounterAction);
                _challengeButtonText.text = COUNTER_TEXT;
                _headerText.text = string.Format(HEADER_COUNTER, _gameManager.GameEngine.CurrentPlayer.PlayerName);
                ShowPanel();

            }
            else
            {
                HidePanel();
                return;
            }
        }

        private void CounterAction()
        {
            _gameManager.GetHumanPlayerController().CounterAction();
            HidePanel();
        }

        private void ChallengeAction() 
        {
            _gameManager.GetHumanPlayerController().ChallengeAction();
            HidePanel();
        }

        private void ChallengeCounter()
        {
            _gameManager.GetHumanPlayerController().ChallengeCounter();
            HidePanel();
        }

        private void Pass()
        {
            _gameManager.GetHumanPlayerController().SkipChallengeOrCounter();
            HidePanel();
        }

        private void ShowPanel()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
        }

        private void HidePanel()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
        }
    }
}
