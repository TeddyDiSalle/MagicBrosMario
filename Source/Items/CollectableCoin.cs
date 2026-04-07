using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class CollectableCoin : IItems
	{
		private AnimatedSprite sprite;
		private bool isCollected = false;

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
			}
		}

		public CollectableCoin(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewAnimatedSprite(192, 64, 10, 14, 4, 0.05f);

			sprite.Position = new Point(positionX, positionY);
			sprite.Scale = 2f;
		}

		public void Update(GameTime gameTime)
		{
			if (!isCollected)
			{
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

		public void OnCollidePlayer(Player player, CollideDirection direction)
		{
			isCollected = true;
			HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.CoinCollected,
			EventPosition = sprite.Position,
			Data = this});

			CollisionController.Instance.RemoveItem(this);
			sprite.Drop();
			if (isCollected) return;
		}

		public void OnCollideItem(IItems item, CollideDirection direction) { }

		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

		public void OnCollideBlock(IBlock block, CollideDirection direction) { }

		public bool getCollected()
		{
			return isCollected;
		}

	}
}