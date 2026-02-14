using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightCrouchBigMarioState : IPlayerState
{
    private readonly Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite sprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;
    public RightCrouchBigMarioState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        sprite = texture.NewSprite(277, 11, 16, 22);
        sprite.Scale = scaleFactor;
    }
    public void Left(GameTime gameTime)
    {
        //Nothing
    }
    public void Right(GameTime gameTime)
    {
        //Nothing
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
        Mario.ChangeState(new RightSmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new RightCrouchFireMarioState(Mario, texture, timeFrame, scaleFactor));
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
        if (Player.ReleaseCrouch())
        {
            Mario.ChangeState(new RightBigMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        sprite.Position = new Point((int)Position.X, (int)Position.Y);
        sprite.Draw(spriteBatch);
    }

}
