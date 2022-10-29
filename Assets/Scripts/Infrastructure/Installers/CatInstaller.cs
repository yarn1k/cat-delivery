using Zenject;
using Core.Cats.States;

namespace Core.Infrastructure.Installers
{
    public class CatInstaller : Installer<CatInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CatStateMachine>().AsSingle();
        }
    }
}