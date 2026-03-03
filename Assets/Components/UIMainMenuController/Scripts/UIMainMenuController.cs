using Components.SceneLoader.Scripts;
using UnityEngine;

namespace Components.UIMainMenuController.Scripts
{
    public class UIMainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneLoaderService.LoadScene();
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}