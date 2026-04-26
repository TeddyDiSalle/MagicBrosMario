using System;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	internal class FlagPole : IItems, ICollidable
	{
		public static bool PlayerHit = false;

		private Sprite.ISprite sprite;
		private Vector2 floatPosition;
		private bool playerHit = false;

		public Rectangle CollisionBox => new Rectangle(
			(int)floatPosition.X, (int)floatPosition.Y,
			sprite.Size.X, sprite.Size.Y);

		public FlagPole(SharedTexture texture, int positionX, int positionY)
		{
			sprite = texture.NewSprite(289, 0, 8, 152);
			sprite.Scale = 2f;
			floatPosition = new Vector2(positionX, positionY);
			sprite.Position = floatPosition.ToPoint();
			CollisionController.Instance.AddItem(this);
		}

		public void OnCollidePlayer(Player player, CollideDirection direction)
		{
			if (playerHit) return;
			playerHit = true;
			MagicBrosMario.INSTANCE.finishedLevel1 = true;
		}

		public void Update(GameTime gameTime) { }

		public void Draw(SpriteBatch spriteBatch)
		{
			sprite.Draw(spriteBatch);
		}

		public bool getCollected() => false;

		public void OnCollideBlock(IBlock block, CollideDirection direction) { }
		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
		public void OnCollideItem(IItems item, CollideDirection direction) { }
	}
}