namespace Core.Infrastructure.Signals.Game
{
    public struct GameScoreChangedSignal
    {
        public int Value;
    }
    public struct GameOverSignal { }
    public struct HealthReducedSignal 
    {
        public byte Value; 
    }
}
