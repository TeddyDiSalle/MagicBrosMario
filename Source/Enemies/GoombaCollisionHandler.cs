using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class GoombaCollisionHandler
{
    private Goomba goomba;

    public GoombaCollisionHandler(Goomba goomba)
    {
        this.goomba = goomba;
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        if (enemy is Bowser || (enemy is Koopa koopa && koopa.IsShellMoving()))
        {
            goomba.Kill();
            HUD.Instance.SendEvent(new GameEvent
            {
                EventType = GameEventType.EnemyStomped,
                EventPosition = goomba.Position,
                Data = goomba
            });
            return;
        }
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
            goomba.UnCollide(Rectangle.Intersect(goomba.CollisionBox, enemy.CollisionBox), direction);
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if (direction == CollideDirection.Down)
        {
            Rectangle intersect = Rectangle.Intersect(goomba.CollisionBox, block.CollisionBox);
            goomba.Position = new Point(goomba.Position.X, goomba.Position.Y - intersect.Height);
            goomba.ResetGravity();
        }
        else if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (block.CollisionBox.Y < goomba.Position.Y + goomba.CollisionBox.Height - 4)
                goomba.UnCollide(Rectangle.Intersect(goomba.CollisionBox, block.CollisionBox), direction);
        }
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (player.GetCurrentPower().Equals(Power.Star) || direction == CollideDirection.Top)
        {
            goomba.Kill();
            HUD.Instance.SendEvent(new GameEvent
            {
                EventType = GameEventType.EnemyStomped,
                EventPosition = goomba.Position,
                Data = goomba
            });
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item is Cloud or MovingPlatform_Size1 or MovingPlatform_Size2 or MovingPlatform_Size3)
        {
            HandlePlatformCollision(item, direction);
            return;
        }
        if (item is MarioFireball)
        {
            goomba.Kill();
            HUD.Instance.SendEvent(new GameEvent
            {
                EventType = GameEventType.EnemyKilledByFireball,
                EventPosition = goomba.Position,
                Data = goomba
            });
        }
    }

    private void HandlePlatformCollision(IItems item, CollideDirection direction)
    {
        if (direction == CollideDirection.Down)
        {
            Rectangle intersect = Rectangle.Intersect(goomba.CollisionBox, item.CollisionBox);
            goomba.Position = new Point(goomba.Position.X, goomba.Position.Y - intersect.Height);
            goomba.ResetGravity();
        }
        else if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (item.CollisionBox.Y < goomba.Position.Y + goomba.CollisionBox.Height - 4)
                goomba.UnCollide(Rectangle.Intersect(goomba.CollisionBox, item.CollisionBox), direction);
        }
    }
}