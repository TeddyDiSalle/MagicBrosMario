using MagicBrosMario.Source.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MagicBrosMario.Source.GameStates
{
	public class TitleScreenState : IGameState
	{
		private MagicBrosMario _game;
		private Texture2D _titleTexture;

		public TitleScreenState(MagicBrosMario game)
		{
			_game = game;
			// Ensure "MarioTitleScreen" is the name of your file in Content
			_titleTexture = _game.Content.Load<Texture2D>("MarioTitleScreen");
		}

		public void Update(GameTime gameTime)
		{
			// When Enter is pressed, skip transitions and go straight to Level 1
			if (Keyboard.GetState().IsKeyDown(Keys.Enter))
			{
				_game.SetState(new PlayingState(_game, new Level1()));
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw the title image to fill the screen
			spriteBatch.Draw(_titleTexture, new Rectangle(0, 0,
				_game.GraphicsDevice.Viewport.Width,
				_game.GraphicsDevice.Viewport.Height), Color.White);
		}
	}
}