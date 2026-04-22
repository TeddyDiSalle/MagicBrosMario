using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// abstract block class with useful utility methods
/// </summary>
/// <param name="sprite">sprite object from shared texture, both sprite and animated sprite works</param>
/// <typeparam name="TBlock">type of self for self referencing</typeparam>
public abstract class BlockBase<TBlock>(Sprite.ISprite sprite) : IBlock where TBlock : BlockBase<TBlock>
{
    public Sprite.ISprite Sprite
    {
        get;
        set
        {
            // sync the sprite data
            value.Position = Position;
            value.Scale = Scale;
            field = value;
        }
    } = sprite;

    public bool Visible
    {
        get => Sprite.Visible;
        set => Sprite.Visible = value;
    }

    public TBlock WithVisibility(bool visibility)
    {
        Visible = visibility;
        return (TBlock)this;
    }

    public Point Position
    {
        get => Sprite.Position;
        set => Sprite.Position = value;
    }

    public TBlock WithPosition(int x, int y)
    {
        Position = new Point(x, y);
        return (TBlock)this;
    }

    public Point Size => Sprite.Size;

    public float Scale
    {
        get => Sprite.Scale;
        set => Sprite.Scale = value;
    }

    public TBlock WithScale(float scale)
    {
        Scale = scale;
        return (TBlock)this;
    }

    public virtual void Update(GameTime gameTime) { }

    public Rectangle CollisionBox => new(sprite.Position.X, sprite.Position.Y, sprite.Size.X, sprite.Size.Y);

    public abstract void OnCollidePlayer(Player player, CollideDirection direction);

    public abstract void OnCollideItem(IItems item, CollideDirection direction);

    public abstract void OnCollideEnemy(IEnemy enemy, CollideDirection direction);

    public abstract void OnCollideBlock(IBlock block, CollideDirection direction);
}