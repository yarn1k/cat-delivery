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
            _signalBus.Subscribe<CatFallingSignal>(OnCatFallingSignal);
            _signalBus.Fire(new CatFallingSignal { });
        }

        private void OnDestroy()
        {
            _signalBus.TryUnsubscribe<CatFallingSignal>(OnCatFallingSignal);
        }

        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }

        private void OnCatFallingSignal(CatFallingSignal signal)
        {
            var fallingState = ChangeState<FallingState>();
            fallingState.Init(_settings);
            _currentState = fallingState;
        }

        public class Factory : PlaceholderFactory<Cat> { }
    }
}