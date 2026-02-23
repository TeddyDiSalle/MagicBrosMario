using System;
using System.Collections.Generic;
using System.Text;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	internal class MovingPlatform_Size1 : IItems
	{

		private Sprite.Sprite sprite;
		private Point position;
		private float speed = 80f;
		private int direction = -1;
		private int xLimit;
		private int yLimit;

		public MovingPlatform_Size1(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{

			sprite = new Sprite.Sprite(texture, 0, 38, 24, 8);
			yLimit = screenHeight;
			xLimit = screenWidth;

			position = new Point(positionX, positionY);
			sprite.Scale = 3f;

		}
		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

			position.Y += (int)(direction * speed * time);

			if (position.Y <= 0 || position.Y + sprite.Size.Y >= yLimit)
			{
				direction *= -1;
			}

			sprite.Position = position;

			sprite.Update(gameTime);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
		}

	}
}
