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
        private GameSettings _settings;
        private IMemoryPool _pool;
        private ICatState _currentState;
        public ICatState State => _currentState;
        public bool IsInvisible => _currentState is SaveState;

        private T ChangeState<T>() where T : MonoBehaviour, ICatState
        {
            if (GetComponent<T>() is T result) return result;

            foreach (MonoBehaviour state in GetComponents<ICatState>())
            {
                Destroy(state);
            }
            T newState = gameObject.AddComponent<T>();

            return newState;
        }

        [Inject]
        private void Construct(SignalBus signalBus, GameSettings settings)
        {
            _signalBus = signalBus;
            _settings = settings;
        }

        public void SetFallingState()
        {
            var fallingState = ChangeState<FallingState>();
            fallingState.Init(_signalBus, _settings);
            _currentState = fallingState;
        }
        public void SetSaveState()
        {
            var saveState = ChangeState<SaveState>();
            saveState.Init(_signalBus, _settings);
            _currentState = saveState;
        }
        public void SetKidnapState()
        {
            var kidnapState = ChangeState<KidnapState>();
            kidnapState.Init(_settings);
            _currentState = kidnapState;
        }
        public void Dispose()
        {
            _pool.Despawn(this);
        }

        void IPoolable<IMemoryPool>.OnDespawned()
        {
            _pool = null;
        }
        void IPoolable<IMemoryPool>.OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            _currentState = ChangeState<NeutralState>();
            _signalBus.Fire(new CatFallingSignal { FallingCat = this });
        }

        public class Factory : PlaceholderFactory<Cat> { }
    }
}