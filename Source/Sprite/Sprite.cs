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

    public int X { get; set; }

    public int Y { get; set; }

    public Rectangle DestRect { get; private set; }

    public float Scale
    {
        get;
        set
        {
            // update field and destination rectangle
            field = value;
            DestRect = new Rectangle(X, Y, (int)(value * width), (int)(value * height));
        }
    } = 1;

    private readonly Rectangle sourceRect = new(offsetX, offsetY, width, height);

    public void Update(GameTime gameTime)
    {
        // no update for non-animated sprite
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(texture.Texture, sourceRect, DestRect, Color.White);
    }
}