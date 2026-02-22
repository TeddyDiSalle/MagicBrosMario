using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Coin : IItems
	{
		private AnimatedSprite sprite;
		private Point position = new Point(100, 100);


		public Coin(SharedTexture texture)
		{
			sprite = new AnimatedSprite(texture, 128, 95, 8, 16, 4, 0.05f);

			sprite.Position = position;
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
