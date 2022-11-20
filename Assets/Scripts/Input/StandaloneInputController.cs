using System;
using UnityEngine;
using Zenject;

namespace Core.Input
{
    public class StandaloneInputController : IInitializable, ITickable, ILateDisposable, IInputSystem
    {
        private PlayerControls _playerControls;
        private float _horizontalAxis;
        private Camera _camera;
        private Vector2 _mouseScreenPosition;
        private Vector2 _mouseWorldPosition;

        public ref float HorizontalAxis => ref _horizontalAxis;
        public ref Vector2 MouseScreenPosition => ref _mouseScreenPosition;
        public ref Vector2 MouseWorldPosition => ref _mouseWorldPosition;
        public bool Enabled => _playerControls.Player.enabled;
        public event Action Fire;
        public event Action Jump;
        public event Action Pause;

        public StandaloneInputController()
        {
            _playerControls = new PlayerControls();
        }  

        void IInitializable.Initialize()
        {
            _playerControls.Enable();
            _playerControls.Player.Fire.started += _ => Fire?.Invoke();
            _playerControls.Player.Jump.started += _ => Jump?.Invoke();
            _playerControls.Player.Pause.started += _ => Pause?.Invoke();
            _camera = Camera.main;
        }
        void ILateDisposable.LateDispose()
        {
            _playerControls.Disable();
            _playerControls.Dispose();
        }
        void ITickable.Tick()
        {
            if (!Enabled) return;

            _horizontalAxis = _playerControls.Player.Movement.ReadValue<float>();
            _mouseScreenPosition = _playerControls.Player.MousePosition.ReadValue<Vector2>();
            _mouseWorldPosition = _camera.ScreenToWorldPoint(_mouseScreenPosition);
        }

        public void Enable()
        {
            _playerControls.Enable();
        }
        public void Disable()
        {
            _playerControls.Disable();

            _horizontalAxis = 0f;
        }
    }
}