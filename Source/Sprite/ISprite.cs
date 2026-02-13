
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
    /// position of sprite
    /// </summary>
    public Point Position { get; set; }

    /// <summary>
    /// scale of sprite
    /// </summary>
    public float Scale { get; set; }

    /// <summary>
    /// size of sprite
    /// </summary>
    public Point Size { get; }

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