using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Weapons/Laser Gun")]
    public class LaserGunConfig : WeaponConfig
    {
        [field: SerializeField, Min(0f)] public float PreparationTime { get; private set; }
        [field: SerializeField, Min(0f)] public float LaserLifetime { get; private set; }
        [field: SerializeField] public GameObject KidnappingEffect { get; private set; }
        [field: Space, SerializeField] public AudioSound[] LaserSounds { get; private set; }
    }
}
