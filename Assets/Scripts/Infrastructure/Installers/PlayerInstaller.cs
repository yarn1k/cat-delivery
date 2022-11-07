using UnityEngine;
using Zenject;
using Core.Input;
using Core.Player;
using Core.Weapons;
using Core.Models;

namespace Core.Infrastructure.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField]
        private GameObject _playerPrefab;

        [Inject]
        private PlayerSettings _playerSettings;
        [Inject]
        private WeaponsSettings _weaponSettings;

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<StandaloneInputController>().AsSingle();
            PlayerView view = Container.InstantiatePrefabForComponent<PlayerView>(_playerPrefab);
            PlayerModel model = Container.Instantiate<PlayerModel>(new object[] { _playerSettings.ReloadTime, _playerSettings.MovementSpeed, _playerSettings.JumpForce });
            PlayerController controller = Container.Instantiate<PlayerController>(new object[] { model, view });

            BulletGunModel bulletGunModel = new BulletGunModel(model.ReloadTime, _weaponSettings.BulletGunConfig, view.FirePoint);
            BulletGun bulletGun = Container.Instantiate<BulletGun>(new object[] { bulletGunModel });

            controller.SetPrimaryWeapon(bulletGun);

            Container.BindInterfacesTo<PlayerController>().FromInstance(controller).AsSingle();
        }
    }
}