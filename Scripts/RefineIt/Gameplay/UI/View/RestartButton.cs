using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Gameplay.UI.View
{
    [RequireComponent(typeof(Button))]
    public class RestartButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        
#if UNITY_EDITOR

        private void Start()
        {
            _button.gameObject.SetActive(false);
        }

#endif
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            _button ??= gameObject.GetComponent<Button>();
        }

#endif

        private void OnEnable()
        {
            _button.onClick.AddListener(RestartLevel);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(RestartLevel);
        }

        private void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
