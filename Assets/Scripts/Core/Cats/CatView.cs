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
        [SerializeField]
        private SpriteRenderer _shield;

        private IMemoryPool _pool;
        private IStateMachine<CatView> _stateMachine;
        public bool Interactable => _stateMachine.CurrentState is FallingState;

        [Inject]
        private void Construct(IStateMachine<CatView> stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void Kidnap()
        {
            _stateMachine.SwitchState<KidnapState>();
        }
        public void Save()
        {
            _shield.gameObject.SetActive(true);
            _stateMachine.SwitchState<SaveState>();
        }
        public void Dispose()
        {
            _pool?.Despawn(this);
        }

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _shield.gameObject.SetActive(false);
            _stateMachine.SwitchState<FallingState>();
        }

        public class Factory : PlaceholderFactory<CatView> { }
    }
}