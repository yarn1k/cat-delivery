using System;
using UnityEngine;
using Core.Weapons;
using Core.Cats;

namespace Core.Player
{
    public class PlayerController : IWeaponHolder, IPauseHandler
    {
        private readonly PlayerModel _model;
        private readonly PlayerView _view;

        private const float FIELD_OF_VIEW = 55f;

        public event Action WeaponMissed;
        public event Action<CatView> WeaponHit;

        public PlayerController(PlayerModel playerModel, PlayerView playerView)
        {
            _model = playerModel;
            _view = playerView;
            _model.InputSystem.Jump += OnJump;
        }

        void IPauseHandler.SetPaused(bool isPaused)
        {
            if (isPaused) Disable();
            else Enable();
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
        private void ProcessMovementInput()
        {
            _view.SetDirection(_model.InputSystem.HorizontalAxis);

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
        private void BindWeapon(bool isBind)
        {
            if (_model.PrimaryWeapon == null) return;

            if (isBind)
            {
                _model.PrimaryWeapon.Hit += WeaponHit;
                _model.PrimaryWeapon.Missed += WeaponMissed;
                _model.InputSystem.Fire += OnFire;
            }
            else
            {
                _model.PrimaryWeapon.Hit -= WeaponHit;
                _model.PrimaryWeapon.Missed -= WeaponMissed;
                _model.InputSystem.Fire -= OnFire;
            }
        }

        public void Update()
        {
            ProcessMovementInput();

            TrackCursor();
        }
        public void Enable()
        {
            _model.InputSystem.Enable();
            _model.InputSystem.Jump += OnJump;
            BindWeapon(true);
        }
        public void Disable()
        {
            _model.InputSystem.Disable();
            _model.InputSystem.Jump -= OnJump;
            BindWeapon(false);
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

            BindWeapon(false);

            _model.PrimaryWeapon = weapon;
            _model.PrimaryWeapon.Hit += WeaponHit;
            _model.PrimaryWeapon.Missed += WeaponMissed;
            _model.InputSystem.Fire += OnFire;
        }
    }
}
