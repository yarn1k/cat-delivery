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
        public float CatsFallingSpeed => _catsFallingSpeed;
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
