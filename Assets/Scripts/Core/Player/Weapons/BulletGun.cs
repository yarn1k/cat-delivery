using System;
using UnityEngine;
using Core.Cats;

namespace Core.Weapons
{
    public class BulletGun : IWeapon
    {
        private readonly BulletGunModel _model;
        private readonly Bullet.Factory _bulletFactory;
        private readonly Bullet.ExplosionFactory _explosionFactory;

        public float Cooldown => _model.CooldownTime;
        public event Action<CatView> Hit;

        public BulletGun(BulletGunModel model, Bullet.Factory bulletFactory, Bullet.ExplosionFactory explosionFactory)
        {
            _model = model;
            _bulletFactory = bulletFactory;
            _explosionFactory = explosionFactory;
        }

        public bool TryShoot()
        {
            if (!_model.Cooldown.IsOver) return false;

            Bullet bullet = _bulletFactory.Create
            (
                _model.FirePoint.position,
                _model.FirePoint.rotation,
                _model.BulletGunConfig.BulletForce,
                _model.BulletGunConfig.BulletLifetime
            );
            bullet.LifetimeElapsed += DisposeBullet;
            bullet.Hit += OnBulletHit;

            _model.Cooldown.Run(_model.CooldownTime);
            return true;
        }

        private void DisposeBullet(Bullet bullet)
        {
            bullet.Hit -= OnBulletHit;
            bullet.LifetimeElapsed -= DisposeBullet;
            bullet.Dispose();
        }
        private void CreateExplosion(Vector2 position)
        {
            int rand = UnityEngine.Random.Range(0, _model.BulletGunConfig.Explosions.Length);
            GameObject prefab = _model.BulletGunConfig.Explosions[rand];
            GameObject explosion = _explosionFactory.Create(prefab, position);
            GameObject.Destroy(explosion, 0.8f);
        }
        private void OnBulletHit(Bullet bullet, CatView target)
        {
            CreateExplosion(bullet.Center);
            DisposeBullet(bullet);
            Hit?.Invoke(target);
        }
    }
}
