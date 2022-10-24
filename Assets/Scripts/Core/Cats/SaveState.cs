using Core.Models;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Core.Cats
{
    public class SaveState : MonoBehaviour, ICatState
    {
        private GameSettings _settings;

        public void Init(GameSettings settings)
        {
            _settings = settings;
        }

        public void Move()
        {
            transform.Translate(_settings.CatsFallingSpeed * 2f * Vector3.down * Time.fixedDeltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}