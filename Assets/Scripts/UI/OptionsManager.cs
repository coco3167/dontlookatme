using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class OptionsManager : MonoBehaviour
    {
        [Header("Main")]
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Button closeButton;

        [Header("Fullscreen")]
        [SerializeField] private Toggle fullscreen;
        
        [Header("Volume")]
        [SerializeField] private Slider volume;
        private void Awake()
        {
            closeButton.onClick.AddListener(() =>
            {
                MenuAudio.Instance.PlayFeedbackUI();
                mainMenu.SetActive(true);
                gameObject.SetActive(false);
            });
            
            fullscreen.onValueChanged.AddListener(value =>
            {
                MenuAudio.Instance.PlayFeedbackUI();
                Screen.fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            });
            
            volume.onValueChanged.AddListener(value =>
            {
                AudioListener.volume = value;
                MenuAudio.Instance.PlayFeedbackUI();
            });
            
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(closeButton.gameObject);
        }
    }
}
