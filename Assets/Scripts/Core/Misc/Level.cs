using Core.Infrastructure.Signals.Game;
using Core.Player;
using Zenject;

namespace Core
{
    public class Level : IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly IPauseProvider _pauseProvider;
        private readonly PlayerFactory _playerFactory;
        private readonly CatSpawner _catSpawner;
        private readonly LaserSpawner _laserSpawner;

        private PlayerController _playerController;

        public Level(
            SignalBus signalBus,
            IPauseProvider pauseProvider, 
            PlayerFactory playerFactory, 
            CatSpawner catSpawner, 
            LaserSpawner laserSpawner)
        {
            _signalBus = signalBus;
            _pauseProvider = pauseProvider;
            _playerFactory = playerFactory;
            _catSpawner = catSpawner;
            _laserSpawner = laserSpawner;
        }

        void IInitializable.Initialize()
        {
            _signalBus.Subscribe<GameOverSignal>(OnGameOver);
            _pauseProvider.Register(_catSpawner);
            _pauseProvider.Register(_laserSpawner);
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            _playerController = _playerFactory.Create();
            _playerController.Enable();
            _pauseProvider.Register(_playerController);
        }

        private void OnGameOver()
        {
            _signalBus.TryUnsubscribe<GameOverSignal>(OnGameOver);
            _playerController.Disable();
        }
    }
}
