using System.Runtime.InteropServices.Swift;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

namespace MagicBrosMario.Source.Block;

public class BrickBlock(ISprite sprite) : BlockBase<BrickBlock>(sprite)
{
    public override void OnCollidePlayer(Player mario, CollideDirection direction)
    {
        if (direction != CollideDirection.Down)
            return;

        // if mario is in big state
        switch (mario.GetCurrentPower())
        {
            case Power.FireFlower:
            case Power.Mushroom:
                break;
            case Power.None:
            case Power.Star:
            default: return;
        }

        sprite.Drop();
        CollisionController.Instance.RemoveBlock(this);
        HUD.Instance.SendEvent(new GameEvent
        {
            EventType = GameEventType.BlockBroken,
            EventPosition = Position
        });
    }

    public override void OnCollideItem(IItems item, CollideDirection direction) { }
    public override void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
    public override void OnCollideBlock(IBlock block, CollideDirection direction) { }
}