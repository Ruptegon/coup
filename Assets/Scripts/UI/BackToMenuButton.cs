using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Coup.UI
{
    [RequireComponent(typeof(Button))]
    public class BackToMenuButton : MonoBehaviour
    {
        private const string INIT_SCENE_NAME = "Init";
        private Button _button;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            SceneManager.LoadScene(INIT_SCENE_NAME);
        }
    }
}
