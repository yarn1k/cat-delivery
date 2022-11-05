using Core.Models;
using UnityEngine;

namespace Core.Weapons
{
    public class LaserGunModel
    {
        public readonly float CooldownTime;
        public readonly LaserGunConfig LaserGunConfig;
        public readonly Transform FirePoint;
        public readonly Cooldown Cooldown;

        public LaserGunModel(float cooldownTime, LaserGunConfig laserGunConfig, Transform firePoint)
        {
            CooldownTime = cooldownTime;
            LaserGunConfig = laserGunConfig;
            FirePoint = firePoint;
            Cooldown = new Cooldown();
        }
    }
}
