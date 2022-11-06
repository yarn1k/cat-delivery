using System;
using UnityEngine;
using Zenject;
using Core.Weapons;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core.Player
{
    public class PlayerController : IInitializable, ILateDisposable, ITickable, IWeaponHolder
    {
        private readonly SignalBus _signalBus;
        private readonly PlayerModel _model;
        private readonly PlayerView _view;

        private const float FIELD_OF_VIEW = 55f;
        private const string MOVING_KEY = "IsMoving";

        public PlayerController(SignalBus signalBus, PlayerModel playerModel, PlayerView playerView)
        {
            _signalBus = signalBus;
            _model = playerModel;
            _view = playerView;
        }

        void IInitializable.Initialize()
        {
            _model.InputSystem.Jump += OnJump;
        }
        void ILateDisposable.LateDispose()
        {
            UnbindWeapon();
            _model.InputSystem.Jump -= OnJump;
        }
        void ITickable.Tick()
        {
            ProcessMovementInput();

            TrackCursor();
        }

        private void OnJump()
        {
            if (_view.IsGrounded)
            {
                _view.Jump(_model.JumpForce);
            }
        }
        private void OnWeaponHit(CatView target)
        {
            target.Save();
            _signalBus.Fire(new CatSavedSignal { SavedCat = target });
        }

        private void ProcessMovementInput()
        {
            bool isMoving = _model.InputSystem.HorizontalAxis != 0f;
            
            _view.Animator.SetBool(MOVING_KEY, isMoving);

            if (isMoving)
            {
                _view.FlipSprite(_model.InputSystem.HorizontalAxis > 0f);
            }
            bool isCursorRightThanPlayer = _model.InputSystem.MouseWorldPosition.x > _view.transform.position.x;
            _view.FlipSprite(isCursorRightThanPlayer);

            Vector2 direction = Vector2.right * _model.InputSystem.HorizontalAxis * _model.MovementSpeed * Time.deltaTime;
            Translate(direction);
        }
        private void TrackCursor()
        {
            Vector3 mouseWorldPosition = _model.InputSystem.MouseWorldPosition;
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
                _model.InputSystem.Fire -= _model.PrimaryWeapon.Shoot;
            }
        }

        public void Enable()
        {
            _model.InputSystem.Enable();
            _view.gameObject.SetActive(true);
        }
        public void Disable()
        {
            _model.InputSystem.Disable();
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
            _model.InputSystem.Fire += weapon.Shoot;
        } 
    }
}
