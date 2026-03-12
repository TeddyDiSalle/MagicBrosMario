using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;


namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class Player : ICollidable
{
    private IPlayerState PlayerState { get; set; }

    public Vector2 Position { get; private set; } = new Vector2(400, 240);
    public Vector2 Velocity { get; private set; }
    public int scaleFactor { get; } = 3;
    private float GroundY = 260; //Temporary for Sprint2
    private const float MovementSpeed = 5.0f, Gravity = 0.35f, MaxSpeed = 15.0f, fireballCooldown = 0.2f;
    public float timeFrame { get; } = 0.15f;
    public bool IsGrounded { get; set; } = true;
    public bool IsCrouching { get; private set; } = false;
    public bool Flipped { get; set; } = false;
    public bool Invincible { get; set; } = false;
    private double StarDuration { get; set; } = 10;
    public double StarTimeRemaining { get; set; } = 0;
    private List<MarioFireball> fireballs = [];
    private float fireballTimeRemaining = 0;
    public int lives { get; set; } = 3;
    private const double DamageCoolDown = 2.0;
    private double DamageTimer = 0;
    public Sprite.SharedTexture texture { get; }
    public bool IsJumping { get; set; } = false;
    private PlayerCollisionHandler playerCollision;
    public Rectangle CollisionBox { get; set; }

    public Player(Sprite.SharedTexture texture)
    {
        this.texture = texture;
        CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        PlayerState = new SmallMarioIdleState(this, texture, timeFrame, scaleFactor);
        playerCollision = new PlayerCollisionHandler(this);
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
    public void SetVelocity(Vector2 vel)
    {
        Velocity = vel;
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
        if (DamageTimer >= DamageCoolDown)
        {
            DamageTimer = 0;
            PlayerState.TakeDamage();
        }
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

    public void MoveLeft(GameTime gameTime, float factor = 1.0f)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed * factor;

        Velocity -= new Vector2(distanceMoved, 0);
        if(-Velocity.X > MaxSpeed)
        {
            Velocity = new Vector2(-MaxSpeed, Velocity.Y);
        }
    }
    public void MoveRight(GameTime gameTime, float factor = 1.0f)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed * factor;

        Velocity += new Vector2(distanceMoved, 0);
        if (Velocity.X > MaxSpeed)
        {
            Velocity = new Vector2(MaxSpeed, Velocity.Y);
        }
    }
    public void MoveUp(GameTime gameTime, float factor = 1.0f)
    {
        float distanceMoved = (float)(gameTime.ElapsedGameTime.TotalSeconds * 45 * MovementSpeed * factor);
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
        playerCollision.OnCollidePlayer(player, direction);
    }
    public void OnCollideItem(IItems item, Collision.CollideDirection direction)
    {
        playerCollision.OnCollideItem(item, direction);
    }
    public void OnCollideEnemy(IEnemy enemy, Collision.CollideDirection direction)
    {
        playerCollision.OnCollideEnemy(enemy, direction);
    }
    public void OnCollideBlock(IBlock block, Collision.CollideDirection direction)
    {
        playerCollision.OnCollideBlock(block, direction);
    }

    //Update and Draw
    public void Update(GameTime gameTime)
    {
        if(DamageTimer < DamageCoolDown)
        {
            DamageTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }
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
        PlayerState.Update(gameTime);
        CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, CollisionBox.Width, CollisionBox.Height);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch);
        foreach(MarioFireball fireball in fireballs)
        {
            fireball.Draw(spriteBatch);
        }
    }
}
