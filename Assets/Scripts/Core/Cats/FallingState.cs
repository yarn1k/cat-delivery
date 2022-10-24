using Core.Models;
using UnityEngine;

namespace Core.Cats
{
    public class FallingState : MonoBehaviour, ICatState
    {
        private GameSettings _settings;

        public void Init(GameSettings settings)
        {
            _settings = settings;
        }

        public void Move()
        {
            transform.Translate(_settings.CatsFallingSpeed * Vector3.down * Time.fixedDeltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}
