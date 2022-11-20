using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Core.Loading;
using UnityWeld.Binding;

namespace Core.UI
{
    [Binding]
    public class MainMenu : MonoBehaviour
    {
        private ILoadingScreenProvider _loadingScreenProvider;

        [Inject]
        private void Construct(ILoadingScreenProvider provider)
        {
            _loadingScreenProvider = provider;
        }

        [Binding]
        public void StartGame()
        {            
            var operations = new Queue<LazyLoadingOperation>();
            Func<ILoadingOperation> sceneCleanupOperation = () => new SceneCleanupOperation(Constants.Scenes.MainMenu);
            Func<ILoadingOperation> gameLoadingOperation = () => new SceneLoadingOperation(Constants.Scenes.Game);
            Func<ILoadingOperation> pressAnyButtonOperation = () => new PressAnyButtonOperation();
            operations.Enqueue(sceneCleanupOperation);
            operations.Enqueue(gameLoadingOperation);
            operations.Enqueue(pressAnyButtonOperation);
            _loadingScreenProvider.LoadAndDestroyAsync(operations);
        }

        [Binding]
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}