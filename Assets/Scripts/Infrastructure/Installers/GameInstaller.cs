using UnityEngine;
using Zenject;
using Core.Match;
using Core.Enemy;
using Core.Cats;
using Core.Models;
using Core.Infrastructure.Signals.Game;
using Core.Infrastructure.Signals.Cats;

namespace Core.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [Inject]
        private ILogger Logger;
        [SerializeField]
        private Laser _laser;

        [Inject]
        private CatsSettings _catsSettings;
        [Inject]
        private PlayerSettings _playerSettings;

        public override void InstallBindings()
        {
            // Declare all signals
            Container.DeclareSignal<GameSpawnedCatSignal>();
            Container.DeclareSignal<EnemyWantsAttackSignal>();
            Container.DeclareSignal<GameSpawnedLaserSignal>();
            Container.DeclareSignal<GameScoreChangedSignal>();
            Container.DeclareSignal<GameOverSignal>();

#if UNITY_EDITOR
            // Include these just to ensure BindSignal works
            Container.BindSignal<GameSpawnedCatSignal>().ToMethod(() => Logger.Log("GameSpawnedCatSignal", LogType.Signal));
            Container.BindSignal<EnemyWantsAttackSignal>().ToMethod(() => Logger.Log("EnemyWantsAttackSignal", LogType.Signal));
            Container.BindSignal<GameSpawnedLaserSignal>().ToMethod(() => Logger.Log("GameSpawnedLaserSignal", LogType.Signal));
            Container.BindSignal<GameOverSignal>().ToMethod(() => Logger.Log("GameOverSignal", LogType.Signal));
#endif                    

            Container.Bind<IInitializable>().To<GameController>().AsSingle();
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();

            BindCats();
            BindEnemy();
            BindPlayer();
            BindPools();
        }

        private void BindCats()
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
        private void BindEnemy()
        {
            Container.BindFactory<Laser, Laser.Factory>().FromComponentInNewPrefab(_laser);
        }
        private void BindPlayer()
        {
            Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle();
        }
        private void BindPools()
        {
            Container.BindFactory<Cat, Cat.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<CatInstaller>(_catsSettings.CatPrefab)
                .UnderTransformGroup("Cats"));

            Container.BindFactory<Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_playerSettings.BulletPrefab)
                .UnderTransformGroup("Bullets"));
        }
    }
}