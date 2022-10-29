using Core;
using Core.Infrastructure.Signals.Player;
using Zenject;

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
        _signalBus.Subscribe<PlayerFiredSignal>(OnPlayerFiredSignal);
    }

    public void LateDispose()
    {
        _signalBus.TryUnsubscribe<PlayerFiredSignal>(OnPlayerFiredSignal);
    }

    private void OnPlayerFiredSignal(PlayerFiredSignal signal)
    {
        Bullet bullet = _bulletFactory.Create();
        bullet.transform.position = signal.FirePoint.position;
        bullet.transform.rotation = signal.FirePoint.rotation;
    }
}
