using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using Microsoft.Xna.Framework;



namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class PlayerCollisionHandler
{
    private Player player;
    public Rectangle CollisionBox
    {
        get
        {
            return player.CollisionBox;
        }
    }

    public PlayerCollisionHandler(Player player)
    {
        this.player = player;
    }

    public void OnCollidePlayer(Player player, Collision.CollideDirection direction)
    {
        //Nothing
    }
    public void OnCollideItem(IItems item, Collision.CollideDirection direction)
    {
        switch (item)
        {
            case Cloud cloud:
                UnCollide(Rectangle.Intersect(CollisionBox, cloud.CollisionBox), direction);
                if (direction == CollideDirection.Down)
                {
                    player.SetPositon(player.Position + new Vector2(cloud.getX(), 0));
                    player.SetVelocity(new Vector2(player.Velocity.X, 0));
                    UnjumpOnGroundCollide();
                }
                break;
            case Fireflower:
            case Fireflower_Underground:
                player.PowerUp(Power.FireFlower);
                break;
            case MovingPlatform_Size1 plat:
                UnCollide(Rectangle.Intersect(CollisionBox, plat.CollisionBox), direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.SetPositon(player.Position + new Vector2(0, plat.getY()));
                UnjumpOnGroundCollide();
                break;
            case MovingPlatform_Size2 plat:
                UnCollide(Rectangle.Intersect(CollisionBox, plat.CollisionBox), direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.SetPositon(player.Position + new Vector2(0, plat.getY()));
                UnjumpOnGroundCollide();
                break;
            case MovingPlatform_Size3 plat:
                UnCollide(Rectangle.Intersect(CollisionBox, plat.CollisionBox), direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.SetPositon(player.Position + new Vector2(0, plat.getY()));
                UnjumpOnGroundCollide();
                break;
            case Mushroom:
                player.PowerUp(Power.Mushroom);
                break;
            case OneUp:
                player.Lives++;
                break;
            case Spring_Stretched spring:
                UnCollide(Rectangle.Intersect(CollisionBox, spring.CollisionBox), direction);
                player.SetVelocity(player.Velocity - new Vector2(0, 15));
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
        if (player.Invincible) { return; }
        switch (enemy)
        {
            case Fireball:
                player.TakeDamage();
                break;
            case Bowser bowser:
                UnCollide(Rectangle.Intersect(CollisionBox, bowser.CollisionBox), direction);
                player.TakeDamage();
                break;
            case PiranhaPlant:
                player.TakeDamage();
                break;
            case Goomba goomba:
                if (direction == CollideDirection.Down)
                {
                    UnCollide(Rectangle.Intersect(CollisionBox, goomba.CollisionBox), direction);
                    player.SetVelocity(player.Velocity - new Vector2(0, 10));
                }
                else
                {
                    player.TakeDamage();
                }
                break;
            case Koopa koopa:
                if (direction == CollideDirection.Down)
                {
                    UnCollide(Rectangle.Intersect(CollisionBox, koopa.CollisionBox), direction);
                    player.SetVelocity(player.Velocity - new Vector2(0, 10));
                }
                else
                {
                    player.TakeDamage();
                }
                break;
            case RotatingFireBar firebar:
                if (firebar.IsCollidingWithFireballs(CollisionBox))
                {
                    player.TakeDamage();
                }
                break;
            default:
                //Nothing
                break;

        }
    }
    public void OnCollideBlock(IBlock block, Collision.CollideDirection direction)
    {
        UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
        //If ground collide call UnjumpOnGroundCollide
        if(direction == CollideDirection.Down)
        {
            UnjumpOnGroundCollide();
        }
        
    }
    private void UnjumpOnGroundCollide()
    {
        if (!player.IsJumping) { return; }
        player.IsJumping = false;
        player.IsGrounded = true;
        switch (player.GetCurrentPower())
        {
            case Power.None:
                player.ChangeState(new SmallMarioIdleState(player, player.Texture, player.TimeFrame, player.ScaleFactor));
                break;
            case Power.Mushroom:
                player.ChangeState(new BigMarioIdleState(player, player.Texture, player.TimeFrame, player.ScaleFactor));
                break;
            case Power.FireFlower: 
                player.ChangeState(new FireMarioIdleState(player, player.Texture, player.TimeFrame, player.ScaleFactor));
                break;
            default:
                break;
        }
    }
    public void UnCollide(Rectangle intersect, Collision.CollideDirection direction)
    {
        switch (direction)
        {
            case Collision.CollideDirection.Top:
                player.SetPositon(player.Position + new Vector2(0, intersect.Height));
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.IsJumping = false;
                break;
            case Collision.CollideDirection.Down:
                player.SetPositon(player.Position - new Vector2(0, intersect.Height));
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                break;
            case Collision.CollideDirection.Left:
                player.SetPositon(player.Position + new Vector2(intersect.X, 0));
                player.SetVelocity(new Vector2(0, player.Velocity.Y));
                break;
            case Collision.CollideDirection.Right:
                player.SetPositon(player.Position - new Vector2(intersect.X, 0));
                player.SetVelocity(new Vector2(0, player.Velocity.Y));
                break;
        }
    }
}