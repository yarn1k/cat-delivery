using UnityEngine;
using Core.Models;

namespace Core.Weapons
{
    public class BulletGunModel
    {
        public readonly float ReloadTime;
        public readonly BulletGunConfig BulletGunConfig;
        public readonly Transform FirePoint;
        public readonly Cooldown Cooldown;

        public BulletGunModel(float reloadTime, BulletGunConfig config, Transform firePoint)
        {
            ReloadTime = reloadTime;
            BulletGunConfig = config;
            FirePoint = firePoint;
            Cooldown = new Cooldown();
        }
    }
}
