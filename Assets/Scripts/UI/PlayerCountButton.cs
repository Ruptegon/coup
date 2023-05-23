using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Coup.UI
{
    [RequireComponent(typeof(Button))]
    public class PlayerCountButton : MonoBehaviour
    {
        private const string PLAYER_COUNT_KEY = "playerCount";
        private const string MAIN_SCENE_NAME = "CoupGameplayScene";

        [SerializeField]
        private int _playerCount;

        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            PlayerPrefs.SetInt(PLAYER_COUNT_KEY, _playerCount);
            SceneManager.LoadScene(MAIN_SCENE_NAME);
        }
    }
}
