using System;
using UnityEngine;
using Zenject;
using Core.Weapons;
using Core.Cats;
using Core.Infrastructure.Signals.Game;
using Core.Input;

namespace Core.Player
{
    public class PlayerController : IInitializable, ILateDisposable, ITickable, IWeaponHolder, IPauseHandler
    {
        private readonly SignalBus _signalBus;
        private readonly IInputSystem _inputSystem;
        private readonly PlayerModel _model;
        private readonly PlayerView _view;

        private const float FIELD_OF_VIEW = 55f;

        public PlayerController(SignalBus signalBus, IInputSystem inputSystem, PlayerModel playerModel, PlayerView playerView)
        {
            _signalBus = signalBus;
            _inputSystem = inputSystem;
            _model = playerModel;
            _view = playerView;
        }

        void IInitializable.Initialize()
        {
            _inputSystem.Jump += OnJump;
            _signalBus.Subscribe<GameOverSignal>(OnGameOverSignal);
        }
        void ILateDisposable.LateDispose()
        {
            UnbindWeapon();
            _inputSystem.Jump -= OnJump;
            _signalBus.TryUnsubscribe<GameOverSignal>(OnGameOverSignal);
        }
        void ITickable.Tick()
        {
            ProcessMovementInput();

            TrackCursor();
        }
        void IPauseHandler.SetPaused(bool isPaused)
        {
            if (isPaused)
                _inputSystem.Disable();
            else
                _inputSystem.Enable();
        }

        private void OnJump()
        {
            if (_view.IsGrounded)
            {
                _view.Jump(_model.JumpForce);
            }
        }
        private void OnFire()
        {
            if (_model.PrimaryWeapon.TryShoot())
            {
                _view.ReloadGun(_model.PrimaryWeapon.ReloadTime);
            }
        }
        private void OnWeaponHit(CatView target)
        {
            target.Save();          
        }
        private void OnWeaponMissed()
        {
            _signalBus.Fire<PlayerWeaponMissedSignal>();
        }
        private void OnGameOverSignal()
        {
            _inputSystem.Disable();
        }

        private void ProcessMovementInput()
        {
            _view.SetDirection(_inputSystem.HorizontalAxis);

            bool isCursorRightThanPlayer = _inputSystem.MouseWorldPosition.x > _view.transform.position.x;
            _view.FlipSprite(isCursorRightThanPlayer);

            Vector2 direction = Vector2.right * _inputSystem.HorizontalAxis * _model.MovementSpeed * Time.deltaTime;
            Translate(direction);
        }
        private void TrackCursor()
        {
            Vector3 mouseWorldPosition = _inputSystem.MouseWorldPosition;
            Vector3 lookDirection = mouseWorldPosition - _view.transform.position;
            float degrees = _view.FlipX ? 0f : -180f;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg + degrees;
            float clampedAngle = _view.FlipX ?
                    Mathf.Clamp(angle, 0f, FIELD_OF_VIEW) :
                    Mathf.Clamp(angle, -FIELD_OF_VIEW, 0f);
            _view.RotateGun(Quaternion.Euler(0f, 0f, clampedAngle));
        }
        private void UnbindWeapon()
        {
            if (_model.PrimaryWeapon != null)
            {
                _model.PrimaryWeapon.Hit -= OnWeaponHit;
                _model.PrimaryWeapon.Missed -= OnWeaponMissed;
                _inputSystem.Fire -= OnFire;
            }
        }

        public void Enable()
        {
            _inputSystem.Enable();
            _view.gameObject.SetActive(true);
        }
        public void Disable()
        {
            _inputSystem.Disable();
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
            _model.PrimaryWeapon.Missed += OnWeaponMissed;
            _inputSystem.Fire += OnFire;
        }
    }
}
