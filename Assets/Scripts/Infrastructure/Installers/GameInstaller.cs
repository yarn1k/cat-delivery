using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;
using Core.UI;
using Core.Weapons;
using Core.Models;
using Core.Infrastructure.Signals.Game;
using Core.Input;
using Core.Player.Buffs;

namespace Core.Infrastructure.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _labelVFXPrefab;   
        [SerializeField]
        private GameObject _catPrefab;

        [Inject]
        private WeaponsSettings _weaponsSettings;

        public override void InstallBindings()
        {
            Container.DeclareSignal<GameOverSignal>();

            BindFactories();
            BindPools();
            BindCats();
            BindPlayer();
        }

        private void BindCats()
        {
            Container.DeclareSignal<CatFellSignal>();
            Container.DeclareSignal<CatSavedSignal>();
            Container.DeclareSignal<CatKidnappedSignal>();
        }
        private void BindPlayer()
        {
            Container.DeclareSignal<PlayerWeaponMissedSignal>();
            Container.BindInterfacesTo<StandaloneInputController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BuffSystem>().AsSingle();
        }
        private void BindFactories()
        {
            Container.BindFactory<GameObject, Vector2, GameObject, Bullet.ExplosionFactory>();
            Container.BindInterfacesAndSelfTo<PlayerFactory>().AsSingle();
            Container.BindInterfacesAndSelfTo<CatSpawner>().AsSingle();
            Container.BindInterfacesAndSelfTo<LaserSpawner>().AsSingle();
        }
        private void BindPools()
        {
            Container.BindFactory<Vector2, Quaternion, float, float, Laser, Laser.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_weaponsSettings.LaserGunConfig.Prefab)
                .UnderTransformGroup("Lasers"));

            Container.BindFactory<CatView, CatView.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_catPrefab)
                .UnderTransformGroup("Cats"));

            Container.BindFactory<Vector2, Quaternion, float, float, Bullet, Bullet.Factory>().FromMonoPoolableMemoryPool(x => x
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_weaponsSettings.BulletGunConfig.Prefab)
                .UnderTransformGroup("Bullets"));

            Container.BindFactory<string, Color, DisposableLabelVFX, DisposableLabelVFX.Factory>().FromMonoPoolableMemoryPool(x => x
               .WithInitialSize(5)
               .FromComponentInNewPrefab(_labelVFXPrefab)
               .UnderTransformGroup("UI VFX"));
        }
    }
}