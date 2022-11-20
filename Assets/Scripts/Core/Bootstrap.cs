using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public class Bootstrap : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return InitExternalServices();
            yield return LoadProcess();
        }

        private IEnumerator InitExternalServices()
        {
            // init Steam API...
            // init Battle.Net API...
            // etc...
            yield return null;
        }
        private IEnumerator LoadProcess()
        {
            yield return SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu, LoadSceneMode.Additive);
        }
    }
}