using UnityEngine;
using Core.Models;

namespace Core.Weapons
{
    public class BulletGunModel
    {
        public readonly float CooldownTime;
        public readonly BulletGunConfig BulletGunConfig;
        public readonly Transform FirePoint;
        public readonly Cooldown Cooldown;

        public BulletGunModel(float cooldownTime, BulletGunConfig config, Transform firePoint)
        {
            CooldownTime = cooldownTime;
            BulletGunConfig = config;
            FirePoint = firePoint;
            Cooldown = new Cooldown();
        }
    }
}
