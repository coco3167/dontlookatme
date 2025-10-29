using System;
using UnityEngine;

public class GameManagerUtils : MonoBehaviour
{
    [SerializeField] private bool skipMenu;

    private void Awake()
    {
        GameManager.GameStarted = skipMenu;
    }
}
