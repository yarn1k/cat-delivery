using System;
using UnityEngine;
using UnityEngine.U2D.Animation;
using Zenject;
using Core.Cats.States;
using Core.Models;

namespace Core.Cats
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class CatView : MonoBehaviour, IPoolable<IMemoryPool>, IDisposable
    {
        [SerializeField]
        private SpriteRenderer _shield;
        [SerializeField]
        private SpriteLibrary _library;

        private IMemoryPool _pool;
        private IStateMachine<CatView> _stateMachine;
        private SpriteLibraryAsset[] _skins;
        public bool Interactable => _stateMachine.CurrentState is FallingState;

        [Inject]
        private void Construct(IStateMachine<CatView> stateMachine, CatsSettings settings)
        {
            _stateMachine = stateMachine;
            _skins = settings.Skins;
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
            int rand = UnityEngine.Random.Range(0, _skins.Length);
            _library.spriteLibraryAsset = _skins[rand];
            _stateMachine.SwitchState<FallingState>();
        }

        public class Factory : PlaceholderFactory<CatView> { }
    }
}