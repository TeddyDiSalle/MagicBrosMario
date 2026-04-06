using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.HUDAndScoring;
//Vincent Do
public enum GameEventType{
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
    public Vector2 EventPosition;
    public object Data;
}