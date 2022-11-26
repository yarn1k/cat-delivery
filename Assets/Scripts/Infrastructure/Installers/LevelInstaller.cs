using UnityEngine;
using Zenject;

namespace Core.Infrastructure.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private CameraView _cameraView;

        public override void InstallBindings()
        {
            Container.Bind<CameraView>().FromInstance(_cameraView).AsSingle();
            Container.BindInterfacesTo<Level>().AsSingle();
        }
    }
}