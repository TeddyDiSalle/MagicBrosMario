using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class LeftJumpSmallMarioState : IPlayerState
{
    private Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite sprite;

    public LeftJumpSmallMarioState(Player Mario, Sprite.SharedTexture texture)
    {
        this.Mario = Mario;
        this.texture = texture;
        sprite = new Sprite.Sprite(texture, 142, 44, 16, 16);
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
    }
    public void Jump(GameTime gameTime)
    {
        //Nothing
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
        Mario.ChangeState(new DeadMarioState(Mario, texture));
    }
    public void PowerUp(Power power)
    {
        //switch (power)
        //{
        //    case Power.FireFlower:
        //        Mario.ChangeState(new LeftJumpFireMarioState(Mario, texture));
        //        break;
        //    case Power.Mushroom:
        //        Mario.ChangeState(new LeftJumpBigMarioState(Mario, texture));
        //        break;
        //    case Power.Star:
        //        //RainbowState?
        //        break;
        //}
    }
    public void Idle()
    {
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity)
    {
        if(Velocity.Y == 0)
        {
            Mario.ChangeState(new LeftSmallMarioIdleState(Mario, texture));
        }
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        sprite.Position = new Point((int)Position.X, (int)Position.Y);
        sprite.Draw(spriteBatch);
    }

}
