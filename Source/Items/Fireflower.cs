using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Fireflower : IItems, ICollidable
	{
		private const float RISE_SPEED = 30f;
		private const float RISE_TARGET = 30f;
		private AnimatedSprite sprite;
		private Vector2 floatPosition;
		private bool isCollected = false;
		private bool hasRisen = false;
		private float riseAmount = 0f;

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)floatPosition.X, (int)floatPosition.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
			}
		}

		public Fireflower(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewAnimatedSprite(4, 64, 16, 16, 4, 0.025f);
			sprite.Scale = 2f;
			floatPosition = new Vector2(positionX, positionY);
			sprite.Position = floatPosition.ToPoint();
			sprite.Midground();
			CollisionController.Instance.AddItem(this);
		}

		public void Update(GameTime gameTime)
		{
			if (isCollected) return;
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
			sprite.Update(gameTime);
			if (!hasRisen)
			{
				Rise(dt);
			}
			sprite.Position = floatPosition.ToPoint();
		}

		private void Rise(float dt)
		{
			float step = RISE_SPEED * dt;
			floatPosition.Y -= step;
			riseAmount += step;
			if (riseAmount >= RISE_TARGET)
			{
				hasRisen = true;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (!isCollected)
			{
				sprite.Draw(spriteBatch);
			}
		}

		public void OnCollidePlayer(Player player, CollideDirection direction)
		{
			isCollected = true;
			CollisionController.Instance.RemoveItem(this);
			sprite.Drop();
		}

		public void OnCollideItem(IItems item, CollideDirection direction) { }
		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
		public void OnCollideBlock(IBlock block, CollideDirection direction) { }
		public bool getCollected() => isCollected;
	}
}