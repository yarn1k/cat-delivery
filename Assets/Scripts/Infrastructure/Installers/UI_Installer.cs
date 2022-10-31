using Zenject;
using Core.Infrastructure.Signals.UI;

namespace Core.Infrastructure.Installers
{
    public class UI_Installer : MonoInstaller
    {
        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<UIScoreChangedSignal>();
        }
    }
}
