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
        private IMemoryPool _pool;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.TryGetComponent(out CatView cat))
            {
                Dispose();
                if (cat.Interactable) _signalBus.Fire(new CatSavedSignal { SavedCat = cat });
            }
        }
        public void Dispose()
        {
            _pool?.Despawn(this);
        }
        public void Blast(Vector2 direction, float force)
        {
            _collider.attachedRigidbody.velocity = direction * force;
        }

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
            _collider.enabled = false;
            _collider.attachedRigidbody.simulated = false;
            _collider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _collider.enabled = true;
            _collider.attachedRigidbody.simulated = true;
            _collider.attachedRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        public class Factory : PlaceholderFactory<Bullet> { }
    }
}