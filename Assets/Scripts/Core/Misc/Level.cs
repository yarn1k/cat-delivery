using Core.Player;
using Zenject;

namespace Core
{
    public class Level : IInitializable
    {
        private readonly IPauseProvider _pauseProvider;
        private readonly PlayerController _player;
        private readonly CatSpawner _catSpawner;
        private readonly LaserSpawner _laserSpawner;

        public Level(IPauseProvider pauseProvider, PlayerController player, CatSpawner catSpawner, LaserSpawner laserSpawner)
        {
            _pauseProvider = pauseProvider;
            _player = player;
            _catSpawner = catSpawner;
            _laserSpawner = laserSpawner;
        }

        void IInitializable.Initialize()
        {
            _pauseProvider.Register(_player);
            _pauseProvider.Register(_catSpawner);
            _pauseProvider.Register(_laserSpawner);
        }
    }
}
