using Core.Models;
using UnityEngine;

namespace Core.Weapons
{
    public class LaserGunModel
    {
        public readonly float ReloadTime;
        public readonly LaserGunConfig LaserGunConfig;
        public readonly Transform FirePoint;
        public readonly Cooldown Cooldown;

        public LaserGunModel(float reloadTime, LaserGunConfig laserGunConfig, Transform firePoint)
        {
            ReloadTime = reloadTime;
            LaserGunConfig = laserGunConfig;
            FirePoint = firePoint;
            Cooldown = new Cooldown();
        }
    }
}
