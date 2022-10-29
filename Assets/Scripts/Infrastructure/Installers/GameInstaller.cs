using UnityEngine;
using Zenject;
using Core.Match;
using Core.Enemy;
using Core.Cats;
using Core.Models;
using Core.Input;
using Core.Infrastructure.Signals.Game;
using Core.Infrastructure.Signals.Cats;
using Core.Enemy.States;

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
            Container.DeclareSignal<GameSpawnedCatSignal>();
            Container.DeclareSignal<EnemyWantsAttackSignal>();
            Container.DeclareSignal<GameSpawnedLaserSignal>();
            Container.DeclareSignal<GameScoreChangedSignal>();
            Container.DeclareSignal<GameOverSignal>();

#if UNITY_EDITOR
            Container.BindSignal<EnemyWantsAttackSignal>().ToMethod(() => Logger.Log("EnemyWantsAttackSignal", LogType.Signal));
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
            Container.DeclareSignal<CatSavedSignal>();
            Container.DeclareSignal<CatKidnappedSignal>();
        }
        private void BindEnemy()
        {
            Container.BindInterfacesAndSelfTo<EnemyStateMachine>().AsSingle();
            Container.BindFactory<Laser, Laser.Factory>().FromComponentInNewPrefab(_laser);
        }
        private void BindPlayer()
        {
            Container.BindInterfacesAndSelfTo<StandaloneInputController>().AsSingle();
        }
        private void BindPools()
        {
            Container.BindFactory<CatView, CatView.Factory>().FromMonoPoolableMemoryPool(x => x
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