using Zenject;
using Core.Infrastructure.Signals.Cat;
using UnityEngine;
using Core.Cats;
using Unity.VisualScripting.Antlr3.Runtime.Misc;

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

            Container.BindFactory<Cat, Cat.Factory>().FromComponentInNewPrefab(_cats[Random.Range(0, (_cats.Length - 1))]);

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<CatFallingSignal>().ToMethod(() => Logger.Log("CatFallingSignal", LogType.Signal));
            Container.BindSignal<CatSavedSignal>().ToMethod(() => Logger.Log("CatSavedSignal", LogType.Signal));
            Container.BindSignal<CatKidnappedSignal>().ToMethod(() => Logger.Log("CatKidnappedSignal", LogType.Signal));
#endif
        }
    }
}