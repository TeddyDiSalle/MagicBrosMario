using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightSmallMarioIdleState : IPlayerState
{
    private Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite sprite;

    public RightSmallMarioIdleState(Player Mario, Sprite.SharedTexture texture)
    {
        this.Mario = Mario;
        this.texture = texture;
        sprite = new Sprite.Sprite(texture, 277, 44, 12, 16);
        sprite.Scale = 5;
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
        Mario.ChangeState(new LeftSmallMarioMoveState(Mario, texture));
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
        Mario.ChangeState(new RightSmallMarioMoveState(Mario, texture));
    }

    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new RightJumpSmallMarioState(Mario, texture));
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
    //    switch (power)
    //    {
    //        case Power.FireFlower:
    //            Mario.ChangeState(new RightFireMarioIdleState(Mario, texture));
    //            break;
    //        case Power.Mushroom:
    //            Mario.ChangeState(new RightBigMarioIdleState(Mario, texture));
    //            break;
    //        case Power.Star:
    //            //RainbowState?
    //            break;
    //    }
    }
    public void Idle()
    {
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity)
    {
        //Nothing
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        sprite.Position = new Point((int)Position.X, (int)Position.Y);
        sprite.Draw(spriteBatch);
    }

}
