using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// interface for all block related class
/// </summary>
public interface IBlock
{
    /// <summary>
    /// whether this block is collidable
    /// </summary>
    bool IsSolid { get; set; }

    /// <summary>
    /// whether this block is visible
    /// </summary>
    bool IsVisible { get; set; }

    /// <summary>
    /// position of this block
    /// </summary>
    /// <value>x coordinate of this block</value>
    Point Position { get; set; }

    /// <summary>
    /// size of this block 
    /// </summary>
    /// <value>size of this block</value>
    Point Size { get; }

    /// <summary>
    /// scale of this block 
    /// </summary>
    /// <value>scale of this block</value>
    float Scale { get; set; }

    /// <summary>
    /// update function for blocks
    /// </summary>
    /// <param name="gameTime">gameTime</param>
    void Update(GameTime gameTime);

    /// <summary>
    /// draw function for blocks
    ///
    /// function implementation should check IsVisible before drawing
    /// </summary>
    /// <param name="spriteBatch">spriteBatch</param>
    void Draw(SpriteBatch spriteBatch);
}