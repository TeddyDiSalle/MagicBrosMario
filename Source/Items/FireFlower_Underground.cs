using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Fireflower_Underground : IItems
	{
		private AnimatedSprite sprite;
		private Point position = new Point(100, 100);


		public Fireflower_Underground(SharedTexture texture)
		{
			sprite = new AnimatedSprite(texture, 124, 64, 16, 16, 4, 0.025f);

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
