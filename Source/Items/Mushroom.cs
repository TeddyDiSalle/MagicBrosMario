// Made by Brian
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
    internal class Mushroom : IItems
	{

		private Sprite.Sprite sprite;
		private Point position;
		private float speed = 80f;
		private int direction = 1;
		private int xLimit;
		private int yLimit;
		private bool isCollected = false;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public Mushroom(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
		{

			sprite = texture.NewSprite(184, 34, 16, 16);
			yLimit = screenHeight;
			xLimit = screenWidth;

			position = new Point(positionX, positionY);
			sprite.Scale = 3f;

		}
		public void Update(GameTime gameTime)
		{
			if (!isCollected)
			{
                float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

                position.X += (int)(direction * speed * time);

                if (position.X <= 0 || position.X + sprite.Size.X >= xLimit)
                {
                    direction *= -1;
                }

                sprite.Position = position;

                sprite.Update(gameTime);
            }
		}
		public void Draw(SpriteBatch spriteBatch)
		{
            if (!isCollected)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public void OnCollidePlayer(Player player, CollideDirection direction) { }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction) { }


    }
}
  