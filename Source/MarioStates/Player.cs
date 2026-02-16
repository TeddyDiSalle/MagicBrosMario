using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class Player
{
    private IPlayerState PlayerState { get; set; }

    private Vector2 Position { get; set; } = new Vector2(400, 240);
    private Vector2 Velocity { get; set; }

    private const double timeFrame = 0.15;
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

    public readonly Color[] rainbow = [Color.Red];


    public Player(Sprite.SharedTexture texture)
    {
        PlayerState = new BigMarioIdleState(this, texture, timeFrame, scaleFactor);
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
        Idle();
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
        PlayerState.Update(gameTime, Velocity, Flipped);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch, Position);
    }
}
