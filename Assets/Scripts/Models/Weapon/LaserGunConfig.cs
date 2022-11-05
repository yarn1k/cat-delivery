using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Weapons/Laser Gun")]
    public class LaserGunConfig : WeaponConfig
    {
        [field: SerializeField, Min(0f)] public float LaserLifetime { get; private set; }
    }
}
