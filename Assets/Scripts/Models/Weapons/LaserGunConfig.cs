using UnityEngine;

namespace Core.Models
{
    [CreateAssetMenu(menuName = "Configuration/Weapons/Laser Gun")]
    public class LaserGunConfig : WeaponConfig
    {
        [field: SerializeField, Min(0f)] public float PreparationTime { get; private set; }
        [field: SerializeField, Min(0f)] public float LaserLifetime { get; private set; }
        [field: SerializeField] public GameObject LaserBeamPrefab { get; private set; }

        [field: Header("Post-Effect")]
        [field: SerializeField] public bool CameraShake { get; private set; }
        [field: SerializeField, Range(0f, 0.5f)] public float ShakeFrequence { get; private set; }
        [field: SerializeField, Min(0f)] public float ShakeDuration { get; private set; }

        [field: Space, SerializeField] public AudioSound[] LaserSounds { get; private set; }
    }
}
