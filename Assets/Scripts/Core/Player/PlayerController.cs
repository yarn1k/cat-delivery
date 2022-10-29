using Core;
using Core.Infrastructure.Signals.Player;
using Zenject;

namespace Core.Player
{
    public class PlayerController : IInitializable, ILateDisposable
    {
        private SignalBus _signalBus;
        private Bullet.Factory _bulletFactory;

        public PlayerController(SignalBus signalBus, Bullet.Factory bulletFactory)
        {
            _signalBus = signalBus;
            _bulletFactory = bulletFactory;
        }

        public void Initialize()
        {
            //_signalBus.Subscribe<PlayerFiredSignal>(OnPlayerFiredSignal);
        }

        public void LateDispose()
        {
            //_signalBus.TryUnsubscribe<PlayerFiredSignal>(OnPlayerFiredSignal);
        }

      
    }
}