namespace Core.Player.Buffs
{
    public interface IBuff
    {
        Cooldown Duration { get; }
        void Execute(PlayerModel model);
        void Reset(PlayerModel model);
    }
}
