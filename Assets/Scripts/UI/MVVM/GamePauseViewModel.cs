using System;
using System.Collections.Generic;
using UnityEngine;
using UnityWeld.Binding;
using Zenject;
using Core.Loading;

namespace Core.UI
{
    [Binding]
    public class GamePauseViewModel : MonoBehaviour
    {        
        private ILoadingScreenProvider _loadingScreenProvider;
        private IPauseProvider _pauseProvider;

        [Inject]
        private void Construct(ILoadingScreenProvider loadingScreenProvider, IPauseProvider pauseProvider)
        {
            _loadingScreenProvider = loadingScreenProvider;
            _pauseProvider = pauseProvider;
        }

        public void Pause()
        {
            gameObject.SetActive(true);
            _pauseProvider.SetPaused(true);
        }

        [Binding]
        public void OpenOptions()
        {

        }
        [Binding]
        public void Continue()
        {
            
        }

        [Binding]
        public void BackToMainMenu()
        {
            var operations = new Queue<LazyLoadingOperation>();
            Func<ILoadingOperation> sceneCleanupOperation = () => new SceneCleanupOperation(Constants.Scenes.Game);
            Func<ILoadingOperation> gameLoadingOperation = () => new SceneLoadingOperation(Constants.Scenes.MainMenu);
            operations.Enqueue(sceneCleanupOperation);
            operations.Enqueue(gameLoadingOperation);
            _loadingScreenProvider.LoadAndDestroyAsync(operations);
        }
    }
}