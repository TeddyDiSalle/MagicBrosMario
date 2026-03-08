using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

public class QuestionMarkBlock(AnimatedSprite sprite, Sprite.Sprite emptySprite) : BlockBase<QuestionMarkBlock>(sprite), ICollidable {
    public override void Update(GameTime gameTime) {
    }

    public override void Draw(SpriteBatch spriteBatch) {
    }

    /* ICollidable methods */

    public Rectangle CollisionBox => new(sprite.Position.X, sprite.Position.Y, sprite.Size.X, sprite.Size.Y);

    private bool empty = false;

    // ReSharper disable once InvertIf
    public void OnCollidePlayer(Player player, CollideDirection direction) {
        if (empty) return;

        // spawn coins and change sprite
        if (direction == CollideDirection.Down) {
            Sprite = emptySprite;
            empty = true;
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction) {
        // never gonna pick coin up, never gonna put coin down
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) {
        // enemies don't have wallet, they can't pick up coins
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction) {
        // this shouldn't even be called
    }
}