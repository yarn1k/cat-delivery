using System;
using UnityEngine;

namespace Core.Input
{
    public interface IInputSystem
    {
        public ref float HorizontalAxis { get; }
        public ref Vector2 MousePosition { get; }
        public bool Enabled { get; }
        public event Action Fire;
        public event Action Jump;
        public void Enable();
        public void Disable();
    }
}