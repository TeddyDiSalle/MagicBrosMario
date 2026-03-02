using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class MarioCollision(Player player) : ICollidable
{
    Rectangle CollisionBox { get; }
    void OnPlayerCollide(IPlayer player, CollideDirection direction)
    {
        //Nothing
    }
    void OnItemCollide(IItems item, CollideDirection direction)
    {
        switch (item)
        {
            case Fireflower:
            case Fireflower_Underground:
                player.PowerUp(Power.FireFlower);
                break;
            case MovingPlatform_Size1:
            case MovingPlatform_Size2:
            case MovingPlatform_Size3:
                //Add dx and dy of platform to player position
                break;
            case Mushroom:
                player.PowerUp(Power.Mushroom);
                break;
            case Spring_Stretched:
                player.MoveUp();
                break;
            case Star:
                player.PowerUp(Power.Star);
                break;
            default:
                //Nothing
                break;
        }
    }
    void OnEnemyCollide(IEnemy enemy, CollideDirection direction)
    {
        switch (enemy) {
            case Fireball:
            case Bowser:
            case PiranhaPlant:
                player.TakeDamage();
                break;
            case Goomba:
            case Koopa:
                if (direction != CollideDirection.Top)
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
    void OnBlockCollide(IBlock block, CollideDirection direction)
    {

    }
}
