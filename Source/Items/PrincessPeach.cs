using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class PrincessPeach : IItems
	{
		private Sprite.Sprite sprite; 


		public PrincessPeach(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
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
	}
}
