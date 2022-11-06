using System;
using Core.Cats;

namespace Core.Weapons
{
    public class LaserGun : IWeapon
    {
        private readonly LaserGunModel _model;
        private readonly Laser.Factory _laserFactory;

        public event Action<CatView> Hit;

        public LaserGun(LaserGunModel model, Laser.Factory laserFactory)
        {
            _model = model;
            _laserFactory = laserFactory;
        }

        public void Shoot()
        {
            if (!_model.Cooldown.IsOver) return;
            
            Laser laser = _laserFactory.Create
            (
                _model.FirePoint.position,
                _model.FirePoint.rotation,
                _model.LaserGunConfig.PreparationTime,
                _model.LaserGunConfig.LaserLifetime
            );
            laser.LifetimeElapsed += OnLifetimeElapsed;
            laser.Hit += Hit.Invoke;

            _model.Cooldown.Run(_model.CooldownTime);
        }

        private void OnLifetimeElapsed(Laser laser)
        {
            laser.Hit -= Hit.Invoke;
            laser.Dispose();
        }
    }
}
