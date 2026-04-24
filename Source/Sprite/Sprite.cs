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
    public bool Animated => false;
    public bool Visible { get; set; } = true;
    public bool HFlipped { get; set; } = false;
    public bool VFlipped { get; set; } = false;

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

    public Color Color { get; set; } = Color.White;
    public float Depth { get; set; } = 1.0f;

    private readonly Rectangle sourceRect = new(offsetX, offsetY, width, height);
    private Rectangle destRect = new(0, 0, width, height);
    private bool shouldDraw = true;

    private void UpdateSize()
    {
        Size = new Point((int)(Scale * width), (int)(Scale * height));
        UpdateDestRect();
    }

    public void UpdateDestRect()
    {
        var screenPosition = Camera.Instance.Position;
        destRect = new Rectangle(Position.X - screenPosition.X, Position.Y - screenPosition.Y, Size.X, Size.Y);
        shouldDraw = Camera.Instance.ShouldDraw(destRect);
    }

    public void Update(GameTime gameTime)
    {
        // no update for non-animated sprite
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!(Visible && shouldDraw)) return;

        if (HFlipped)
        {
            if (VFlipped)
            {
                spriteBatch.Draw(texture.Texture, destRect, sourceRect, Color, 180f, Vector2.Zero,
                    SpriteEffects.None, Depth);
            } else
            {
                spriteBatch.Draw(texture.Texture, destRect, sourceRect, Color, 0f, Vector2.Zero,
                    SpriteEffects.FlipHorizontally, Depth);
            }
        } else
        {
            if (VFlipped)
            {
                spriteBatch.Draw(texture.Texture, destRect, sourceRect, Color, 0f, Vector2.Zero,
                    SpriteEffects.FlipVertically, Depth);
            } else
            {
                spriteBatch.Draw(texture.Texture, destRect, sourceRect, Color, 0f, Vector2.Zero,
                    SpriteEffects.None, Depth);
            }
        }
    }

    public void Drop()
    {
        Camera.Instance.Sprites.Remove(this);
    }

    public void Background()
    {
        Depth = 0.0f;
    }

    public void Midground()
    {
        Depth = 0.5f;
    }
}