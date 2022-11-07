using System;
using Core.Cats;
using Core.Infrastructure.Signals.Player;
using Core.Models;
using UnityEngine;
using Zenject;

namespace Core.Weapons
{
    public class BulletGun : IWeapon
    {
        private readonly BulletGunModel _model;
        private readonly Bullet.Factory _bulletFactory;

        public event Action<CatView> Hit;
        public float Cooldown => _model.CooldownTime;

        private AudioSource _audioSource;
        private AudioPlayerSettings _audioPlayerSettings;

        public BulletGun(BulletGunModel model, Bullet.Factory bulletFactory, AudioSource audioSource, AudioPlayerSettings audioPlayerSettings)
        {
            _model = model;
            _bulletFactory = bulletFactory;
            _audioSource = audioSource;
            _audioPlayerSettings = audioPlayerSettings;
        }

        public bool TryShoot()
        {
            if (!_model.Cooldown.IsOver) return false;

            Audio playerShoot = _audioPlayerSettings.PlayerShoot;
            _audioSource.PlayOneShot(playerShoot.Clip, playerShoot.Volume);

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
        private void OnBulletHit(Bullet bullet, CatView target)
        {
            DisposeBullet(bullet);
            Hit?.Invoke(target);
        }
    }
}
