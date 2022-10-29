using Zenject;
using Core.Infrastructure.Signals.Game;
using UnityEngine;
using Core.Match;
using Core.Enemy;
using Core.Cats;
using Core.Models;

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

            BindEnemy();
            BindPlayer();
            BindPools();
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
                .FromComponentInNewPrefab(_catsSettings.CatPrefab)
                .UnderTransformGroup("Cats"));

            Container.BindFactory<Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_playerSettings.BulletPrefab)
                .UnderTransformGroup("Bullets"));
        }
    }
}