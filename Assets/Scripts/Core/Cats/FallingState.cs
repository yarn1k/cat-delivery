using Core.Models;
using System.Reflection.Emit;
using UnityEngine;
using Zenject;

namespace Core.Cats
{
    public class FallingState : MonoBehaviour, ICatState
    {
        private GameSettings _gameSettings;
        private float _fallSpeed = 2f;

        public void Move()
        {
            transform.Translate(_fallSpeed * Vector3.down * Time.fixedDeltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}
