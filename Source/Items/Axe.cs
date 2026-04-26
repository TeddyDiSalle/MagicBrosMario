using System;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
	public class Axe : IItems, ICollidable
	{ 
		private AnimatedSprite sprite;
		private Vector2 floatPosition;
		private bool isCollected = false;
		private int groupNum;

		public Rectangle CollisionBox
		{
			get
			{
				return new Rectangle((int)floatPosition.X, (int)floatPosition.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
			}
		}

		public Axe(SharedTexture texture, int positionX, int positionY, int group)
		{
			sprite = texture.NewAnimatedSprite(5, 117, 16, 16, 4, 0.025f);
			sprite.Scale = 2f;
			floatPosition = new Vector2(positionX, positionY);
			sprite.Position = floatPosition.ToPoint();
			CollisionController.Instance.AddItem(this);
			groupNum = group;
		}

		public void Update(GameTime gameTime)
		{
			if (isCollected) return;
			float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
			sprite.Update(gameTime); 
			sprite.Position = floatPosition.ToPoint();
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
			if (isCollected) return; 
			isCollected = true;
			//Console.WriteLine($"Axe collected! Group: {groupNum}");
			BridgeBlock.CollapseBridge(groupNum);
			CollisionController.Instance.RemoveItem(this);
			sprite.Drop();
		}

		public void OnCollideItem(IItems item, CollideDirection direction) { }
		public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
		public void OnCollideBlock(IBlock block, CollideDirection direction) { }
		public bool getCollected() => isCollected;
	}
}