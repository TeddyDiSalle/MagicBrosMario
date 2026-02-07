using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;

public class Player
{
    private IPlayerState PlayerState { get; set; }

    private Vector2 Position { get; set; }

    private const float MovementSpeed = 5.0f;
    
    public Player()
    {
        PlayerState = new LeftSmallMarioIdleState(this);
    }
    public void Left()
    {
        PlayerState.Left();
    }
    public void Right()
    {
        PlayerState.Right();
    }
    public void Jump()
    {
        PlayerState.Jump();
    }
    public void Crouch()
    {
        PlayerState.Crouch();
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
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch);
    }

    public void MoveLeft(GameTime gameTime)
    {
        Vector2 pos = Position;
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed;
        pos.X -= distanceMoved;
    }

    public void MoveRight(GameTime gameTime)
    {
        Vector2 pos = Position;
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed;
        pos.X += distanceMoved;
    }

}
