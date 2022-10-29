using System;
using UnityEngine;
using Zenject;
using Editor;

namespace Core.Models
{
    [Serializable]
    public class GameSettings
    {
   
    }

    [Serializable]
    public struct CatsSettings
    {
        public GameObject CatPrefab;
        [Range(0f, 10f)]
        public float CatsFallingSpeed;
        [MinMaxSlider(0f, 20f, width: 45f)]
        public Vector2 SpawnInterval;
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
