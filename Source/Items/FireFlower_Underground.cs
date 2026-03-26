using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.Items
{
	public class Fireflower_Underground : IItems
	{
		private AnimatedSprite sprite;
        private bool isCollected = false;


        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public Fireflower_Underground(SharedTexture texture, int positionX, int positionY)
		{
            sprite = texture.NewAnimatedSprite(124, 64, 16, 16, 4, 0.025f);

			sprite.Position = new Point(positionX, positionY);
			sprite.Scale = 3f;

		}

		public void Update(GameTime gameTime)
		{
            if (!isCollected)
            {
                sprite.Update(gameTime);
            }
        }

		public void Draw(SpriteBatch spriteBatch)
		{
            if (!isCollected)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public void OnCollidePlayer(Player player, CollideDirection direction)
        {
            if (isCollected) return;

            isCollected = true;
        }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction) { }

        public bool getCollected()
        {
            return isCollected;
        }

    }
}
