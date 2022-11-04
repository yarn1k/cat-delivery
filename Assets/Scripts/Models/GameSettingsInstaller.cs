using System;
using UnityEngine;
using Zenject;

namespace Core.Models
{
    [Serializable]
    public class GameSettings
    {
        public ushort GameTime;
        public ushort SavedReward;
        public ushort KidnapPenalty;
    }

    [Serializable]
    public class CatsSettings
    {
        public GameObject CatPrefab;
        [Range(0f, 10f)]
        public float CatsFallingSpeed;
        [Range(0f, 10f)]
        public float CatsKidnapSpeed;
        [Range(0f, 10f)]
        public float CatsSaveSpeed;
        [Editor.MinMaxSlider(0f, 10f, width: 45f)]
        public Vector2 SpawnInterval;
    }

    [Serializable]
    public class PlayerSettings
    {
        public GameObject BulletPrefab;
        [Min(0f)]
        public float BulletForce;
        [Range(0f, 1f)]
        public float BulletLifetime;
        [Min(0f)]
        public float ReloadTime;
        [Range(0f, 10f)]
        public float MovementSpeed;
        [Range(0f, 50f)]
        public float JumpForce;
    }

    [Serializable]
    public class EnemySettings
    {
        [Editor.MinMaxSlider(0f, 10f, width: 45f)]
        public Vector2 LaserCooldownInterval;
    }

    [CreateAssetMenu(fileName = "Game Settings", menuName = "Installers/Game Settings")]
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

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<ILogger>().To<StandaloneLogger>().AsCached();

            Container.BindInstances(_gameSettings, _playerSettings, _catsSettings, _enemySettings);
        }
    }
}
