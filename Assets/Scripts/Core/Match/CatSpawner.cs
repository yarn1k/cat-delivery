using System;
using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Enemy;
using Core.Infrastructure.Signals.Game;
using Core.Models;

namespace Core
{
    public class CatSpawner : MonoBehaviour, IDisposable
    {
        private SignalBus _signalBus;
        private Cat.Factory _catFactory;
        private CatsSettings _settings;
        private float _timer;
        private float _spawnTime;

        [Inject]
        private Laser.Factory _laserFactory;
        [SerializeField]
        private Transform _enemyFirePoint;

        [Inject]
        private void Construct(SignalBus signalBus, Cat.Factory catFactory, CatsSettings settings)
        {
            _signalBus = signalBus;
            _catFactory = catFactory;
            _settings = settings;
        }

        public void Start()
        {
            _spawnTime = UnityEngine.Random.Range(_settings.SpawnInterval.x, _settings.SpawnInterval.y);
            _signalBus.Subscribe<GameSpawnedLaserSignal>(OnGameSpawnedLaserSignal);
        }
        private void Update()
        {
            if (Time.realtimeSinceStartup - _timer >= _spawnTime)
            {
                SpawnCat();
                _timer = Time.realtimeSinceStartup;
                _spawnTime = UnityEngine.Random.Range(_settings.SpawnInterval.x, _settings.SpawnInterval.y);
            }
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<GameSpawnedLaserSignal>(OnGameSpawnedLaserSignal);
        }

        private void SpawnCat()
        {
            Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(-10.0f, 8.0f), 5.85f);
            Cat cat = _catFactory.Create();
            cat.transform.position = spawnPosition;
        }

        private void OnGameSpawnedLaserSignal(GameSpawnedLaserSignal signal)
        {
            Vector3 spawnPosition = new Vector3(0, UnityEngine.Random.Range(0f, 3.0f), 0);
            Laser laser = _laserFactory.Create();
            laser.transform.position = spawnPosition;
        }
    }
}