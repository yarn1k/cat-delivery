using Core.Cats;
using Core.Infrastructure.Signals.Cat;
using UnityEngine;
using Zenject;

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
            Destroy(gameObject, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Cat"))
            {
                if (!other.gameObject.GetComponent<Cat>().IsInvisible)
                    _signalBus.Fire(new CatKidnappedSignal { Cat = other.gameObject.GetComponent<Cat>() });
            }
        }

        public class Factory : PlaceholderFactory<Laser> { }
    }
}
