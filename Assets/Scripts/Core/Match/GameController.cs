using Core.Infrastructure.Signals.Game;
using UnityEngine;
using Zenject;

namespace Core.Match
{
    public class GameController : ITickable
    {
        private readonly SignalBus _signalBus;
        private readonly ILogger _logger;
        private float _timer = Random.Range(1, 6);

        public GameController(SignalBus signalBus, ILogger logger)
        {
            _signalBus = signalBus;
            _logger = logger;
        }

        public void Tick()
        {
            RandomCatSpawn();
        }

        private void RandomCatSpawn()
        {
            if (_timer > 0f) _timer -= Time.deltaTime;
            else
            {
                _signalBus.Fire(new GameSpawnedCatSignal { });
                _timer = Random.Range(1, 6);
            }
        }
    }
}
