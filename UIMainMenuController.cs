using UnityEngine;
using Component.;

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