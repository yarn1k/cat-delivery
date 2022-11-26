using Core.Weapons;

namespace Core.Player
{
    public class PlayerModel
    {
        public readonly float ReloadTime;
        public readonly float MovementSpeed;
        public readonly float JumpForce;
        public IWeapon PrimaryWeapon { get; set; }

        public PlayerModel(float reloadTime, float movementSpeed, float jumpForce)
        {
            ReloadTime = reloadTime;
            MovementSpeed = movementSpeed;
            JumpForce = jumpForce;
        }
    }
}
