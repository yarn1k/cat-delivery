using Zenject;
using Core.Infrastructure.Signals.Game;
using Core.Match;
using UnityEngine;
using Core.Enemy;
using Zenject.SpaceFighter;

namespace Core.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;
        [SerializeField]
        private Laser _laser;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<GameSpawnedCatSignal>();
            Container.DeclareSignal<EnemyWantsAttackSignal>();
            Container.DeclareSignal<GameSpawnedLaserSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<GameSpawnedCatSignal>().ToMethod(() => Logger.Log("GameSpawnedCatSignal", LogType.Signal));
            Container.BindSignal<GameSpawnedLaserSignal>().ToMethod(() => Logger.Log("GameSpawnedLaserSignal", LogType.Signal));
#endif

            Container.BindFactory<Laser, Laser.Factory>().FromComponentInNewPrefab(_laser);

            Container.Bind<IInitializable>().To<GameController>().AsSingle();
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();

            //Container.BindInterfacesAndSelfTo<GameController>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }
    }
}