using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Models;
using Core.Infrastructure.Signals.Game;

namespace Core
{
    public class CatSpawner : IInitializable, ITickable, ILateDisposable
    {
        private SignalBus _signalBus;
        private CatView.Factory _catFactory;
        private CatsSettings _settings;
        private float _timer;
        private float _spawnTime;
        private bool _enabled = true;

        [Inject]
        private void Construct(SignalBus signalBus, CatView.Factory catFactory, CatsSettings settings)
        {
            _signalBus = signalBus;
            _catFactory = catFactory;
            _settings = settings;
        }

        private void SpawnCat()
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-10.0f, 8.0f), 5.85f);
            CatView cat = _catFactory.Create();
            cat.transform.position = spawnPosition;
        }

        void IInitializable.Initialize()
        {
            _spawnTime = Random.Range(_settings.SpawnInterval.x, _settings.SpawnInterval.y);
            _signalBus.Subscribe<GameOverSignal>(OnGameOverSignal);
        }
        void ITickable.Tick()
        {
            if (_enabled && Time.realtimeSinceStartup - _timer >= _spawnTime)
            {
                SpawnCat();
                _timer = Time.realtimeSinceStartup;
                _spawnTime = Random.Range(_settings.SpawnInterval.x, _settings.SpawnInterval.y);
            }
        }
        void ILateDisposable.LateDispose()
        {
            _signalBus.TryUnsubscribe<GameOverSignal>(OnGameOverSignal);
        }

        private void OnGameOverSignal()
        {
            _enabled = false;
        }
    }
}