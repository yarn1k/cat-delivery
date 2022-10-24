using Core.Models;
using UnityEngine;

namespace Core.Cats
{
    public class KidnapState : MonoBehaviour, ICatState
    {
        private GameSettings _settings;

        public void Init(GameSettings settings)
        {
            _settings = settings;
        }

        public void Move()
        {
            transform.Translate(_settings.CatsFallingSpeed * 3f * Vector3.right * Time.fixedDeltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            Move();
        }
    }
}
