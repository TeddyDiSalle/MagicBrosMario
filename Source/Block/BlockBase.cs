using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// abstract block class with useful utility methods
/// </summary>
/// <param name="sprite">sprite object from shared texture, both sprite and animated sprite works</param>
/// <typeparam name="TBlock">type of self for self referencing</typeparam>
public abstract class BlockBase<TBlock>(Sprite.ISprite sprite) : IBlock where TBlock : BlockBase<TBlock> {
    protected Sprite.ISprite Sprite {
        get;
        set {
            // sync the sprite data
            value.Position = Position;
            value.Scale = Scale;
            field = value;
        }
    } = sprite;

    public bool Visible {
        get => Sprite.Visible;
        set => Sprite.Visible = value;
    }

    /// <summary>
    /// this utility method allow giving block a initial visibility by chaining with constructor
    /// </summary>
    /// <param name="visibility">visibility</param>
    /// <returns></returns>
    public BlockBase<TBlock> WithVisibility(bool visibility) {
        Visible = visibility;
        return this;
    }

    public Point Position {
        get => Sprite.Position;
        set => Sprite.Position = value;
    }

    /// <summary>
    /// this utility method allow giving block a initial position by chaining with constructor
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns></returns>
    public BlockBase<TBlock> WithPosition(int x, int y) {
        Position = new Point(x, y);
        return this;
    }

    public Point Size => Sprite.Size;

    public float Scale {
        get => Sprite.Scale;
        set => Sprite.Scale = value;
    }

    /// <summary>
    /// this utility method allow giving block an initial scale by chaining with constructor
    /// </summary>
    /// <param name="scale">scale</param>
    /// <returns></returns>
    public BlockBase<TBlock> WithScale(float scale) {
        Scale = scale;
        return this;
    }
}