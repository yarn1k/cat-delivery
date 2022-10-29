using System;
using UnityEngine;
using Zenject;
using Core.Infrastructure.Signals.Cats;
using Core.Models;

namespace Core.Cats
{
    public class Cat : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        private SignalBus _signalBus;
        private IMemoryPool _pool;
        private bool _disposed;

        public bool IsInvisible => _disposed;

        [Inject]
        private void Construct(SignalBus signalBus, GameSettings settings)
        {
            _signalBus = signalBus;
        }
        private void OnBecameInvisible()
        {
            Dispose();
        }
        public void Dispose()
        {
            _disposed = true;
            _pool?.Despawn(this);
        }

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _disposed = false;
            _signalBus.Fire(new CatFallingSignal { FallingCat = this });
        }

        public class Factory : PlaceholderFactory<Cat> { }
    }
}