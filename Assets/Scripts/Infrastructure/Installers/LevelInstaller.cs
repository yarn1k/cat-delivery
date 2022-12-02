using UnityEngine;
using Zenject;
using Core.UI;

namespace Core.Infrastructure.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private CameraView _cameraView;
        [SerializeField]
        private Countdown _countdown;
        public override void InstallBindings()
        {
            Container.Bind<CameraView>().FromInstance(_cameraView).AsSingle();
            Container.Bind<Countdown>().FromInstance(_countdown).AsSingle();

            Container.BindInterfacesTo<Level>().AsSingle();
        }
    }
}