using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;

public class LeftSmallMarioIdleState : IPlayerState
{
    private Player Mario;

    public LeftSmallMarioIdleState(Player Mario)
    {
        this.Mario = Mario;
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
        //Mario = new LeftSmallMarioMoveState(Mario);
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
        //Mario = new RightSmallMarioMoveState(Mario);
    }
    public void Jump(GameTime gameTime)
    {

    }
    public void Crouch(GameTime gameTime)
    {
        //Nothing
    }
    public void Attack()
    {

    }
    public void TakeDamage()
    {

    }
    public void PowerUp(Power power)
    {

    }
    public void Update(GameTime gameTime)
    {

    }
    public void Draw(SpriteBatch spriteBatch)
    {

    }

}
