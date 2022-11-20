using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.UnloadSceneAsync(Constants.Scenes.MainMenu).completed += op =>
            {
                if (op.isDone)
                {
                    SceneManager.LoadSceneAsync(Constants.Scenes.Game, LoadSceneMode.Additive);
                }
            };
        }
    }
}