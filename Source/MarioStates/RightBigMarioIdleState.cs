using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightBigMarioIdleState : IPlayerState
{
    private readonly Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite sprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;
    public RightBigMarioIdleState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        sprite = texture.NewSprite(258, 1, 16, 32);
        sprite.Scale = scaleFactor;
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime, 1);
        Mario.ChangeState(new LeftBigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime, 1);
        Mario.ChangeState(new RightBigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
    }

    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new RightJumpBigMarioState(Mario, texture, timeFrame, scaleFactor));
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
        Mario.ChangeState(new RightSmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new RightFireMarioIdleState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Mushroom:
                //Nothing
                break;
            case Power.Star:
                //RainbowState?
                break;
        }
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
