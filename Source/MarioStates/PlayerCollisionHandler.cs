using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sound;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using static MagicBrosMario.Source.MarioStates.Player;



namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class PlayerCollisionHandler
{
    private readonly Player player;
    private bool ReachedFlagpole = false;
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
            case PoisonMushroom:
                player.DamageTimer = 2;
                player.TakeDamage();
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
            case FlagPole flagPole:
                if (ReachedFlagpole) { return; }
                ReachedFlagpole = true;
                float yLandValue = player.CollisionBox.Bottom;
                player.SetPositon(new Vector2(flagPole.CollisionBox.X - flagPole.CollisionBox.Width, player.Position.Y));
                player.FlagPoleBottomY = flagPole.CollisionBox.Y + flagPole.CollisionBox.Height-player.CollisionBox.Height;
                player.CastleEntranceX = player.Position.X + 230;
                player.EndPhase = EndLevelPhase.SlidingDown;
                IPlayerState sliding = null;
                switch (player.GetCurrentPower())
                {
                    case Power.Mushroom:
                        sliding = new BigMarioSlideState(player, player.Texture, player.TimeFrame, player.ScaleFactor);
                        break;
                    case Power.FireFlower:
                        sliding = new FireMarioSlideState(player, player.Texture, player.TimeFrame, player.ScaleFactor);
                        break;
                    default:
                        sliding = new SmallMarioSlideState(player, player.Texture, player.TimeFrame, player.ScaleFactor);
                        break;
                }
                player.ChangeState(sliding);
                HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.FlagpoleReached, EventPosition = new Point(flagPole.CollisionBox.X, flagPole.CollisionBox.Y), Data = (yLandValue, flagPole.CollisionBox) });
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
                if (direction == CollideDirection.Down && !MarioGameController.IsMarioDown())
                {
                    UnCollide(block.CollisionBox, direction);
                    if (direction == CollideDirection.Down)
                    {
                        UnjumpOnGroundCollide();
                    }
                    return; 
                }
                
                Vector2 travelVelocity = Vector2.Zero;
                Vector2 exitVelocity = Vector2.Zero;
                player.PipePhase = Player.PipeTravelPhase.Entering;
                switch (direction)
                {
                    case CollideDirection.Left:
                        travelVelocity = new Vector2(-4, 0);
                        player.PipeEntryDestination = new Vector2(pipe.CollisionBox.Left - player.CollisionBox.Width, player.Position.Y);
                        break;
                    case CollideDirection.Right:
                        travelVelocity = new Vector2(4, 0);
                        player.PipeEntryDestination = new Vector2(pipe.CollisionBox.Right, player.Position.Y);
                        break;
                    case CollideDirection.Top:
                        travelVelocity = new Vector2(0, 4);
                        player.PipeEntryDestination = new Vector2(player.Position.X, pipe.CollisionBox.Bottom);
                        break;
                    case CollideDirection.Down:
                        travelVelocity = new Vector2(0, -4);
                        player.PipeEntryDestination = new Vector2(player.Position.X, pipe.CollisionBox.Top - player.CollisionBox.Height);
                        break;
                    default: break;
                }
                
                player.PipeTravelVelocity = travelVelocity;

            var exitData = GetPipeExitData(teleportCoordinates.Value, pipe.ExitDirection);
            player.PipeExitPosition = exitData.start;
            player.PipeExitDestination = exitData.finish;
            player.PipeExitVelocity = exitData.velocity;
                SoundController.PlaySound(SoundType.PipeTravel, 1.0f);
                return;
            }
        }
        if(player.PipePhase == PipeTravelPhase.Exiting || player.PipePhase == PipeTravelPhase.Exiting) { return; }
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
    private (Vector2 start, Vector2 finish, Vector2 velocity) GetPipeExitData(
    Point anchor,
    PipeEntryBlock.PipeDirection? exitDirection)
{
    int tile = 16 * player.ScaleFactor;   // 32
    int pipeSpan = tile * 2;              // 64

    float centeredX = anchor.X + (pipeSpan - player.CollisionBox.Width) / 2f;
    float centeredY = anchor.Y + (pipeSpan - player.CollisionBox.Height) / 2f;

    return exitDirection switch
    {
        PipeEntryBlock.PipeDirection.Up => (
            new Vector2(centeredX, anchor.Y + tile),
            new Vector2(centeredX, anchor.Y - player.CollisionBox.Height),
            new Vector2(0, -4)
        ),

        PipeEntryBlock.PipeDirection.Down => (
            new Vector2(centeredX, anchor.Y - player.CollisionBox.Height),
            new Vector2(centeredX, anchor.Y + tile),
            new Vector2(0, 4)
        ),

        PipeEntryBlock.PipeDirection.Left => (
            new Vector2(anchor.X + tile, centeredY),
            new Vector2(anchor.X - player.CollisionBox.Width, centeredY),
            new Vector2(-4, 0)
        ),

        PipeEntryBlock.PipeDirection.Right => (
            new Vector2(anchor.X - player.CollisionBox.Width, centeredY),
            new Vector2(anchor.X + tile, centeredY),
            new Vector2(4, 0)
        ),

        _ => (
            anchor.ToVector2(),
            anchor.ToVector2(),
            Vector2.Zero
        )
    };
}
}