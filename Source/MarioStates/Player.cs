using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class Player : ICollidable
{
    private IPlayerState PlayerState { get; set; }

    public Vector2 Position { get; private set; } = new Vector2(400, 240);
    public Vector2 Velocity { get; private set; }
    private const int scaleFactor = 3;
    private float GroundY = 1260; //Temporary for Sprint2
    private const float timeFrame = 0.15f, MovementSpeed = 15.0f, Gravity = 0.35f, MaxSpeed = 15.0f, fireballCooldown = 0.2f;
    public bool IsCrouching { get; private set; } = false;
    public bool Flipped { get; set; } = false;
    public bool Invincible { get; set; } = false;
    private double StarDuration { get; set; } = 10;
    public double StarTimeRemaining { get; set; } = 0;
    private List<MarioFireball> fireballs = [];
    private float fireballTimeRemaining = 0;
    private int lives = 3;
    private readonly Sprite.SharedTexture texture;

    public Rectangle CollisionBox { get; set; }

    public Player(Sprite.SharedTexture texture)
    {
        this.texture = texture;
        CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        PlayerState = new SmallMarioIdleState(this, texture, timeFrame, scaleFactor);
    }
    public void CreateFireball()
    {
        if(fireballTimeRemaining < fireballCooldown) { return; }
        fireballTimeRemaining = 0;
        AnimatedSprite movingFireball = new(texture, 207, 168, 8, 8, 4, timeFrame);
        Sprite.Sprite explosion = new(texture, 239, 168, 8, 8);
        movingFireball.Scale = scaleFactor;
        explosion.Scale = scaleFactor;
        movingFireball.Flipped = Flipped;
        explosion.Flipped = Flipped;
        MarioFireball fireball = new(movingFireball, explosion, Position + new Vector2(16, 0), !Flipped, GroundY);
        fireballs.Add(fireball);
    }
    public void Left(GameTime gameTime)
    {
        Flipped = true;
        PlayerState.Left(gameTime);
    }
    public void Right(GameTime gameTime)
    {
        Flipped = false;
        PlayerState.Right(gameTime);
    }
    public void Jump(GameTime gameTime)
    {
        PlayerState.Jump(gameTime);
    }
    public void Crouch(GameTime gameTime)
    {
        IsCrouching = true;
        PlayerState.Crouch(gameTime);
    }
    public void SetPositon(Vector2 pos)
    {
        Position = pos;
    }
    public void ReleaseCrouch()
    {
        IsCrouching = false;
    }
    public void Attack()
    {
        if(fireballs.Count < 2)
            PlayerState.Attack();
    }
    public void TakeDamage()
    {
        PlayerState.TakeDamage();
    }
    public void KillMario()
    {
        lives--;
    }
    public bool IsAlive()
    {
        return lives != 0;
    }
    public void PowerUp(Power power)
    {
        PlayerState.PowerUp(power);
    }
    public Power GetCurrentPower()
    {
        return PlayerState.GetCurrentPower();
    }

    public void ChangeState(IPlayerState state)
    {
        PlayerState = state;
    }

    public void MoveLeft(GameTime gameTime, int factor)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed * factor;

        Velocity -= new Vector2(distanceMoved, 0);
        if(-Velocity.X > MaxSpeed)
        {
            Velocity = new Vector2(-MaxSpeed, Velocity.Y);
        }
    }
    public void MoveRight(GameTime gameTime, int factor)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed * factor;

        Velocity += new Vector2(distanceMoved, 0);
        if (Velocity.X > MaxSpeed)
        {
            Velocity = new Vector2(MaxSpeed, Velocity.Y);
        }
    }
    public void MoveUp(GameTime gameTime)
    {
        float distanceMoved = (float)(gameTime.ElapsedGameTime.TotalSeconds * 40 * MovementSpeed);
        Velocity -= new Vector2(0, distanceMoved);
    }
    public void Idle()
    {
        PlayerState.Idle();
        if (Velocity.X < 0)
        {
            Velocity += new Vector2(0.1f, 0);
            if(Velocity.X > 0)
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }
        else if(Velocity.X > 0)
        {
            Velocity -= new Vector2(0.1f, 0);
            if (Velocity.X < 0)
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }
    }

    //Collision Handling Methods
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
                    Position += new Vector2(cloud.getX(), 0);
                    Velocity = new Vector2(Velocity.X, 0);
                }
                break;
            case Fireflower:
            case Fireflower_Underground:
                PlayerState.PowerUp(Power.FireFlower);
                break;
            case MovingPlatform_Size1 plat:
                UnCollide(Rectangle.Intersect(CollisionBox, plat.CollisionBox), direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                Velocity = new Vector2(Velocity.X, 0);
                Position += new Vector2(0, plat.getY());
                break;
            case MovingPlatform_Size2 plat:
                UnCollide(Rectangle.Intersect(CollisionBox, plat.CollisionBox), direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                Velocity = new Vector2(Velocity.X, 0);
                Position += new Vector2(0, plat.getY());
                break;
            case MovingPlatform_Size3 plat:
                UnCollide(Rectangle.Intersect(CollisionBox, plat.CollisionBox), direction);
                if (direction is CollideDirection.Left or CollideDirection.Right) { return; }
                Velocity = new Vector2(Velocity.X, 0);
                Position += new Vector2(0, plat.getY());
                break;
            case Mushroom:
                PlayerState.PowerUp(Power.Mushroom);
                break;
            case OneUp:
                lives++;
                break;
            case Spring_Stretched:
                //Uncollide
                Velocity -= new Vector2(Velocity.X, 10);
                break;
            case Star:
                PlayerState.PowerUp(Power.Star);
                break;
            default:
                //Nothing
                break;
        }
    }
    public void OnCollideEnemy(IEnemy enemy, Collision.CollideDirection direction)
    {
        if (Invincible) { return; }
        switch (enemy)
        {
            case Fireball:
                PlayerState.TakeDamage();
                break;
            case Bowser bowser:
                UnCollide(Rectangle.Intersect(CollisionBox, bowser.CollisionBox), direction);
                break;
            case PiranhaPlant:
                PlayerState.TakeDamage();
                break;
            case Goomba goomba:
                UnCollide(Rectangle.Intersect(CollisionBox, goomba.CollisionBox), direction);
                if (direction == CollideDirection.Down) 
                {
                    Velocity -= new Vector2(0, 10);
                }
                else
                {
                    PlayerState.TakeDamage();
                }
                break;
            case Koopa koopa:
                UnCollide(Rectangle.Intersect(CollisionBox, koopa.CollisionBox), direction);
                if (direction == CollideDirection.Down)
                {
                    Velocity -= new Vector2(0, 10);
                }
                else
                {
                    PlayerState.TakeDamage();
                }
                break;
            case RotatingFireBar:
                PlayerState.TakeDamage();
                break;
            default:
                //Nothing
                break;

        }
    }
    public void OnCollideBlock(IBlock block, Collision.CollideDirection direction)
    {
        // if (!block.IsSolid) return;
        //Uncollide
        Rectangle intersect = Rectangle.Intersect(CollisionBox, new Rectangle(block.Position.X, block.Position.Y, block.Size.X, block.Size.Y));
        UnCollide(intersect, direction);
    }

    public void UnCollide(Rectangle intersect, Collision.CollideDirection direction)
    {
        switch (direction)
        {
            case Collision.CollideDirection.Top:
                Position += new Vector2(0, intersect.Height);
                Velocity = new Vector2(Velocity.X, 0);
                break;
            case Collision.CollideDirection.Down:
                Position -= new Vector2(0, intersect.Height);
                Velocity = new Vector2(Velocity.X, 0);
                break;
            case Collision.CollideDirection.Left:
                Position += new Vector2(intersect.X, 0);
                Velocity = new Vector2(0, Velocity.Y);
                break;
            case Collision.CollideDirection.Right:
                Position -= new Vector2(intersect.X, 0);
                Velocity = new Vector2(0, Velocity.Y);
                break;
        }
    }

    //Update and Draw
    public void Update(GameTime gameTime)
    {
        if (Position.Y < GroundY)
        {
            Velocity += new Vector2(0, Gravity);
        }
        Position += Velocity;
        if (Position.Y > GroundY)
        {
            Position = new Vector2(Position.X, GroundY);
            Velocity -= new Vector2(0, Velocity.Y);
        }
        if(StarTimeRemaining >= StarDuration)
        {
            StarTimeRemaining = 0;
            Invincible = false;
        }
        if (IsCrouching)
        {
            Idle();
        }
        if(fireballTimeRemaining < fireballCooldown)
        {
            fireballTimeRemaining += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        for (int i = 0; i < fireballs.Count; i++)
        {
            fireballs[i].Update(gameTime);

            if (fireballs[i].IsExpired())
            {
                fireballs.RemoveAt(i);
            }
        }
        
        PlayerState.Update(gameTime, Velocity, Flipped);
        CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, CollisionBox.Width, CollisionBox.Height);
        Camera.Instance.Follow(Position);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch, Position);
        foreach(MarioFireball fireball in fireballs)
        {
            fireball.Draw(spriteBatch);
        }
    }
}
