using Core.Infrastructure.Signals.Game;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Core.Match
{
    public class AsyncProcessor : MonoBehaviour
    {
    }

    public class GameController : IInitializable, ILateDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly ILogger _logger;
        private AsyncProcessor _asyncProcessor;

        public GameController(SignalBus signalBus, ILogger logger, AsyncProcessor asyncProcessor)
        {
            _signalBus = signalBus;
            _logger = logger;
            _asyncProcessor = asyncProcessor;
        }

        public void Initialize()
        {
            _asyncProcessor.StartCoroutine(RandomCatSpawn());
            _asyncProcessor.StartCoroutine(RandomLaserSpawn());
        }

        public void LateDispose()
        {
            _asyncProcessor.StopCoroutine(RandomCatSpawn());
            _asyncProcessor.StopCoroutine(RandomLaserSpawn());
        }

        IEnumerator RandomCatSpawn()
        {
            while (true)
            {
                float timer = Random.Range(1, 6);
                yield return new WaitForSeconds(timer);
                _signalBus.Fire(new GameSpawnedCatSignal { SpawnPositionY = 5.85f });
            }
        }

        IEnumerator RandomLaserSpawn()
        {
            while (true)
            {
                float timer = Random.Range(1, 10);
                yield return new WaitForSeconds(timer);
                _signalBus.Fire(new GameSpawnedLaserSignal { });
            }
        }
    }
}
