using UnityEngine;
using Core.Infrastructure.Signals.Cat;
using Zenject;
using Core.Models;
using System;

namespace Core.Cats
{
    public class Cat : MonoBehaviour
    {
        private SignalBus _signalBus;
        private GameSettings _settings;

        private ICatState _currentState;
        public ICatState State { get => _currentState; }
        public bool IsInvisible
        {
            get => _currentState is SaveState ? true : false;
        }

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

        private void Awake()
        {
            _currentState = ChangeState<NeutralState>();
        }

        private void Start()
        {
            _signalBus.Fire(new CatFallingSignal { Cat = this });
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        public void SetFallingState()
        {
            var fallingState = ChangeState<FallingState>();
            fallingState.Init(_settings);
            _currentState = fallingState;
        }

        public void SetSaveState()
        {
            var saveState = ChangeState<SaveState>();
            saveState.Init(_settings);
            _currentState = saveState;
        }

        public class Factory : PlaceholderFactory<Cat> { }
    }
}