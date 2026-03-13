using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class MarioDeadState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private readonly Sprite.Sprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;
    public MarioDeadState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        CurrentSprite = texture.NewSprite(136, 2, 16, 16);
        CurrentSprite.Scale = scaleFactor;
        Mario.KillMario();
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 0 * scaleFactor, 0 * scaleFactor);
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
        //Nothing
    }
    public void PowerUp(Power power)
    {
        //Nothing
    }
    public Power GetCurrentPower()
    {
        return Power.None;
    }
    public void Idle()
    {
        //Nothing
    }
    public void StateChangePrep()
    {
        CurrentSprite.Visible = false;
        CurrentSprite.Drop();
    }

    public void Update(GameTime gameTime)
    {
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
