using System;
using System.Collections.Generic;
using System.Text;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	internal class Spring : IItems
	{

		private AnimatedSprite sprite;
		private Point position = new Point(100, 100);


		public Spring(SharedTexture texture)
		{
			sprite = new AnimatedSprite(texture, 90, 0, 30, 24, 3, 0.25f);

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
