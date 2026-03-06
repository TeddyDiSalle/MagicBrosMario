using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class Player
{
    private IPlayerState PlayerState { get; set; }

    public Vector2 Position { get; private set; } = new Vector2(400, 240);
    public Vector2 Velocity { get; private set; }
    private const int scaleFactor = 3;
    private float GroundY = 260; //Temporary for Sprint2
    private const float timeFrame = 0.15f, MovementSpeed = 7.0f, Gravity = 0.35f, MaxSpeed = 15.0f, fireballCooldown = 0.2f;
    public bool IsCrouching { get; private set; } = false;
    public bool Flipped { get; set; } = false;
    public bool Invincible { get; set; } = false;
    private double StarDuration { get; set; } = 10;
    public double StarTimeRemaining { get; set; } = 0;
    private List<MarioFireball> fireballs = [];
    private float fireballTimeRemaining = 0;
    private int lives = 3;
    private readonly Sprite.SharedTexture texture;

    public MarioCollision collision { get; private set; } 

    public Player(Sprite.SharedTexture texture)
    {
        this.texture = texture;
        collision = new MarioCollision(this);
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
    public void AddLife()
    {
        lives++;
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
        float distanceMoved = (float)(gameTime.ElapsedGameTime.TotalSeconds * 80 * MovementSpeed);
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

    public void OnGround(float NewGroundY)
    {
        GroundY = NewGroundY;
    }
    public void SetPositon(Vector2 pos)
    {
        Position = pos;
    }
    public void AddToPositon(Vector2 dxdy)
    {
        Position += dxdy;
    }
    public void AddToVelocity(Vector2 dxdy)
    {
        Velocity += dxdy;
    }

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
        collision.CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, collision.CollisionBox.Width, collision.CollisionBox.Height);
        PlayerState.Update(gameTime, Velocity, Flipped);
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
