using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;

public class LeftSmallMarioMoveState : IPlayerState
{
    private Player Mario;
    private SharedTexture texture;
    private AnimatedSprite sprite;

    public LeftSmallMarioMoveState(Player Mario, SharedTexture texture)
    {
        this.Mario = Mario;
        this.texture = texture;
        sprite = new AnimatedSprite(texture);
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
        
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
        //Mario = new RightSmallMarioMoveState(Mario);
    }
    public void Jump(GameTime gameTime)
    {
        //Mario = new LeftJumpSmallMarioState(Mario);
    }
    public void Crouch(GameTime gameTime)
    {
        //Nothing
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        //Mario = new DeadMarioState(Mario);
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                //Mario = new LeftFireMarioState(Mario);
                break;
            case Power.Mushroom:
                //Mario = new LeftBigMarioState(Mario);
                break;
            case Power.Star:
                //RainbowState?
                break;
        }
    }
    public void Update(GameTime gameTime)
    {
        //Nothing
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        sprite.Draw(spriteBatch);
    }

}
