using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Infrastructure.Signals.Game;
using Core.Infrastructure.Signals.Cats;
using Core.UI;
using Core.Weapons;
using Core.Models;

namespace Core.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private LevelBounds _levelBounds;
        [SerializeField]
        private GameObject _labelVFXPrefab;   
        [SerializeField]
        private GameObject _catPrefab;

        [Inject]
        private WeaponsSettings _weaponsSettings;

        public override void InstallBindings()
        {
            Container.DeclareSignal<GameScoreChangedSignal>();
            Container.DeclareSignal<GameOverSignal>();              

            Container.Bind<LevelBounds>().FromInstance(_levelBounds).AsSingle();
            Container.Bind<IInitializable>().To<GameController>().AsSingle();
            Container.Bind<AsyncProcessor>().FromNewComponentOnNewGameObject().AsSingle();

            BindFactories();
            BindPools();
            BindCats();
            BindUI();
        }

        private void BindCats()
        {
            Container.DeclareSignal<CatFellSignal>();
            Container.DeclareSignal<CatSavedSignal>();
            Container.DeclareSignal<CatKidnappedSignal>();
            Container.BindInterfacesTo<CatSpawner>().AsSingle();
        }
      
        private void BindUI()
        {
            Container.Bind<Score>().AsSingle();
        }
     
        private void BindFactories()
        {
            Container.BindFactory<GameObject, Vector2, GameObject, Bullet.ExplosionFactory>();
        }
        private void BindPools()
        {
            Container.BindFactory<Vector2, Quaternion, float, float, Laser, Laser.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_weaponsSettings.LaserGunConfig.Prefab)
                .UnderTransformGroup("Lasers"));

            Container.BindFactory<CatView, CatView.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<CatInstaller>(_catPrefab)
                .UnderTransformGroup("Cats"));

            Container.BindFactory<Vector2, Quaternion, float, float, Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_weaponsSettings.BulletGunConfig.Prefab)
                .UnderTransformGroup("Bullets"));

            Container.BindFactory<string, Color, BulletLabelVFX, BulletLabelVFX.Factory>().FromMonoPoolableMemoryPool(x => x
               .WithInitialSize(5)
               .FromComponentInNewPrefab(_labelVFXPrefab)
               .UnderTransformGroup("UI VFX"));
        }
    }
}