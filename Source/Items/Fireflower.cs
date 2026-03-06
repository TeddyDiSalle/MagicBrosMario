using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Fireflower : IItems, ICollidable
	{
		private AnimatedSprite sprite;
        private bool isCollected = false;

        public Rectangle CollisionBox => throw new System.NotImplementedException();

        public Fireflower(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{
			sprite = new AnimatedSprite(texture, 4, 64, 16, 16, 4, 0.025f);

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

        public void OnCollidePlayer(Player player, CollideDirection direction)
        {
            isCollected = true;
        }

        public void OnCollideItem(IItems item, CollideDirection direction)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
        {
            throw new System.NotImplementedException();
        }

        public void OnCollideBlock(IBlock block, CollideDirection direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
