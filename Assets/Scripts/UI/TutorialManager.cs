using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private GameObject objToDeactivate;
        [SerializeField] private Image objectImage;
        [SerializeField] private Sprite[] sprites;

        private void Awake()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            GameManager.RollItem();
            objectImage.sprite = sprites[(int)GameManager.ItemToFear];

            StartCoroutine(StartGame());
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(5);
            
            MenuAudio.Instance.PlayFeedbackUI();
            MenuAudio.Instance.StopMainMenuMusic();
            
            MainMusicManager.Instance.PlayMusic();
            
            GameManager.GameStarted = true;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            objToDeactivate.SetActive(false);
        }
    }
}
