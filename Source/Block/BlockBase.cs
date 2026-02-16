using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// abstract block class with useful utility methods
/// </summary>
/// <param name="sprite">sprite object from shared texture, both sprite and animated sprite works</param>
/// <typeparam name="TBlock">type of self for self referencing</typeparam>
public abstract class BlockBase<TBlock>(Sprite.ISprite sprite) : IBlock
    where TBlock : BlockBase<TBlock>
{
    protected Sprite.ISprite Sprite { get; set; } = sprite;

    public bool IsSolid { get; set; } = false;
    public bool IsVisible { get; set; } = true;

    public Point Position
    {
        get => Sprite.Position;
        set => Sprite.Position = value;
    }

    /// <summary>
    /// this utility method allow giving block a initial position by chaining with constructor
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns></returns>
    public BlockBase<TBlock> WithPosition(int x, int y)
    {
        Position = new Point(x, y);
        return this;
    }

    public Point Size => Sprite.Size;

    public float Scale
    {
        get => Sprite.Scale;
        set => Sprite.Scale = value;
    }

    /// <summary>
    /// this utility method allow giving block a initial scale by chaining with constructor
    /// </summary>
    /// <param name="scale">scale</param>
    /// <returns></returns>
    public BlockBase<TBlock> WithScale(float scale)
    {
        Scale = scale;
        return this;
    }

    public abstract void Update(GameTime gameTime);
    public abstract void Draw(SpriteBatch spriteBatch);
}