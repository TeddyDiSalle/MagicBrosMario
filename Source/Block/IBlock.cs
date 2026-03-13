using MagicBrosMario.Source.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// interface for all block related class
/// </summary>
public interface IBlock
{
    /// <summary>
    /// whether this block is visible
    /// </summary>
    bool Visible { get; set; }

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

    Sprite.ISprite Sprite { get;set; }
}