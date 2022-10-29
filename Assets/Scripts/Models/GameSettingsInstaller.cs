using System;
using UnityEngine;
using Zenject;

namespace Core.Models
{
    [Serializable]
    public class GameSettings
    {
        [SerializeField]
        private float _catsFallingSpeed;
        [SerializeField]
        private float _bulletForce;
        [SerializeField]
        private float _reloadTime;
        [SerializeField]
        private float _movementSpeed;

        public float CatsFallingSpeed => _catsFallingSpeed;
        public float BulletForce => _bulletForce;
        public float ReloadTime => _reloadTime;
        public float MovementSpeed => _movementSpeed;
    }

    [Serializable]
    public struct CatsSettings
    {
        public GameObject CatPrefab;
        [Range(0f, 10f)]
        public float CatsFallingSpeed;
    }

    [Serializable]
    public struct PlayerSettings
    {
        public GameObject BulletPrefab;
        [Range(0f, 10f)]
        public float ReloadTime;
        [Range(0f, 10f)]
        public float MovementSpeed;
    }

    [Serializable]
    public struct EnemySettings
    {

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

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<ILogger>().To<StandaloneLogger>().AsCached();

            Container.BindInstances(_gameSettings, _playerSettings, _catsSettings);
        }
    }
}
