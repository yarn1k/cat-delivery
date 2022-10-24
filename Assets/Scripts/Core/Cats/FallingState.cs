using Core.Infrastructure.Signals.Game;
using Core.Models;
using UnityEngine;
using Zenject;

namespace Core.Cats
{
    public class FallingState : MonoBehaviour, ICatState
    {
        private SignalBus _signalBus;
        private GameSettings _settings;

        public void Init(SignalBus signalBus, GameSettings settings)
        {
            _signalBus = signalBus;
            _settings = settings;
        }

        public void Move()
        {
            transform.Translate(_settings.CatsFallingSpeed * Vector3.down * Time.fixedDeltaTime, Space.World);
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnBecameInvisible()
        {
            _signalBus.Fire(new GameScoreChangedSignal { Value = 10 });
            Destroy(gameObject);
        }
    }
}
