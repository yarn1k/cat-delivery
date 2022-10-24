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

    [CreateAssetMenu(fileName = "Game Settings", menuName = "Installers/Game Settings")]
    public class GameSettingsInstaller : ScriptableObjectInstaller<GameSettingsInstaller>
    {
        [SerializeField]
        private GameSettings _gameSettings;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.Bind<ILogger>().To<StandaloneLogger>().AsCached();

            Container.BindInstance(_gameSettings).IfNotBound();
        }
    }
}
