using UnityEngine;
using Zenject;
using Core.Weapons;
using Core.Models;
using Core.Audio;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core
{
    public class LaserSpawner : IInitializable, ITickable
    {
        private readonly SignalBus _signalBus;
        private readonly Laser.Factory _factory;
        private readonly LaserGunConfig _laserConfig;
        private readonly Vector2 _attackCooldownInterval;
        private readonly float _spawnZone;
        private readonly LevelBounds _levelBounds;

        private float _timer;
        private float _spawnTime;

        public LaserSpawner(SignalBus signalBus, Laser.Factory factory, EnemySettings enemySettings, WeaponsSettings weaponSettings, LevelBounds levelBounds)
        {
            _signalBus = signalBus;
            _factory = factory;
            _attackCooldownInterval = enemySettings.AttackCooldownInterval;
            _spawnZone = enemySettings.LaserSpawnZone;
            _laserConfig = weaponSettings.LaserGunConfig;
            _levelBounds = levelBounds;
        }

        private Vector2 GetRandomPosition(bool isLeft)
        {
            int sign = isLeft ? -1 : 1;
            float y = Random.Range(-_spawnZone, _spawnZone);
            return new Vector2(sign * _levelBounds.Size.x / 2, y);
        }
        private Quaternion GetRandomRotation(bool isLeft)
        {
            float startAngle = isLeft ? 0f : 180f;
            float randomAngle = Random.Range(-35f, 35f);
            return Quaternion.AngleAxis(startAngle - randomAngle, Vector3.forward);
        }
        private void SpawnLaser(bool isLeft)
        {
            Laser laser = _factory.Create
            (
                GetRandomPosition(isLeft),
                GetRandomRotation(isLeft),
                _laserConfig.PreparationTime,
                _laserConfig.LaserLifetime
            );

            var clip = _laserConfig.LaserSounds.Random();
            SoundManager.PlayOneShot(clip.Clip, clip.Volume);

            laser.LifetimeElapsed += OnLifetimeElapsed;
            laser.Hit += OnHit;
        }

        void IInitializable.Initialize()
        {
            _spawnTime = Random.Range(_attackCooldownInterval.x, _attackCooldownInterval.y);
        }
        void ITickable.Tick()
        {
            if (Time.realtimeSinceStartup - _timer >= _spawnTime)
            {
                bool isLeft = Random.Range(0, 2) > 0.5f ? true : false;

                SpawnLaser(isLeft);
                _timer = Time.realtimeSinceStartup;
                _spawnTime = Random.Range(_attackCooldownInterval.x, _attackCooldownInterval.y);
            }
        }

        private void OnLifetimeElapsed(Laser laser)
        {
            laser.Hit -= OnHit;
            laser.Dispose();
        }
        private void OnHit(CatView target)
        {
            target.Kidnap();
            _signalBus.Fire(new CatKidnappedSignal { KidnappedCat = target });
        }
    }
}