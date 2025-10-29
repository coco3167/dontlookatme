using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerUtils : MonoBehaviour
{
    [SerializeField] private bool skipMenu;

    private void Awake()
    {
        if (!skipMenu)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Additive);
        }
        GameManager.GameStarted = skipMenu;
        GameManager.GameEnded = false;
    }
}
