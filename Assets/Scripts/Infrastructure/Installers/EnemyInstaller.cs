using UnityEngine;
using Zenject;
using Core.Weapons;
using Core.Models;
using Core.Enemy;

namespace Core.Infrastructure.Installers
{
    public class EnemyInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _enemyPrefab;
        [SerializeField]
        private Transform _spawnPoint;

        [Inject]
        private EnemySettings _enemySettings;
        [Inject]
        private WeaponsSettings _weaponSettings;

        public override void InstallBindings()
        {
            EnemyView view = Container.InstantiatePrefabForComponent<EnemyView>(_enemyPrefab, _spawnPoint.position, Quaternion.identity, null);
            EnemyModel model = new EnemyModel(_enemySettings.MovementSpeed, _enemySettings.AttackCooldownInterval);
            EnemyController controller = Container.Instantiate<EnemyController>(new object[] { model, view });

            LaserGunModel laserGunModel = new LaserGunModel(model.AttackCooldownInterval.x, _weaponSettings.LaserGunConfig, view.FirePoint);
            LaserGun laserGun = Container.Instantiate<LaserGun>(new object[] { laserGunModel });

            //controller.SetPrimaryWeapon(laserGun);

            Container.BindInterfacesTo<EnemyController>().FromInstance(controller).AsSingle();
        }
    }
}