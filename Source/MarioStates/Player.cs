using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.MarioStates;

public class Player
{
    private IPlayerState PlayerState { get; set; }


    private Vector2 Position { get; set; }
    private Vector2 Velocity { get; set; }

    private const float MovementSpeed = 8.0f;
    private const float Gravity = 10.0f;
    private float GroundY = 50; //Temporary for Sprint2
    
    public Player(SharedTexture texture)
    {
        PlayerState = new LeftSmallMarioIdleState(this, texture);
    }
    public void Left(GameTime gameTime)
    {
        PlayerState.Left(gameTime);
    }
    public void Right(GameTime gameTime)
    {
        PlayerState.Right(gameTime);
    }
    public void Jump(GameTime gameTime)
    {
        PlayerState.Jump(gameTime);
    }
    public void Crouch(GameTime gameTime)
    {
        PlayerState.Crouch(gameTime);
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
    public void Update(GameTime gameTime)
    {
        PlayerState.Update(gameTime);
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
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch);
    }

    public void MoveLeft(GameTime gameTime)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed;
        Velocity -= new Vector2(distanceMoved, 0);
    }

    public void MoveRight(GameTime gameTime)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed;
        Velocity += new Vector2(distanceMoved, 0);
    }

    public void Idle()
    {
        if (Velocity.X > 0)
        {
            Velocity -= new Vector2(MovementSpeed, 0);
        }
        else if (Velocity.X < 0)
        {
            Velocity += new Vector2(MovementSpeed, 0);
        }
    }

    public void OnGround(float NewGroundY)
    {
        GroundY = NewGroundY;
    }
    public void MoveUp(GameTime gameTime)
    {
        float distanceMoved = (float)(gameTime.ElapsedGameTime.TotalSeconds * 3 * MovementSpeed);
        Velocity += new Vector2(0, distanceMoved);
    }
}
