using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.GameStates
{
	public interface IGameState
		{
			void Update(GameTime gameTime);
			void Draw(SpriteBatch spriteBatch);
			void Clear();
		}
}
