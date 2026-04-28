using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class BowserCollisionHandler
{
    private Bowser bowser;

    public BowserCollisionHandler(Bowser bowser)
    {
        this.bowser = bowser;
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (player.GetCurrentPower().Equals(Enums.Star))
        {
            bowser.Kill();
            HUD.Instance.SendEvent(new GameEvent
            {
                EventType = GameEventType.EnemyStomped,
                EventPosition = bowser.Position,
                Data = bowser
            });
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item is MarioFireball)
            bowser.TakeDamage();
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if (direction == CollideDirection.Down)
        {
            Rectangle intersect = Rectangle.Intersect(bowser.CollisionBox, block.CollisionBox);
            bowser.Position = new Point(bowser.Position.X, bowser.Position.Y - intersect.Height);
            bowser.ResetGravity();
        }
        else if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (block.CollisionBox.Y < bowser.Position.Y + bowser.CollisionBox.Height - 4)
                bowser.UnCollide(Rectangle.Intersect(bowser.CollisionBox, block.CollisionBox), direction);
        }
    }
}