namespace Core.Infrastructure.Signals.Game
{
    public struct EnemyWantsAttackSignal { }
    public struct GameScoreChangedSignal
    {
        public int Value;
    }
    public struct GameOverSignal { }
}
