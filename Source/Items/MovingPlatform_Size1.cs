using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicBrosMario.Source.Items
{
	internal class MovingPlatform_Size1 : IItems
	{											  

		private Sprite.Sprite sprite;
		private Point position;
		private float speed = 80f;
		private int direction;
		private int xLimit;
		private int yLimit;
		private int yDifference = 0;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(24 * sprite.Scale), (int)(8 * sprite.Scale));
            }
        }

        public MovingPlatform_Size1(SharedTexture texture, int screenHeight, int positionX, int positionY, int d)
		{

			sprite = texture.NewSprite(0, 38, 24, 8);
			yLimit = screenHeight; 
			direction = d;

			position = new Point(positionX, positionY);
			sprite.Scale = 2f;

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

        public void OnCollidePlayer(Player player, CollideDirection direction) { }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction) { }


		public bool getCollected()
		{
			return false;
		}
	}
}
