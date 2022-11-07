using UnityEngine;
using Zenject;
using Core.Match;
using Core.Cats;
using Core.Models;
using Core.Input;
using Core.Infrastructure.Signals.Game;
using Core.Infrastructure.Signals.Cats;
using Core.UI;
using Core.Weapons;
using Core.Player;
using Core.Enemy;
using Core.Infrastructure.Signals.Player;

namespace Core.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller
    {
        [SerializeField]
        private AudioSource _audioSource;
        [SerializeField]
        private LevelBounds _levelBounds;
        [SerializeField]
        private GameObject _labelVFXPrefab;
        [SerializeField]
        private GameObject _playerPrefab;
        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField]
        private BulletGunConfig _bulletGunConfig;
        [SerializeField]
        private LaserGunConfig _laserGunConfig;

        [Inject]
        private CatsSettings _catsSettings;
        [Inject]
        private PlayerSettings _playerSettings;
        [Inject]
        private EnemySettings _enemySettings;

        public override void InstallBindings()
        {
            Container.DeclareSignal<GameScoreChangedSignal>();
            Container.DeclareSignal<GameOverSignal>();

            Container.Bind<AudioSource>().FromInstance(_audioSource).AsSingle();
            Container.Bind<LevelBounds>().FromInstance(_levelBounds).AsSingle();
            Container.Bind<IInitializable>().To<GameController>().AsSingle();
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();

            BindPools();
            BindCats();
            BindEnemy();
            BindPlayer();
            BindUI();
        }

        private void BindCats()
        {
            Container.DeclareSignal<CatFellSignal>();
            Container.DeclareSignal<CatSavedSignal>();
            Container.DeclareSignal<CatKidnappedSignal>();
            Container.BindInterfacesTo<CatSpawner>().AsSingle();
        }
        private void BindEnemy()
        {
            Vector3 position = new Vector3(8f, 3f, 0f);
            EnemyView view = Container.InstantiatePrefabForComponent<EnemyView>(_enemyPrefab, position, Quaternion.identity, null);
            EnemyModel model = new EnemyModel(_enemySettings.MovementSpeed, _enemySettings.AttackCooldownInterval);
            EnemyController controller = Container.Instantiate<EnemyController>(new object[] { model, view });

            LaserGunModel laserGunModel = new LaserGunModel(model.AttackCooldownInterval.x, _laserGunConfig, view.FirePoint);
            LaserGun laserGun = Container.Instantiate<LaserGun>(new object[] { laserGunModel });

            controller.SetPrimaryWeapon(laserGun);

            Container.BindInterfacesTo<EnemyController>().FromInstance(controller).AsSingle();
        }
        private void BindUI()
        {
            Container.Bind<Score>().AsSingle();
        }
        private void BindPlayer()
        {
            Container.BindInterfacesTo<StandaloneInputController>().AsSingle();
            PlayerView view = Container.InstantiatePrefabForComponent<PlayerView>(_playerPrefab);
            PlayerModel model = Container.Instantiate<PlayerModel>(new object[] { _playerSettings.ReloadTime, _playerSettings.MovementSpeed, _playerSettings.JumpForce });
            PlayerController controller = Container.Instantiate<PlayerController>(new object[] { model, view });

            BulletGunModel bulletGunModel = new BulletGunModel(model.ReloadTime, _bulletGunConfig, view.FirePoint);
            BulletGun bulletGun = Container.Instantiate<BulletGun>(new object[] { bulletGunModel });

            controller.SetPrimaryWeapon(bulletGun);

            Container.BindInterfacesTo<PlayerController>().FromInstance(controller).AsSingle();
        }
        private void BindPools()
        {
            Container.BindFactory<Vector2, Quaternion, float, Laser, Laser.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_laserGunConfig.Prefab)
                .UnderTransformGroup("Lasers"));

            Container.BindFactory<CatView, CatView.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<CatInstaller>(_catsSettings.CatPrefab)
                .UnderTransformGroup("Cats"));

            Container.BindFactory<Vector2, Quaternion, float, float, Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_bulletGunConfig.Prefab)
                .UnderTransformGroup("Bullets"));

            Container.BindFactory<string, Color, BulletLabelVFX, BulletLabelVFX.Factory>().FromMonoPoolableMemoryPool(x => x
               .WithInitialSize(5)
               .FromComponentInNewPrefab(_labelVFXPrefab)
               .UnderTransformGroup("VFX"));
        }
    }
}