namespace Core.Player.Buffs
{
    public class AttackSpeedBonus : IBuff
    {
        private readonly float _attackBonus;
        private float _oldReloadTime;
        public Cooldown Duration { get; private set; }

        public AttackSpeedBonus(float attackBonus)
        {
            _attackBonus = attackBonus;
            Duration = new Cooldown();
        }

        public void Execute(PlayerModel model)
        {
            _oldReloadTime = model.ReloadTime;
            model.MovementSpeed += _attackBonus;
        }

        public void Reset(PlayerModel model)
        {
            model.ReloadTime = _oldReloadTime;
        }
    }
}
