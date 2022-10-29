using UnityEngine;
using Zenject;
using Core.Infrastructure.Signals.Cats;
using Core.Cats;

namespace Core.Infrastructure.Installers
{
    public class CatInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;
        [SerializeField]
        private Cat[] _cats;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<CatFallingSignal>();
            Container.DeclareSignal<CatSavedSignal>();
            Container.DeclareSignal<CatKidnappedSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<CatFallingSignal>().ToMethod(() => Logger.Log("CatFallingSignal", LogType.Signal));
            Container.BindSignal<CatSavedSignal>().ToMethod(() => Logger.Log("CatSavedSignal", LogType.Signal));
            Container.BindSignal<CatKidnappedSignal>().ToMethod(() => Logger.Log("CatKidnappedSignal", LogType.Signal));
#endif
        }
    }
}