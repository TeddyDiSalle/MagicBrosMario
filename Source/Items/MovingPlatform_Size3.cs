using System;
using System.Collections.Generic;
using System.Text;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	internal class MovingPlatform_Size3 : IItems
	{

		private Sprite.Sprite sprite;
		private Point position;
		private float speed = 80f;
		private int direction = -1;
		private int xLimit;
		private int yLimit;
		private int yDifference = 0;

		public MovingPlatform_Size3(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY, int d)
		{

			sprite = texture.NewSprite(63, 38, 48, 8);
			yLimit = screenHeight;
			xLimit = screenWidth;
			direction = d;

			position = new Point(positionX, positionY);
			sprite.Scale = 3f;

		}
		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

			yDifference = (int)(direction * speed * time);

			position.Y += yDifference;

			if (position.Y + sprite.Size.Y < 0) 
			{
				position.Y = yLimit;
			}
			else if (position.Y > yLimit) 
			{
				position.Y = -sprite.Size.Y;
			}

			sprite.Position = position;

			sprite.Update(gameTime);
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
		}

		public int getY()
		{
			return yDifference;
		}

	}
}
