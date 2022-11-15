using System;
using UnityEngine;
using Zenject;
using Core.Cats;

namespace Core.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IPoolable<Vector2, Quaternion, float, float, IMemoryPool>, IDisposable
    {
        private Collider2D _collider;
        private IMemoryPool _pool;
        private float _bulletForce;

        public Vector2 Center => _collider.bounds.center;
        public event Action LifetimeElapsed;
        public event Action<Bullet, CatView> Hit;
        public event Action<Bullet> Disposed;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void Update()
        {
            transform.position += transform.right * _bulletForce * Time.deltaTime;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out CatView target) && target.Interactable)
            {
                Hit?.Invoke(this, target);
            }
        }
        private void OnLifetimeElapsed()
        {
            LifetimeElapsed?.Invoke();
        }
        private void Enable(bool isEnabled)
        {
            _collider.enabled = isEnabled;
            _collider.attachedRigidbody.simulated = isEnabled;
            _collider.attachedRigidbody.constraints = isEnabled ? RigidbodyConstraints2D.FreezeRotation : RigidbodyConstraints2D.FreezeAll;
        }
        public void Dispose()
        {
            _pool?.Despawn(this);
            Disposed?.Invoke(this);
        }

        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnDespawned()
        {
            _pool = null;
            Enable(false);
        }
        void IPoolable<Vector2, Quaternion, float, float, IMemoryPool>.OnSpawned(Vector2 position, Quaternion rotation, float force, float lifetime, IMemoryPool pool)
        {
            _pool = pool;
            transform.position = position;
            transform.rotation = rotation;
            _bulletForce = force;
            Enable(true);
            Invoke(nameof(OnLifetimeElapsed), lifetime);
        }

        public class Factory : PlaceholderFactory<Vector2, Quaternion, float, float, Bullet> { }
        public class ExplosionFactory : PlaceholderFactory<GameObject, Vector2, GameObject> 
        {
            private readonly DiContainer _container;
            public ExplosionFactory(DiContainer container)
            {
                _container = container;
            }

            public override GameObject Create(GameObject prefab, Vector2 position)
            {
                return _container.InstantiatePrefab(prefab, position, Quaternion.identity, null);
            }
        }
    }
}