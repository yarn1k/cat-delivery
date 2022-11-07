using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            LoadProcess();
        }

        private void LoadProcess()
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        }
    }
}