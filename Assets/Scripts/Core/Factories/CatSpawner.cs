using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Models;

namespace Core
{
    public class CatSpawner : IInitializable, ITickable
    {
        private CatView.Factory _catFactory;
        private CatsSettings _settings;
        private float _timer;
        private float _spawnTime;

        [Inject]
        private void Construct(CatView.Factory catFactory, CatsSettings settings)
        {
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
        }
        void ITickable.Tick()
        {
            if (Time.realtimeSinceStartup - _timer >= _spawnTime)
            {
                SpawnCat();
                _timer = Time.realtimeSinceStartup;
                _spawnTime = Random.Range(_settings.SpawnInterval.x, _settings.SpawnInterval.y);
            }
        }
    }
}