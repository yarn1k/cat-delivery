using System;
using Core.Audio;
using Core.Cats;

namespace Core.Weapons
{
    public class LaserGun : IWeapon
    {
        private readonly LaserGunModel _model;
        private readonly Laser.Factory _laserFactory;

        public float ReloadTime => _model.ReloadTime;
        public event Action Missed;
        public event Action<CatView> Hit;

        public LaserGun(LaserGunModel model, Laser.Factory laserFactory)
        {
            _model = model;
            _laserFactory = laserFactory;
        }

        public bool TryShoot()
        {
            if (!_model.Cooldown.IsOver) return false;
            
            Laser laser = _laserFactory.Create
            (
                _model.FirePoint.position,
                _model.FirePoint.rotation,
                _model.LaserGunConfig.PreparationTime,
                _model.LaserGunConfig.LaserLifetime
            );

            var clip = _model.LaserGunConfig.LaserSounds.Random();
            SoundManager.PlayOneShot(clip.Clip, clip.Volume);

            laser.LifetimeElapsed += OnLifetimeElapsed;
            laser.Hit += Hit.Invoke;

            _model.Cooldown.Run(_model.ReloadTime);
            return true;
        }

        private void OnLifetimeElapsed(Laser laser)
        {
            if (!laser.HitAnyTarget) Missed?.Invoke();
            laser.LifetimeElapsed -= OnLifetimeElapsed;
            laser.Hit -= Hit.Invoke;
            laser.Dispose();
        }
    }
}
