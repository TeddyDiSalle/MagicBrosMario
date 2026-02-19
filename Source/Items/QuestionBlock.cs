using System;
using System.Collections.Generic;
using System.Text;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
    internal class QuestionBlock : IItems
	{

		private AnimatedSprite sprite;
		private Point position = new Point(100, 100);


		public QuestionBlock(SharedTexture texture)
		{
			sprite = new AnimatedSprite(texture, 4, 4, 16, 16, 3, 0.1f);

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
