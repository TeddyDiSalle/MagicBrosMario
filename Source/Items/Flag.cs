using System;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	internal class Flag : IItems, ICollidable
	{
		private const float FallSpeed = 100f;
		private const float FallAmount = 250f;

		private Sprite.ISprite sprite;
		private Vector2 floatPosition;
		private bool isSliding = false;
		private float amountFell = 0f;

		public Rectangle CollisionBox => new Rectangle(
			(int)floatPosition.X, (int)floatPosition.Y,
			sprite.Size.X, sprite.Size.Y);

		public Flag(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewSprite(259, 44, 16, 16);
			sprite.Scale = 2f;
			floatPosition = new Vector2(positionX, positionY);
			sprite.Position = floatPosition.ToPoint();
			CollisionController.Instance.AddItem(this);
			MagicBrosMario.INSTANCE.items.Add(this);
		}

		public void Update(GameTime gameTime)
		{
			if (!isSliding && FlagPole.PlayerHit)
			{
				isSliding = true;
			}

			if (!isSliding) return;

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
			float step = FallSpeed * dt;

			if (amountFell < FallAmount)
			{
				float remaining = FallAmount - amountFell;
				float actualStep = Math.Min(step, remaining);

				floatPosition.Y += actualStep;
				amountFell += actualStep;
				sprite.Position = floatPosition.ToPoint();
			}
			else
			{
				isSliding = false;
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
		}

		public bool getCollected() => false;

		public void OnCollideBlock(IBlock block, CollideDirection direction) { }
		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
		public void OnCollideItem(IItems item, CollideDirection direction) { }
		public void OnCollidePlayer(Player player, CollideDirection direction) { }
	}
}