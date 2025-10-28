using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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
        playButton.onClick.AddListener(Play);
        optionsButton.onClick.AddListener(Options);
        creditsButton.onClick.AddListener(Credits);
        quitButton.onClick.AddListener(Quit);
    }

    private void Play()
    {
        objToDeactivate.SetActive(false);
    }

    private void Options()
    {
        optionsMenu.SetActive(true);
    }

    private void Credits()
    {
        creditsImage.SetActive(true);
    }

    private void Quit()
    {
        if (!Application.isEditor)
        {
            Application.Quit();
            return;
        }

        EditorApplication.isPlaying = false;
    }
}
