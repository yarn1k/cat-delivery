using UnityEngine;
using Core.Infrastructure.Signals.Game;
using Zenject;
using Core.Infrastructure.Signals.Cat;

namespace Core.Cats
{
    public class CatSpawner : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private Cat.Factory _catFactory;

        [SerializeField]
        private float _spawnPositionY;
        [SerializeField]
        private Transform _level;

        [Inject]
        private void Construct(SignalBus signalBus) => _signalBus = signalBus;

        public void Start()
        {
            _signalBus.Subscribe<GameSpawnedCatSignal>(OnGameSpawnedCatSignal);
        }

        public void Disable()
        {
            _signalBus.TryUnsubscribe<GameSpawnedCatSignal>(OnGameSpawnedCatSignal);
        }

        private void OnGameSpawnedCatSignal(GameSpawnedCatSignal signal)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10.0f, 10.0f), _spawnPositionY, 0);
            var cat = _catFactory.Create();
            cat.transform.SetParent(_level);
            cat.gameObject.transform.position = spawnPosition;
            _signalBus.Fire(new CatFallingSignal { });
        }
    }
}
