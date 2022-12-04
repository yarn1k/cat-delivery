using UnityEngine;
using Zenject;
using Core.Models;
using Core.Player;
using Core.Weapons;
using Core.Infrastructure.Signals.Game;

namespace Core
{
    public class PlayerFactory : ITickable, IFactory<PlayerController>
    {
        private readonly DiContainer _container;
        private readonly SignalBus _signalBus;
        private readonly PlayerSettings _playerSettings;
        private readonly WeaponsSettings _weaponSettings;
        private PlayerController _playerController;

        private const string PATH = "Prefabs/Player/Player";

        public PlayerFactory(DiContainer container, SignalBus signalBus, PlayerSettings playerSettings, WeaponsSettings weaponSettings)
        {
            _container = container;
            _signalBus = signalBus;
            _playerSettings = playerSettings;
            _weaponSettings = weaponSettings;
        }
        public PlayerController Create()
        {
            PlayerView asset = Resources.Load<PlayerView>(PATH);
            PlayerView view = GameObject.Instantiate<PlayerView>(asset);

            PlayerModel model = _container.Instantiate<PlayerModel>(new object[] { _playerSettings.ReloadTime, _playerSettings.MovementSpeed, _playerSettings.JumpForce });
            _container.Bind<PlayerModel>().FromInstance(model).AsSingle();

            _playerController = new PlayerController(model, view);
            _playerController.WeaponMissed += OnPlayerMissed;
            _playerController.WeaponHit += OnWeaponHit;

            BulletGunModel bulletGunModel = new BulletGunModel(model.ReloadTime, _weaponSettings.BulletGunConfig, view.FirePoint);
            BulletGun bulletGun = _container.Instantiate<BulletGun>(new object[] { bulletGunModel });

            _playerController.SetPrimaryWeapon(bulletGun);            
            return _playerController;
        }

        private void OnPlayerMissed()
        {
            _signalBus.Fire<PlayerWeaponMissedSignal>();
        }
        private void OnWeaponHit(Cats.CatView cat)
        {
            cat.Save();
        }

        void ITickable.Tick()
        {
            _playerController?.Update();
        }
    }
}
