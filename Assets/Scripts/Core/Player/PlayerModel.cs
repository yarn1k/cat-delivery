using Core.Input;
using Core.Weapons;
using Zenject;

namespace Core.Player
{
    public class PlayerModel
    {
        public readonly float ReloadTime;
        public readonly float MovementSpeed;
        public readonly float JumpForce;
        public IInputSystem InputSystem { get; private set; }
        public IWeapon PrimaryWeapon { get; set; }

        [Inject]
        private void Construct(IInputSystem inputSystem)
        {
            InputSystem = inputSystem;
        }

        public PlayerModel(float reloadTime, float movementSpeed, float jumpForce)
        {
            ReloadTime = reloadTime;
            MovementSpeed = movementSpeed;
            JumpForce = jumpForce;
        }
    }
}
