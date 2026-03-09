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
    internal class QuestionBlock : IItems
	{

		private AnimatedSprite sprite;


        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public QuestionBlock(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{
			sprite = new AnimatedSprite(texture, 4, 4, 16, 16, 3, 0.1f);

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

        public void OnCollidePlayer(Player player, CollideDirection direction) { }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction) { }
		public void spitOutCoin()
		{

		}
		public void spitOutMushroom()
        {
         
        }
        
        public void spitOutFireFlower()
        {

        }

        public void spitOutOneUp()
        {

        }

		public bool getCollected()
		{
			return false;
		}

	}
}
