using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class ButtonsManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button playButton, optionsButton, creditsButton, quitButton;

        [Header("Play parameters")]
        [SerializeField] private GameObject objToDeactivate;
    
        [Header("Options parameters")]
        [SerializeField] private GameObject optionsMenu;
    
        [Header("Credits parameters")]
        [SerializeField] private GameObject creditsImage;
    
        private void Awake()
        {
            MenuAudio.Instance.PlayMainMenuMusic();
            
            playButton.onClick.AddListener(Play);
            optionsButton.onClick.AddListener(Options);
            creditsButton.onClick.AddListener(Credits);
            quitButton.onClick.AddListener(Quit);
            
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(playButton.gameObject);
        }

        private void Play()
        {
            MenuAudio.Instance.PlayFeedbackUI();
            MenuAudio.Instance.StopMainMenuMusic();
            GameManager.GameStarted = true;
            objToDeactivate.SetActive(false);
        }

        private void Options()
        {
            MenuAudio.Instance.PlayFeedbackUI();
            optionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        private void Credits()
        {
            MenuAudio.Instance.PlayFeedbackUI();
            creditsImage.SetActive(true);
            gameObject.SetActive(false);
        }

        private void Quit()
        {
            MenuAudio.Instance.PlayFeedbackUI();
            if (!Application.isEditor)
            {
                Application.Quit();
                return;
            }

            EditorApplication.isPlaying = false;
        }
    }
}
