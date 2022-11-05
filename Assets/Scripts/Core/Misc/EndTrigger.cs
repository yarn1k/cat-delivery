using UnityEngine;
using Zenject;
using Core.Cats;
using Core.Infrastructure.Signals.Cats;

namespace Core
{
    public class EndTrigger : MonoBehaviour
    {
        private SignalBus _signalBus;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out CatView cat))
            {
                cat.Dispose();
                _signalBus.Fire(new CatFellSignal { FallenCat = cat });
            }
        }
    }
}
