using System;
using UnityEngine;
using Zenject;
using Core.Cats.States;

namespace Core.Cats
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CatView : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        private IMemoryPool _pool;
        private bool _disposed;
        private bool _saved;
        private IStateMachine<CatView> _stateMachine;
        public bool IsInvisible => _saved || _disposed;

        [Inject]
        private void Construct(IStateMachine<CatView> stateMachine)
        {
            _stateMachine = stateMachine;
        }
        private void SetFalling()
        {
            _stateMachine.SwitchState<FallingState>();
        }
        public void Kidnap()
        {
            _stateMachine.SwitchState<KidnapState>();
        }
        public void Save()
        {
            _saved = true;
            _stateMachine.SwitchState<SaveState>();
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
            _saved = false;
            _disposed = false;
            SetFalling();
        }

        public class Factory : PlaceholderFactory<CatView> { }
    }
}