namespace Core.Player.Buffs
{
    public class MovementSpeedBonus : IBuff
    {
        private readonly float _movementBonus;
        private float _oldMovementSpeed;
        public Cooldown Duration { get; private set; }

        public MovementSpeedBonus(float speedBonus)
        {
            _movementBonus = speedBonus;
            Duration = new Cooldown();
        }

        public void Execute(PlayerModel model)
        {
            _oldMovementSpeed = model.MovementSpeed;
            model.MovementSpeed += _movementBonus;
        }

        public void Reset(PlayerModel model)
        {
            model.MovementSpeed = _oldMovementSpeed;
        }
    }
}
