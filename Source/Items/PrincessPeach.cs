using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.Items
{
	public class PrincessPeach : IItems
	{
		private Sprite.Sprite sprite;


        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(14 * sprite.Scale), (int)(24 * sprite.Scale));
            }
        }

        public PrincessPeach(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewSprite(245, 90, 14, 24);

			sprite.Position = new Point(positionX, positionY);
			sprite.Scale = 3f;

		}

		public void Update(GameTime gameTime)
		{
			sprite.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
        }

        public void OnCollidePlayer(Player player, CollideDirection direction) { }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction) { }

		public bool getCollected()
		{
			return false;
		}
	}
}
