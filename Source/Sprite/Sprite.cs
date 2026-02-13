using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Sprite;

/// <summary>
/// non-animated sprite class that uses a <see cref="SharedTexture"/><br/>
///
/// this class should not be initiated outside SharedTexture
/// </summary>
/// <param name="texture">shared texture</param>
/// <param name="offsetX">x offset on texture</param>
/// <param name="offsetY">y offset on texture</param>
/// <param name="width">width of sprite</param>
/// <param name="height">height of sprite</param>
public class Sprite(
    SharedTexture texture,
    int offsetX,
    int offsetY,
    int width,
    int height
) : ISprite
{
    public bool IsAnimated => false;

    public Point Position
    {
        get;
        set
        {
            field = value;
            UpdateSize();
        }
    }

    public float Scale
    {
        get;
        set
        {
            field = value;
            UpdateSize();
        }
    } = 1;

    public Point Size { get; private set; }

    private readonly Rectangle sourceRect = new(offsetX, offsetY, width, height);
    private Rectangle destRect = new(0, 0, width, height);

    private void UpdateSize()
    {
        Size = new Point((int)(Scale * width), (int)(Scale * height));
        destRect = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
    }

    public void Update(GameTime gameTime)
    {
        // no update for non-animated sprite
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        //spriteBatch.Draw(texture.Texture, destRect, sourceRect, Color.White);
       // spriteBatch.Draw(texture.Texture, new Vector2(Position.X, Position.Y), sourceRect, Color.White);
        spriteBatch.Draw(texture.Texture, new Vector2(Position.X, Position.Y), sourceRect, Color.White, 0.0f,Vector2.Zero, 3.0f, SpriteEffects.None, 0.0f);
        spriteBatch.End();
    }
}