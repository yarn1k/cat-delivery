using System;
using UnityEngine;
using Zenject;
using Core.Cats.States;

namespace Core.Cats
{
    [RequireComponent(typeof(Collider2D))]
    public class CatView : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool;
        private bool _disposed;
        private IStateMachine<CatView> _stateMachine;
        public bool IsInvisible => _disposed;

        [Inject]
        private void Construct(IStateMachine<CatView> stateMachine)
        {
            _stateMachine = stateMachine;
        }
        private void OnBecameInvisible()
        {
            Dispose();
        }
        public void Kidnap()
        {
            _stateMachine.SwitchState<KidnapState>();
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