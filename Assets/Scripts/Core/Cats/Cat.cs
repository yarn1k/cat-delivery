using UnityEngine;
using Core.Infrastructure.Signals.Cat;
using Zenject;

namespace Core.Cats
{
    public class Cat : MonoBehaviour
    {
        private SignalBus _signalBus;

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
        private void Construct(SignalBus signalBus) => _signalBus = signalBus;

        private void Awake()
        {
            _currentState = ChangeState<NeutralState>();
        }

        private void Start()
        {
            _signalBus.Subscribe<CatFallingSignal>(OnCatFallingSignal);
        }

        private void Dispose()
        {
            _signalBus.TryUnsubscribe<CatFallingSignal>(OnCatFallingSignal);
        }

        private void OnCatFallingSignal(CatFallingSignal signal)
        {
            _currentState = ChangeState<FallingState>();
        }

        public class Factory : PlaceholderFactory<Cat> { }
    }
}