using Zenject;

namespace Core.Infrastructure.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CameraView>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<Level>().AsSingle();
        }
    }
}