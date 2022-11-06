using Core.Input;
using Core.Weapons;

namespace Core.Player
{
    public class PlayerModel
    {
        public readonly IInputSystem InputSystem;
        public readonly float ReloadTime;
        public readonly float MovementSpeed;
        public readonly float JumpForce;
        public IWeapon PrimaryWeapon { get; set; }

        public PlayerModel(IInputSystem inputSystem, float reloadTime, float movementSpeed, float jumpForce)
        {
            InputSystem = inputSystem;
            ReloadTime = reloadTime;
            MovementSpeed = movementSpeed;
            JumpForce = jumpForce;
        }
    }
}
