using System;
using UnityEngine;
using Zenject;

namespace Core.Input
{
    public class StandaloneInputController : IInitializable, ITickable, ILateDisposable, IInputSystem
    {
        private PlayerControls _playerControls;
        private float _direction;
        private Vector2 _mousePosition;

        public ref float HorizontalAxis => ref _direction;
        public ref Vector2 MousePosition => ref _mousePosition;
        public bool Enabled => _playerControls.Player.enabled;
        public Action Fire { get; set; }

        public StandaloneInputController()
        {
            _playerControls = new PlayerControls();
        }  

        void IInitializable.Initialize()
        {
            _playerControls.Enable();
            _playerControls.Player.Fire.started += _ => Fire?.Invoke();
        }
        void ILateDisposable.LateDispose()
        {
            _playerControls.Disable();
        }
        void ITickable.Tick()
        {
            if (!Enabled) return;

            _direction = _playerControls.Player.Movement.ReadValue<float>();
            _mousePosition = _playerControls.Player.MousePosition.ReadValue<Vector2>();
        }
    }
}