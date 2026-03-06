using System;
using System.Collections.Generic;
using System.Text;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	internal class Cloud : IItems
	{

		private Sprite.Sprite sprite;
		private Point position;
		private float speed = 80f;
		private int direction = 1;
		private int xLimit;
		private int yLimit;
		private int xDifference = 0;

		public Cloud(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{

			sprite = new Sprite.Sprite(texture, 123, 38, 48, 8);
			yLimit = screenHeight;
			xLimit = screenWidth;

			position = new Point(positionX, positionY);
			sprite.Scale = 3f;

		}
		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

			xDifference = (int)(direction * speed * time);

			position.X += xDifference;


			sprite.Position = position;

			sprite.Update(gameTime);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
		}

		public int getX()
		{
			return xDifference;
		}

	}
}
