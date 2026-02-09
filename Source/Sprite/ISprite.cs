using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Sprite;

/// <summary>
/// sprite interface for animated and non-animated sprite
/// </summary>
public interface ISprite
{
    /// <summary>
    /// true for animated sprite and false for non-animated
    /// </summary>
    public bool IsAnimated { get; }

    /// <summary>
    /// x position of sprite
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// y position of sprite
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// destination rectangle, contains the drawing width and height
    /// </summary>
    public Rectangle DestRect { get; }

    /// <summary>
    /// scale of sprite
    /// </summary>
    public float Scale { get; set; }

    /// <summary>
    /// update function for sprite, currently only used by AnimatedSprite
    /// </summary>
    /// <param name="gameTime">gameTime</param>
    public void Update(GameTime gameTime);

    /// <summary>
    /// draw function for sprite
    ///
    /// there is no IsVisible and OutOfScreen check for sprite
    /// </summary>
    /// <param name="spriteBatch"></param>
    public void Draw(SpriteBatch spriteBatch);
}