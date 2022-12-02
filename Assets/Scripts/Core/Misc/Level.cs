using System.Collections;
using UnityEngine;
using Zenject;
using Core.Infrastructure.Signals.Game;
using Core.Models;
using Core.Player;

namespace Core
{
    public class Level : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly IPauseProvider _pauseProvider;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly PlayerFactory _playerFactory;
        private readonly CatSpawner _catSpawner;
        private readonly LaserSpawner _laserSpawner;
        private readonly GameSettings _gameSettings;
        private PlayerController _playerController;

        public Level(
            SignalBus signalBus,
            IPauseProvider pauseProvider, 
            ICoroutineRunner coroutineRunner,
            PlayerFactory playerFactory, 
            CatSpawner catSpawner, 
            LaserSpawner laserSpawner,
            GameSettings gameSettings)
        {
            _signalBus = signalBus;
            _pauseProvider = pauseProvider;
            _coroutineRunner = coroutineRunner;
            _playerFactory = playerFactory;
            _catSpawner = catSpawner;
            _laserSpawner = laserSpawner;
            _gameSettings = gameSettings;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<GameOverSignal>(OnGameOver);
            _pauseProvider.Register(_catSpawner);
            _pauseProvider.Register(_laserSpawner);
            SpawnPlayer();
            _coroutineRunner.StartCoroutine(StartGame());
        }

        private void SpawnPlayer()
        {
            _playerController = _playerFactory.Create();
            _playerController.Disable();
            _pauseProvider.Register(_playerController);
        }
        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(_gameSettings.PreparationTime);
            _catSpawner.SetEnabled(true);
            _laserSpawner.SetEnabled(true);
            _playerController.Enable();
        }

        private void OnGameOver()
        {
            _catSpawner.SetEnabled(false);
            _catSpawner.Dispose();
            _laserSpawner.SetEnabled(false);
            _playerController.Disable();
        }
    }
}
