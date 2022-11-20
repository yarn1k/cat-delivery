using System;
using UnityEngine;

namespace Core.Input
{
    public interface IInputSystem
    {
        public ref float HorizontalAxis { get; }
        public ref Vector2 MouseScreenPosition { get; }
        public ref Vector2 MouseWorldPosition { get; }
        public bool Enabled { get; }
        public event Action Fire;
        public event Action Jump;
        public event Action Pause;
        public void Enable();
        public void Disable();
    }
}