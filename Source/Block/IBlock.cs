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
    /// x coordinate of the block
    /// </summary>
    /// <value>x coordinate of the block</value>
    int X { get; set; }

    /// <summary>
    /// y coordinate of the block
    /// </summary>
    /// <value>y coordinate of the block</value>
    int Y { get; set; }

    /// <summary>
    /// size of this block 
    /// </summary>
    /// <value>size of this block</value>
    Vector2 Size { get; }

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