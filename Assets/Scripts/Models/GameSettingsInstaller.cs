using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;

namespace Core.Models
{
    [Serializable]
    public class GameSettings
    {
        public float PreparationTime;
        [Min(0f)]
        public float FadeTime;
        public byte Lifes;
    }

    [Serializable]
    public class CatsSettings
    {
        public ushort SavedReward;
        public ushort KidnapPenalty;
        public ushort FallingReward;
        public SpriteLibraryAsset[] Skins;
        [Range(0f, 10f)]
        public float CatsFallingSpeed;
        [Range(0f, 10f)]
        public float CatsKidnapSpeed;
        [Range(0f, 10f)]
        public float CatsSaveSpeed;
        [Editor.MinMaxSlider(0f, 10f, width: 45f)]
        public Vector2 SpawnInterval;
        [Min(0f)]
        public float SpawnWidth;
    }

    [Serializable]
    public class PlayerSettings
    {
        [Min(0f)]
        public float ReloadTime;
        [Range(0f, 10f)]
        public float MovementSpeed;
        [Range(0f, 20f)]
        public float JumpForce;
    }

    [Serializable]
    public class EnemySettings
    {
        [Range(0f, 10f)]
        public float MovementSpeed;
        [Editor.MinMaxSlider(0f, 10f, width: 45f)]
        public Vector2 AttackCooldownInterval;
        [Min(0f)]
        public float LaserSpawnHeight;
        [Range(0f, 45f)]
        public float LaserAngle;
    }

    [Serializable]
    public class WeaponsSettings
    {
        public BulletGunConfig BulletGunConfig;
        public LaserGunConfig LaserGunConfig;
    }

    [CreateAssetMenu(menuName = "Configuration/Settings/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField]
        private GameSettings _gameSettings;
        [SerializeField]
        private PlayerSettings _playerSettings;
        [SerializeField]
        private CatsSettings _catsSettings;
        [SerializeField]
        private EnemySettings _enemySettings;
        [SerializeField]
        private WeaponsSettings _weaponsSettings;

        public CatsSettings CatsSettings => _catsSettings;
        public EnemySettings EnemySettings => _enemySettings;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<ILogger>().To<StandaloneLogger>().AsCached();

            Container.Bind<GameSettings>().FromInstance(_gameSettings).IfNotBound();
            Container.Bind<PlayerSettings>().FromInstance(_playerSettings).IfNotBound();
            Container.Bind<CatsSettings>().FromInstance(_catsSettings).IfNotBound();
            Container.Bind<EnemySettings>().FromInstance(_enemySettings).IfNotBound();
            Container.Bind<WeaponsSettings>().FromInstance(_weaponsSettings).IfNotBound();
        }
    }
}
