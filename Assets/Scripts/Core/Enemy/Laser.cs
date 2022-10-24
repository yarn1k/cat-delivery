using UnityEngine;
using Zenject;

namespace Core.Enemy
{
    public class Laser : MonoBehaviour
    {
        private void Start()
        {
            Destroy(gameObject, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Cat"))
            {
                Destroy(other.gameObject);
            }
        }

        public class Factory : PlaceholderFactory<Laser> { }
    }
}
