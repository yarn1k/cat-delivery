using Zenject;
using Core.Infrastructure.Signals.Game;
using Core.Match;

namespace Core.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<GameSpawnedCatSignal>();

        #if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<GameSpawnedCatSignal>().ToMethod(() => Logger.Log("GameSpawnedCatSignal", LogType.Signal));
        #endif

            Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }
    }
}