using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.HUDAndScoring;
//Vincent Do
public enum GameEventType{
    StartLevel,
    EnemyStomped,
    LandedOnGround,
    EnemyKilledByFireball,
    CoinCollected,
    BlockBroken,
    PowerupAppears,
    PowerupCollected,
    FlagpoleReached,
    EndOfLevel
}

public struct GameEvent
{
    public GameEventType EventType;
    public Point EventPosition;
    public object Data;
}