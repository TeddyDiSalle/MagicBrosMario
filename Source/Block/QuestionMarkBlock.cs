using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Block;

public class QuestionMarkBlock : BlockBase<QuestionMarkBlock> {
    public enum InnerItem {
        Coin,
        Star,
        FireFlower
    }

    public Rectangle CollisionBox => new(sprite.Position.X, sprite.Position.Y, sprite.Size.X, sprite.Size.Y);

    private bool empty = false;
    private AnimatedSprite sprite;
    private readonly Sprite.Sprite emptySprite;
    private readonly InnerItem innerItem;

    public QuestionMarkBlock(AnimatedSprite sprite, Sprite.Sprite emptySprite, InnerItem innerItem) : base(sprite) {
        this.sprite = sprite;
        this.emptySprite = emptySprite;
        this.innerItem = innerItem;
        sprite.Visible = true;
        emptySprite.Visible = false;
    }

    public override void OnCollidePlayer(Player player, CollideDirection direction) {
        if (empty) return;

        // if mario does not collide from below
        if (direction != CollideDirection.Down) return;

        // drop the sprite and allow gc to clean it up
        sprite.Drop();
        sprite = null;

        // spawn coins and change sprite
        Sprite = emptySprite;
        Sprite.Visible = true;
        empty = true;

        switch (innerItem) {
            case InnerItem.Coin:
                // spawn coin above
                break;
            case InnerItem.Star:
                // spawn star above
                break;
            case InnerItem.FireFlower:
                // spawn fire flower above
                break;
            default:
                throw new Exception("impossible default branch");
        }
    }

    public override void OnCollideItem(IItems item, CollideDirection direction) {
        // never gonna pick coin up, never gonna put coin down
    }

    public override void OnCollideEnemy(IEnemy enemy, CollideDirection direction) {
        // enemies don't have wallet, they can't pick up coins
    }

    public override void OnCollideBlock(IBlock block, CollideDirection direction) {
        // this shouldn't even be called
    }
}