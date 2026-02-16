using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class MarioDeadState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private readonly Sprite.Sprite sprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;
    public MarioDeadState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        sprite = texture.NewSprite(182, 34, 16, 16);
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
        //Nothing
    }
    public void PowerUp(Power power)
    {
        //Nothing
    }
    public void Idle()
    {
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        //Nothing
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        sprite.Position = new Point((int)Position.X, (int)Position.Y);
        sprite.Draw(spriteBatch);
    }

}
