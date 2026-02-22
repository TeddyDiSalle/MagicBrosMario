using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Coin : IItems
	{
		private AnimatedSprite sprite; 


		public Coin(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{
			sprite = new AnimatedSprite(texture, 128, 95, 8, 16, 4, 0.05f);

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
	}
}
