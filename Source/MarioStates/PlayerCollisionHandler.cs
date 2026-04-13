using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sound;
using Microsoft.Xna.Framework;



namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class PlayerCollisionHandler
{
    private readonly Player player;
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
        CollisionController.Instance.BindPlayer(player);
    }

    public void OnCollidePlayer(Player player, Collision.CollideDirection direction)
    {
        //Nothing
    }
    public void OnCollideItem(IItems item, Collision.CollideDirection direction)
    {
        if (!player.IsAlive) { return; }
        switch (item)
        {
            case Cloud cloud:
                UnCollide(cloud.CollisionBox, direction);
                if (direction == CollideDirection.Down)
                {
                    player.SetPositon(player.Position + new Vector2(cloud.getX(), 0));
                    player.SetVelocity(new Vector2(player.Velocity.X, 0));
                    UnjumpOnGroundCollide();
                }
                break;
            case Fireflower:
                player.PowerUp(Power.FireFlower);
                HUD.Instance.SendEvent(new GameEvent
                {
                    EventType = GameEventType.PowerupCollected,
                    Data = item,
                    EventPosition = new Point(item.CollisionBox.X, item.CollisionBox.Y) + new Point(item.CollisionBox.Width / 2, item.CollisionBox.Height / 2)
                });
                break;
            case MovingPlatform_Size1 plat:
                UnCollide(plat.CollisionBox, direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.SetPositon(player.Position + new Vector2(0, plat.getY()));
                UnjumpOnGroundCollide();
                break;
            case MovingPlatform_Size2 plat:
                UnCollide(plat.CollisionBox, direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.SetPositon(player.Position + new Vector2(0, plat.getY()));
                UnjumpOnGroundCollide();
                break;
            case MovingPlatform_Size3 plat:
                UnCollide(plat.CollisionBox, direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                player.SetPositon(player.Position + new Vector2(0, plat.getY()));
                UnjumpOnGroundCollide();
                break;
            case Mushroom:
                player.PowerUp(Power.Mushroom);
                HUD.Instance.SendEvent(new GameEvent
                {
                    EventType = GameEventType.PowerupCollected,
                    Data = item,
                    EventPosition = new Point(item.CollisionBox.X, item.CollisionBox.Y) + new Point(item.CollisionBox.Width / 2, item.CollisionBox.Height / 2)
                });
                break;
            case OneUp:
                player.Lives++;
                HUD.Instance.SendEvent(new GameEvent
                {
                    EventType = GameEventType.PowerupCollected,
                    Data = item,
                    EventPosition = new Point(item.CollisionBox.X, item.CollisionBox.Y) + new Point(item.CollisionBox.Width / 2, item.CollisionBox.Height / 2)
                });
                break;
            case Star:
                player.PowerUp(Power.Star);
                HUD.Instance.SendEvent(new GameEvent
                {
                    EventType = GameEventType.PowerupCollected,
                    Data = item,
                    EventPosition = new Point(item.CollisionBox.X, item.CollisionBox.Y) + new Point(item.CollisionBox.Width / 2, item.CollisionBox.Height / 2)
                });
                break;
            default:
                //Nothing
                break;
        }
    }
    public void OnCollideEnemy(IEnemy enemy, Collision.CollideDirection direction)
    {
        if (!player.IsAlive) { return; }
        if (player.Invincible) 
        {
            SoundController.PlaySound(SoundType.Stomp, 1.0f);
            return; 
        }
        switch (enemy)
        {
            case Fireball:
                player.TakeDamage();
                break;
            case Bowser bowser:
                UnCollide(bowser.CollisionBox, direction);
                player.TakeDamage();
                break;
            case PiranhaPlant:
                player.TakeDamage();
                break;
            case Goomba goomba:
                if (direction == CollideDirection.Down)
                {
                    UnCollide(goomba.CollisionBox, direction);
                    player.SetVelocity(player.Velocity - new Vector2(0, 7));
                }
                else
                {
                    player.TakeDamage();
                }
                break;
            case Koopa koopa:
                if (direction == CollideDirection.Down)
                {
                    UnCollide(koopa.CollisionBox, direction);
                    player.SetVelocity(player.Velocity - new Vector2(0, 7));
                }
                else if (direction == CollideDirection.Top || koopa.IsShellMoving() || koopa.IsWalking())
                {
                    UnCollide(koopa.CollisionBox, direction);
                    player.TakeDamage();
                }
                else
                {
                    player.InvincibilityOnEnemyContact();
                    SoundController.PlaySound(SoundType.Kick, 1.0f);
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
        if (!player.IsAlive) { return; }
        if (block is PipeEntryBlock pipe)
        {
            if(player.PipePhase != Player.PipeTravelPhase.None) { return; }
            Point? teleportCoordinates = pipe.CanEnter(direction, player.CollisionBox);
            if (teleportCoordinates.HasValue)
            {
                Vector2 travelVelocity = Vector2.Zero;
                Vector2 exitVelocity = Vector2.Zero;
                player.PipePhase = Player.PipeTravelPhase.Entering;
                switch (direction)
                {
                    case CollideDirection.Left:
                        travelVelocity = new Vector2(-4, 0); break;
                    case CollideDirection.Right:
                        travelVelocity = new Vector2(4, 0); break;
                    case CollideDirection.Top:
                        travelVelocity =  new Vector2(0, 4); break;
                    case CollideDirection.Down:
                        travelVelocity =  new Vector2(0, -4); break;
                    default:break;
                }
                switch (pipe.ExitDirection)
                {
                    case PipeEntryBlock.PipeDirection.Left:
                        exitVelocity = new Vector2(4, 0); break;
                    case PipeEntryBlock.PipeDirection.Right:
                        exitVelocity = new Vector2(-4, 0); break;
                    case PipeEntryBlock.PipeDirection.Up:
                        exitVelocity = new Vector2(0, -4); break;
                    case PipeEntryBlock.PipeDirection.Down:
                        exitVelocity = new Vector2(0, 4); break;
                    default: break;
                }
                player.PipeTravelVelocity = travelVelocity;
                player.PipeExitVelocity = exitVelocity;
                player.PipeExitPosition = teleportCoordinates.Value.ToVector2();
                SoundController.PlaySound(SoundType.PipeTravel, 1.0f);
                return;
            }
        }
        UnCollide(block.CollisionBox, direction);
        if (direction == CollideDirection.Down)
        {
            UnjumpOnGroundCollide();
        }
    }
    private void UnjumpOnGroundCollide()
    {
        player.IsGrounded = true;
        if (!player.IsJumping || !player.IsAlive) { return; }
        player.IsJumping = false;
        
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
    public void UnCollide(Rectangle block, Collision.CollideDirection direction)
    {
        switch (direction)
        {
            case Collision.CollideDirection.Top:
                player.SetPositon(new Vector2(player.Position.X, block.Bottom));
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                break;
            case Collision.CollideDirection.Down:
                player.SetPositon(new Vector2(player.Position.X, block.Top - player.CollisionBox.Height));
                player.SetVelocity(new Vector2(player.Velocity.X, 0));
                break;
            case Collision.CollideDirection.Left:
                player.SetPositon(new Vector2(block.Right, player.Position.Y));
                player.SetVelocity(new Vector2(0, player.Velocity.Y));
                break;
            case Collision.CollideDirection.Right:
                player.SetPositon(new Vector2(block.Left - player.CollisionBox.Width, player.Position.Y));
                player.SetVelocity(new Vector2(0, player.Velocity.Y));
                break;
        }
    }
}