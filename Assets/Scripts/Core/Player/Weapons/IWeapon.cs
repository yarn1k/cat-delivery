using System;
using Core.Cats;

namespace Core.Weapons
{
    public interface IWeapon
    {
        float Cooldown { get; }
        event Action<CatView> Hit;
        bool TryShoot();
    }
}
