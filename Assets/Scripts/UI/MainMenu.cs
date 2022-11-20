using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame_UnityEditor()
        {
            try
            {
                SceneManager.UnloadSceneAsync(Constants.Scenes.MainMenu).completed += OnSceneLoaded;
            }
            catch
            {
#if UNITY_EDITOR
                Debug.LogWarning("Please, initialize game with Bootstrap scene.");
#endif
            }
        }
        private void OnSceneLoaded(AsyncOperation operation)
        {
            if (operation.isDone)
            {
                SceneManager.LoadSceneAsync(Constants.Scenes.Game, LoadSceneMode.Additive);
            }
        }

        public void Quit_UnityEditor()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}