using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

public class CoinBlock(AnimatedSprite sprite, Sprite.Sprite emptySprite) : BlockBase<CoinBlock>(sprite), ICollidable {
    public override void Update(GameTime gameTime) {
        throw new System.NotImplementedException();
    }

    public override void Draw(SpriteBatch spriteBatch) {
        throw new System.NotImplementedException();
    }

    /* ICollidable methods */

    public Rectangle CollisionBox => new(sprite.Position.X, sprite.Position.Y, sprite.Size.X, sprite.Size.Y);

    public void OnCollidePlayer(Player player, CollideDirection direction) {
        // spawn coins
        throw new System.NotImplementedException();
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