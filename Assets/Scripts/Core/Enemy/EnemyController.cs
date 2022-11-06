using System;
using UnityEngine;
using Zenject;
using Core.Enemy.States;
using Core.Weapons;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core.Enemy
{
    public class EnemyController : IInitializable, ITickable, ILateDisposable, IWeaponHolder
    {
        private readonly EnemyModel _model;
        private readonly EnemyView _view;
        private readonly SignalBus _signalBus;
        private IStateMachine<EnemyController> _stateMachine;

        public EnemyController(SignalBus signalBus, EnemyModel enemyModel, EnemyView enemyView)
        {
            _signalBus = signalBus;
            _model = enemyModel;
            _view = enemyView;
        }

        void IInitializable.Initialize()
        {
            _stateMachine = new EnemyStateMachine(this, _model);
            _stateMachine.SwitchState<UpAndDownState>();
        }
        void ILateDisposable.LateDispose()
        {
            UnbindWeapon();
        }
        void ITickable.Tick()
        {
            _stateMachine.CurrentState.Update();
        }

        private void OnWeaponHit(CatView target)
        {
            target.Kidnap();
            _signalBus.Fire(new CatKidnappedSignal { KidnappedCat = target });
        }

        private void UnbindWeapon()
        {
            if (_model.PrimaryWeapon != null)
            {
                _model.PrimaryWeapon.Hit -= OnWeaponHit;
            }
        }
        public void Enable()
        {
            _view.gameObject.SetActive(true);
        }
        public void Disable()
        {
            _view.gameObject.SetActive(false);
        }
        public void Translate(Vector2 direction)
        {
            _view.transform.Translate(direction, Space.World);
        }
        public void SetPosition(Vector2 position)
        {
            _view.transform.position = position;
        }
        public void SetPrimaryWeapon(IWeapon weapon)
        {
            if (weapon == null) throw new ArgumentNullException("Weapon is null!");

            UnbindWeapon();

            _model.PrimaryWeapon = weapon;
            _model.PrimaryWeapon.Hit += OnWeaponHit;
        }
    }
}
