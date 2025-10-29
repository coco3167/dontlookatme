using UnityEngine.SceneManagement;

public static class GameManager
{
    public static bool GameStarted;

    public static void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
