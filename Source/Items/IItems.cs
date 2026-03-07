using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
    public interface IItems:Collision.ICollidable
    {

		void Update(GameTime gameTime);
		void Draw(SpriteBatch spriteBatch);

		bool getCollected();


	}
}
