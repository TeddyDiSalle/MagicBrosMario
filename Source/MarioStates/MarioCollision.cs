using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class MarioCollision(Player player) : Collision.ICollidable
{
    public Rectangle CollisionBox { get; set; }

    public void OnCollidePlayer(Player player, Collision.CollideDirection direction)
    {
        //Nothing
    }
    public void OnCollideItem(IItems item, Collision.CollideDirection direction)
    {
        switch (item)
        {
            case Cloud:
                //Uncollide
                //Add dxdy to player pos
                break;
            case Fireflower:
            case Fireflower_Underground:
                player.PowerUp(Power.FireFlower);
                break;
            case MovingPlatform_Size1 plat:
                //Uncollide
                player.AddToPositon(new Vector2(0, plat.getY()));
                break;
            case MovingPlatform_Size2 plat:
                //Uncollide
                player.AddToPositon(new Vector2(0, plat.getY()));
                break;
            case MovingPlatform_Size3 plat:
                //Uncollide
                player.AddToPositon(new Vector2(0, plat.getY()));
                break;
            case Mushroom:
                player.PowerUp(Power.Mushroom);
                break;
            case OneUp:
                player.AddLife();
                break;
            case Spring_Stretched:
                //Uncollide
                player.AddToVelocity(new Vector2(0, 70));
                break;
            case Star:
                player.PowerUp(Power.Star);
                break;
            default:
                //Nothing
                break;
        }
    }
    public void OnCollideEnemy(IEnemy enemy, Collision.CollideDirection direction)
    {
        switch (enemy)
        {
            case Fireball:
            case Bowser:
                //Uncollide
            case PiranhaPlant:
                player.TakeDamage();
                break;
            case Goomba:
                //Uncollide
            case Koopa:
                //Uncollide
                if (direction != Collision.CollideDirection.Top)
                {
                    player.TakeDamage();
                }
                break;
            case RotatingFireBar:
                player.TakeDamage();
                break;
            default:
                //Nothing
                break;

        }
    }
    public void OnCollideBlock(IBlock block, Collision.CollideDirection direction)
    {
        if (!block.IsSolid) return;
        //Uncollide
    }

    public void UnCollide(Rectangle intersect, Collision.CollideDirection direction)
    {
        switch (direction)
        {
            case Collision.CollideDirection.Top:
                player.AddToPositon(new Vector2(0, intersect.Height));
                player.AddToVelocity(new Vector2(0, -player.Velocity.Y));
                break;
            case Collision.CollideDirection.Down:
                player.AddToPositon(new Vector2(0, -intersect.Height));
                player.AddToVelocity(new Vector2(0, -player.Velocity.Y));
                break;
            case Collision.CollideDirection.Left:
                player.AddToPositon(new Vector2(intersect.X, 0));
                player.AddToVelocity(new Vector2(-player.Velocity.X, 0));
                break;
            case Collision.CollideDirection.Right:
                player.AddToPositon(new Vector2(-intersect.X, 0));
                player.AddToVelocity(new Vector2(-player.Velocity.X, 0));
                break;
        }
    }
}
