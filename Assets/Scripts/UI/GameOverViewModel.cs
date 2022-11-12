using UnityEngine;
using UnityEngine.SceneManagement;
using UnityWeld.Binding;

namespace Core.UI
{
    [Binding]
    public class GameOverViewModel
    {
        private readonly GameObject _gameOverPanel;

        public GameOverViewModel(GameObject gameOverPanel)
        {
            _gameOverPanel = gameOverPanel;
        }

        public void Show()
        {
            _gameOverPanel.SetActive(true);
        }
        public void Hide()
        {
            _gameOverPanel.SetActive(false);
        }

        [Binding]
        public void Restart()
        {
            SceneManager.LoadScene(Constants.Scenes.Game);
        }

        [Binding]
        public void BackToMainMenu()
        {
            SceneManager.UnloadSceneAsync(Constants.Scenes.Game).completed += op =>
            {
                if (op.isDone)
                {
                    SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu, LoadSceneMode.Additive);
                }
            };
        }
    }
}