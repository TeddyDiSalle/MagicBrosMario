using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Block;

public abstract class BlockBase<TBlock>(Sprite.ISprite sprite) : IBlock
    where TBlock : BlockBase<TBlock>
{
    public Sprite.ISprite Sprite { get; set; } = sprite;
    
    public bool Visible {
        get => Sprite.Visible;
        set => Sprite.Visible = value;
    }

    public TBlock WithVisibility(bool visibility) {
        Visible = visibility;
        return (TBlock)this;
    }

    public Point Position {
        get => Sprite.Position;
        set => Sprite.Position = value;
    }

    public TBlock WithPosition(int x, int y) {
        Position = new Point(x, y);
        return (TBlock)this;
    }

    public Point Size => Sprite.Size;

    public float Scale {
        get => Sprite.Scale;
        set => Sprite.Scale = value;
    }

    public TBlock WithScale(float scale) {
        Scale = scale;
        return (TBlock)this;
    }
}