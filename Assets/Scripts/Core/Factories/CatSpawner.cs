using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Models;

namespace Core
{
    public class CatSpawner : MonoBehaviour
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

        public void Start()
        {
            _spawnTime = UnityEngine.Random.Range(_settings.SpawnInterval.x, _settings.SpawnInterval.y);
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

        private void SpawnCat()
        {
            Vector2 spawnPosition = new Vector2(UnityEngine.Random.Range(-10.0f, 8.0f), 5.85f);
            CatView cat = _catFactory.Create();
            cat.transform.position = spawnPosition;
        }
    }
}