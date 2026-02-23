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

    private Vector2 Position { get; set; } = new Vector2(400, 240);
    private Vector2 Velocity { get; set; }

    private const float timeFrame = 0.15f;
    private const int scaleFactor = 3;

    private const float MovementSpeed = 3.0f;
    private const float Gravity = 0.35f;
    private float GroundY = 260; //Temporary for Sprint2
    private const float MaxSpeed = 15.0f;
    public bool IsCrouching { get; private set; } = false;
    public bool Flipped { get; set; } = false;
    public bool Invincible { get; set; } = false;
    public double StarDuration { get; private set; } = 10;
    public double StarTimeRemaining { get; set; } = 0;

    private List<MarioFireball> fireballs = new List<MarioFireball>();
    private const float fireballCooldown = 0.2f;
    private float fireballTimeRemaining = 0;

    private readonly Sprite.SharedTexture texture;

    public Player(Sprite.SharedTexture texture)
    {
        PlayerState = new SmallMarioIdleState(this, texture, timeFrame, scaleFactor);
        this.texture = texture;
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
        PlayerState.Attack();
    }
    public void TakeDamage()
    {
        PlayerState.TakeDamage();
    }
    public void PowerUp(Power power)
    {
        PlayerState.PowerUp(power);
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
        float distanceMoved = (float)(gameTime.ElapsedGameTime.TotalSeconds * 100 * MovementSpeed);
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
