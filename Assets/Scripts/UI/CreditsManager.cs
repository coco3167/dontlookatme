using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CreditsManager : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private Button close;

        private void Awake()
        {
            close.onClick.AddListener(() =>
            {
                MenuAudio.Instance.PlayFeedbackUI();
                mainMenu.SetActive(true);
                gameObject.SetActive(false);
            });
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(close.gameObject);
        }
    }
}
