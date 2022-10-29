using System;
using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class Bullet : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        private SignalBus _signalBus;
        private Collider2D _collider;
        private Rigidbody2D _rigidbody;
        private IMemoryPool _pool;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out CatView cat))
            {
                Dispose();
                _signalBus.Fire(new CatSavedSignal { SavedCat = cat });
            }
        }
        private void OnBecameInvisible() => Dispose();
        public void Dispose()
        {
            _pool?.Despawn(this);
        }
        public void Blast(Vector2 direction, float force)
        {
            _rigidbody.velocity = direction * force;
        }

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
            _collider.enabled = false;
            _rigidbody.simulated = false;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _collider.enabled = true;
            _rigidbody.simulated = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public class Factory : PlaceholderFactory<Bullet> { }
    }
}