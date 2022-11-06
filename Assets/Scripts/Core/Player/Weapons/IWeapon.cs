using System;
using Core.Cats;

namespace Core.Weapons
{
    public interface IWeapon
    {
        event Action<CatView> Hit;
        void Shoot();
    }
}
