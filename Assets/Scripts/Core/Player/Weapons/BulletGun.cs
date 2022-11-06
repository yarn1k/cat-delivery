using System;
using Core.Cats;
using Core.Models;
using UnityEngine;

namespace Core.Weapons
{
    public class BulletGun : IWeapon
    {
        private readonly BulletGunModel _model;
        private readonly Bullet.Factory _bulletFactory;

        public event Action<CatView> Hit;

        private AudioSource _audioSource;
        private AudioPlayerSettings _audioPlayerSettings;

        public BulletGun(BulletGunModel model, Bullet.Factory bulletFactory, AudioSource audioSource, AudioPlayerSettings audioPlayerSettings)
        {
            _model = model;
            _bulletFactory = bulletFactory;
            _audioSource = audioSource;
            _audioPlayerSettings = audioPlayerSettings;
        }

        public void Shoot()
        {
            if (!_model.Cooldown.IsOver) return;

            _audioSource.PlayOneShot(_audioPlayerSettings.PlayerShoot);

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
