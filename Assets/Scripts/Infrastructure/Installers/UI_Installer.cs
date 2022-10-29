using Core.Infrastructure.Signals.Game;
using Core.Infrastructure.Signals.UI;
using Zenject;

namespace Core.Infrastructure.Installers
{
    public class UI_Installer : MonoInstaller
    {
        [Inject]
        private ILogger Logger;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<UIScoreChangedSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<UIScoreChangedSignal>().ToMethod(() => Logger.Log("UIScoreChangedSignal", LogType.Signal));
#endif
        }
    }
}
