using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core.Enemy
{
    public class Laser : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            Destroy(gameObject, 2f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Cat cat) && !cat.IsInvisible)
            {
                _signalBus.Fire(new CatKidnappedSignal { KidnappedCat = cat });
            }
        }

        public class Factory : PlaceholderFactory<Laser> { }
    }
}
