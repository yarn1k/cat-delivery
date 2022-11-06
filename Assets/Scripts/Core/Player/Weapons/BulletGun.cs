using System;
using Core.Cats;

namespace Core.Weapons
{
    public class BulletGun : IWeapon
    {
        private readonly BulletGunModel _model;
        private readonly Bullet.Factory _bulletFactory;

        public event Action<CatView> Hit;

        public BulletGun(BulletGunModel model, Bullet.Factory bulletFactory)
        {
            _model = model;
            _bulletFactory = bulletFactory;
        }

        public void Shoot()
        {
            if (!_model.Cooldown.IsOver) return;

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
        }

        private void DisposeBullet(Bullet bullet)
        {
            bullet.Hit -= OnBulletHit;
            bullet.LifetimeElapsed -= DisposeBullet;
            bullet.Dispose();
        }
        private void OnBulletHit(Bullet bullet, CatView target)
        {
            DisposeBullet(bullet);
            Hit?.Invoke(target);
        }
    }
}
