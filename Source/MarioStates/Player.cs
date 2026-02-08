using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;

public class Player
{
    private IPlayerState PlayerState { get; set; }

    private Vector2 Position { get; set; }
    private Vector2 Velocity { get; set; }

    private const float MovementSpeed = 5.0f;
    private const float Gravity = 5.0f;
    private bool InAir = false;
    
    public Player()
    {
        PlayerState = new LeftSmallMarioIdleState(this);
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
        if (InAir)
        {
            Velocity = new Vector2(Velocity.X, Velocity.Y + Gravity);
        }
        Position += Velocity;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch);
    }

    public void MoveLeft(GameTime gameTime)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed;
        Velocity = new Vector2(Velocity.X - distanceMoved, Velocity.Y);
    }

    public void MoveRight(GameTime gameTime)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed;
        Velocity = new Vector2(Velocity.X + distanceMoved, Velocity.Y);
    }

    public void Idle()
    {
        Velocity = Vector2.Zero;
    }

    public void OnGround()
    {
        InAir = false;
        Velocity = new Vector2(Velocity.X, 0);
    }
    public void MoveUp(GameTime gameTime)
    {
        InAir = true;
        Velocity = new Vector2(Velocity.X, Velocity.Y);
    }



}
