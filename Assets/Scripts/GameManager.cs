using System;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public static class GameManager
{
    public static bool GameStarted, GameEnded;
    public static bool WanaRestart = false;
    public static ItemType ItemToFear = 0;

    public static void RollItem()
    {
        ItemToFear = (ItemType)Random.Range(0, Enum.GetNames(typeof(ItemType)).Length);
    }

    public static void RestartGame(bool WanaRestart = false)
    {
        GameManager.WanaRestart = WanaRestart;
        GameManager.GameStarted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public static void PausingGame()
	{
        RestartGame(false);
	}
}
