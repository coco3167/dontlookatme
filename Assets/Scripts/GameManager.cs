using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public static class GameManager
{
    public static bool GameStarted;
    public static ItemType ItemToFear = 0;

    public static void RollItem()
    {
        ItemToFear = (ItemType)Random.Range(0, Enum.GetNames(typeof(ItemType)).Length);
    }

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
