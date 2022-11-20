using System;
using Core.Cats;

namespace Core.Weapons
{
    public interface IWeapon
    {
        float ReloadTime { get; }
        event Action Missed;
        event Action<CatView> Hit;
        bool TryShoot();
    }
}
