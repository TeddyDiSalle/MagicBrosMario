using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.MarioStates;

public class Player
{
    private IPlayerState PlayerState;
    
    public Player()
    {
        PlayerState = new LeftSmallMarioIdle();
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

}
