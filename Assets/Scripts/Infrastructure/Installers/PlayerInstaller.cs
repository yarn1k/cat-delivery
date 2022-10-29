using Zenject;
using Core.Infrastructure.Signals.Player;
using Core.Match;
using UnityEngine;
using Core.Cats;

namespace Core.Infrastructure.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<PlayerFiredSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<PlayerFiredSignal>().ToMethod(() => Logger.Log("PlayerFiredSignal", LogType.Signal));
        #endif
        }
    }
}