using Core.Cats;
using Core.Infrastructure.Signals.Player;
using Zenject;

public class PlayerController : IInitializable, ILateDisposable
{
    private SignalBus _signalBus;

    [Inject]
    private Bullet.Factory _bulletFactory;

    public PlayerController(SignalBus signalBus)
    {
        _signalBus = signalBus;
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
        var bullet = _bulletFactory.Create();
        bullet.Init(signal.FirePoint);
    }
}
