using UnityEngine.SceneManagement;

namespace Components.SceneLoader.Scripts
{
    public static class SceneLoaderService
    {
        public static void LoadScene()
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
            SceneManager.LoadScene("UIScene", LoadSceneMode.Additive);
        }
        
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenuScene", LoadSceneMode.Single);
        }
    }
}