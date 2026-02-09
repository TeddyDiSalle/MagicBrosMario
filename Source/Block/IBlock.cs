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
    /// position of this block relative to the absolute origin(ignoring offset due to camera)
    /// </summary>
    /// <value>position of this block</value>
    Vector2 Position { get; set; }

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
    /// load content function for blocks
    ///
    /// all property of the block should be valid after this function
    /// </summary>
    void LoadContent();
    
    /// <summary>
    /// draw function for blocks
    ///
    /// function implementation should check IsVisible before drawing
    /// </summary>
    /// <param name="spriteBatch">spriteBatch</param>
    /// <param name="gameTime">gameTime</param>
    void Draw(SpriteBatch spriteBatch, GameTime gameTime);
}