namespace Core.Infrastructure.Signals.Game
{
    public struct GameSpawnedCatSignal 
    {
        public float SpawnPositionY;
    }
    public struct EnemyWantsAttackSignal { }
    public struct GameSpawnedLaserSignal { }
    public struct GameScoreChangedSignal
    {
        public int Value;
    }
    public struct GameOverSignal { }
}
