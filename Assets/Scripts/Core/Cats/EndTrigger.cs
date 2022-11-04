using Core.Infrastructure.Signals.Cats;
using UnityEngine;
using Zenject;

namespace Core.Cats
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
            if (collision.TryGetComponent(out CatView cat) && cat.Interactable)
            {
                _signalBus.Fire(new CatFellSignal { FallenCat = cat });
            }
        }
    }
}
