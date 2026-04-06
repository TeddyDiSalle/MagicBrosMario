using Microsoft.Xna.Framework;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class KoopaCollisionHandler
{
    private Koopa koopa;

    public KoopaCollisionHandler(Koopa koopa)
    {
        this.koopa = koopa;
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        if (enemy is Bowser || (enemy is Koopa other && other.IsShellMoving()))
        {
            koopa.FullKill();
            return;
        }
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (koopa.IsWalking())
                koopa.UnCollide(Rectangle.Intersect(koopa.CollisionBox, enemy.CollisionBox), direction);
        }
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if (direction == CollideDirection.Down)
        {
            Rectangle intersect = Rectangle.Intersect(koopa.CollisionBox, block.CollisionBox);
            koopa.Position = new Point(koopa.Position.X, koopa.Position.Y - intersect.Height);
            koopa.ResetGravity();
        }
        else if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (block.CollisionBox.Y < koopa.Position.Y + koopa.CollisionBox.Height - 4)
            {
                if (koopa.IsWalking() || koopa.IsShellMoving())
                    koopa.UnCollide(Rectangle.Intersect(koopa.CollisionBox, block.CollisionBox), direction);
            }
        }
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (player.GetCurrentPower().Equals(Power.Star))
        {
            koopa.FullKill();
            return;
        }
        if (!koopa.GetIsAlive()) return;

        if (direction == CollideDirection.Top)
        {
            if (koopa.IsWalking()) koopa.Kill();
            else if (koopa.IsShellIdle() || koopa.IsStomped()) koopa.KickShell(player.Position.X < koopa.Position.X);
            else if (koopa.IsShellMoving()) koopa.StopShell();
        }
        else
        {
            if (koopa.IsShellIdle() || koopa.IsStomped()) koopa.KickShell(direction == CollideDirection.Left);
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item is MarioFireball)
            koopa.FullKill();
    }
}