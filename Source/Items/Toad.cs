using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Toad : IItems
	{
		private Sprite.Sprite sprite; 


		public Toad(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{
			sprite = new Sprite.Sprite(texture, 244, 0, 16, 24);

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
