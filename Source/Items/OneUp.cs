using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.Items
{
	internal class OneUp : IItems, ICollidable
	{
		private const float GRAVITY_SPEED = 250f;
		private const float RISE_SPEED = 30f;
		private const float X_SPEED = 120f;

		private Sprite.ISprite sprite;
		private Vector2 floatPosition;

		private int xDirection = 1;
		private bool isCollected = false;
		private bool hasRisen = false;
		private bool isOnBlock = false;
		private float riseAmount = 0f;
		private float riseTarget = 32f;

		public Rectangle CollisionBox
		{
			get
			{
				int width = (int)(sprite.Size.X - 4);
				int height = sprite.Size.Y + 1;
				return new Rectangle((int)floatPosition.X + 2, (int)floatPosition.Y, width, height);
			}
		}

		public Point position { get => sprite.Position; private set => sprite.Position = value; }

		public OneUp(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewSprite(214, 34, 16, 16);
			sprite.Scale = 2f;

			floatPosition = new Vector2(positionX, positionY);
			sprite.Position = floatPosition.ToPoint();

			CollisionController.Instance.AddItem(this);
		}

		public void Update(GameTime gameTime)
		{
			if (isCollected) return;

			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

			if (!hasRisen)
			{
				Rise(dt);
			}
			else
			{
				if (!isOnBlock)
				{
					floatPosition.Y += (GRAVITY_SPEED * dt);
				}
				floatPosition.X += (xDirection * X_SPEED * dt);
			}
			sprite.Position = floatPosition.ToPoint();

			isOnBlock = false;
		}

		private void Rise(float dt)
		{
			float step = RISE_SPEED * dt;
			floatPosition.Y -= step;
			riseAmount += step;

			if (riseAmount >= riseTarget)
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

		public void OnCollideBlock(IBlock block, CollideDirection direction)
		{
			if (!hasRisen) return;

			Rectangle blockBox = block.CollisionBox;

			if (direction == CollideDirection.Down)
			{
				isOnBlock = true;
				floatPosition.Y = blockBox.Top - sprite.Size.Y;
			}
			else if (direction == CollideDirection.Left)
			{
				xDirection = 1;
				floatPosition.X = blockBox.Right;
			}
			else if (direction == CollideDirection.Right)
			{
				xDirection = -1;
				floatPosition.X = blockBox.Left - sprite.Size.X;
			}
			else if (direction == CollideDirection.Top)
			{
				floatPosition.Y = blockBox.Bottom;
			}

			sprite.Position = floatPosition.ToPoint();
		}

		public void OnCollidePlayer(Player player, CollideDirection direction)
		{
			if (isCollected) return;
			isCollected = true;
			CollisionController.Instance.RemoveItem(this);
			sprite.Drop();
		}

		public void OnCollideItem(IItems item, CollideDirection direction) { }
		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
		public bool getCollected() => isCollected;
	}
}