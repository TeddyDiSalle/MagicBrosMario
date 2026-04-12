using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Coin : IItems
	{
		private AnimatedSprite sprite;
		private Vector2 floatPosition;
		private bool isCollected = false;
		private bool rising = true;

		private float riseAmount = 0f;
		private const float RISE_TARGET = 48f;
		private const float SPEED = 200f;

		public Rectangle CollisionBox => new Rectangle((int)floatPosition.X, (int)floatPosition.Y, (int)(8 * sprite.Scale), (int)(16 * sprite.Scale));

		public Coin(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewAnimatedSprite(128, 95, 8, 16, 4, 0.05f);
			sprite.Scale = 2f;
			
			floatPosition = new Vector2(positionX, positionY);
			sprite.Position = floatPosition.ToPoint();

			MagicBrosMario.INSTANCE.items.Add(this);
		}

		public void Update(GameTime gameTime)
		{
			if (isCollected) return;

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
			sprite.Update(gameTime);

			float moveStep = SPEED * dt;

			if (rising)
			{
				floatPosition.Y -= moveStep;
				riseAmount += moveStep;

				if (riseAmount >= RISE_TARGET)
				{
					rising = false;
				}
			}
			else
			{
				floatPosition.Y += moveStep;
				riseAmount -= moveStep;

				if (riseAmount <= 0)
				{
					isCollected = true; 
				}
			}

			sprite.Position = floatPosition.ToPoint();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!isCollected)
			{
				sprite.Draw(spriteBatch);
			}
		}

		public bool getCollected() => isCollected;

		public void OnCollidePlayer(Player player, CollideDirection direction) { }
		public void OnCollideItem(IItems item, CollideDirection direction) { }
		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
		public void OnCollideBlock(IBlock block, CollideDirection direction) { }
	}
}