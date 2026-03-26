// Made by Brian
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace MagicBrosMario.Source.Items
{
	internal class OneUp : IItems
	{
		private const float GRAVITY_SPEED = 250f;


		private Sprite.Sprite sprite;
		private float xSpeed = 120f;
		private int xDirection = 1;
		private int xLimit;
		private int yLimit;
		private bool isCollected = false;
		private bool hasRisen = false;
		private bool isOnBlock = false;
		private float riseAmount = 0f;
		private float riseTarget = 16f;

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle(sprite.Position.X, sprite.Position.Y, sprite.Size.X, sprite.Size.Y);
			}
		}

		public Point position { get => sprite.Position; private set => sprite.Position = value; }

		public OneUp(SharedTexture texture, int positionX, int positionY)
		{

			sprite = texture.NewSprite(214, 34, 16, 16); 

			if (positionX <= 0)
			{
				positionX = 1;
			}
			if (positionY <= 0)
			{
				positionY = 1;
			}

			position = new Point(positionX, positionY);
			sprite.Scale = 2f;
			CollisionController.Instance.AddItem(this);

		}
		public void Update(GameTime gameTime)
		{
			if (!isCollected)
			{
				isOnBlock = false;
				move(gameTime);
			}
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			if (!isCollected)
			{
				sprite.Draw(spriteBatch);
			}
		}

		private void move(GameTime gameTime)
		{
			float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (!isOnBlock)
			{
				position = new Point(position.X, position.Y + (int)(GRAVITY_SPEED * time));
			}
			position = new Point(position.X + (int)(xDirection * xSpeed * time), position.Y);

		}

		public void OnCollidePlayer(Player player, CollideDirection direction)
		{

			isCollected = true;
			CollisionController.Instance.RemoveItem(this);
			sprite.Drop();
		}


		public void OnCollideItem(IItems item, CollideDirection direction) { }

		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

		public void OnCollideBlock(IBlock block, CollideDirection direction)
		{
			Debug.WriteLine(direction);
			if (direction == CollideDirection.Down)
			{
				isOnBlock = true;
				position = new Point(position.X, block.CollisionBox.Top - CollisionBox.Height - 3);
			}
			else if (direction == CollideDirection.Left)
			{
				xDirection = 1;
				position = new Point(block.CollisionBox.Right + 1, position.Y);
			}
			else if (direction == CollideDirection.Right)
			{
				xDirection = -1;
				position = new Point(block.CollisionBox.Left - sprite.Size.X - 1, position.Y);
			}

		}

		public bool getCollected()
		{
			return isCollected;
		}

	}
}
