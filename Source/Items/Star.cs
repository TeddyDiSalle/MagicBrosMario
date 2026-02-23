using System.Runtime.CompilerServices;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Star : IItems
	{
		private AnimatedSprite sprite;
		private Vector2 position;
		private float speed = 80f;
		private int xDirection = 1;
		private int yDirection = -1;
		private int xLimit;
		private float yBottom;
		private float yTop;


		public Star(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{
			sprite = new AnimatedSprite(texture, 5, 94, 14, 16, 4, 0.03f);
			yBottom = positionY + 30f;
			yTop = positionY - 30f;
			xLimit = screenWidth;

			position = new Vector2(positionX, positionY);
			sprite.Position = position.ToPoint();
			sprite.Scale = 3f;

		}

		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

			position.X += (int)(xDirection * speed * time);
			position.Y += (int)(yDirection * speed * time);

			if (position.X <= 0)
			{
				position.X = 0;
				xDirection = 1;
			}
			else if (position.X + sprite.Size.X >= xLimit)
			{
				position.X = xLimit - sprite.Size.X;
				xDirection = -1;
			}

			if (position.Y <= yTop)
			{
				position.Y = yTop;
				yDirection = 1;
			}
			else if (position.Y >= yBottom)
			{
				position.Y = yBottom;
				yDirection = -1;
			}


			sprite.Position = position.ToPoint();
			sprite.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
		}
	}
}
