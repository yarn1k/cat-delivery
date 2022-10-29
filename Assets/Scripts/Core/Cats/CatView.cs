using Core.Cats.States;
using System;
using UnityEngine;
using Zenject;

namespace Core.Cats
{
    [RequireComponent(typeof(Collider2D))]
    public class CatView : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool;
        private bool _disposed;
        public bool IsInvisible => _disposed;

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
        }

        public class Factory : PlaceholderFactory<CatView> { }
    }
}