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
	internal class Cloud : IItems, ICollidable
	{

		private Sprite.ISprite sprite;
		private Point position;
		private float speed = 80f;
		private int direction = 1;
		private int xDifference = 0;
		private bool startMoving = false;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(48 * sprite.Scale), (int)(8 * sprite.Scale));
            }
        }

        public Cloud(SharedTexture texture, int positionX, int positionY)
		{

            sprite = texture.NewSprite(123, 38, 48, 8); 

			position = new Point(positionX, positionY);
			sprite.Scale = 2f;
            CollisionController.Instance.AddItem(this);

        }
		public void Update(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (startMoving)
			{
				xDifference = (int)(direction * speed * time);
				position.X += xDifference;
			}

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

        public void OnCollidePlayer(Player player, CollideDirection direction) {

			if (direction == CollideDirection.Top)
			{
				startMoving = true;
			}

		}

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction) { }


		public bool getCollected()
		{
			return false;
		}
	}
}
