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
        private SpriteFont _font;

        public TitleScreenState(MagicBrosMario game)
		{
			_game = game;
			_titleTexture = _game.Content.Load<Texture2D>("MarioTitleScreen");
            _font = _game.Content.Load<SpriteFont>("font");
        }

		public void Update(GameTime gameTime)
		{
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _game.SetState(new TransitionState(_game, new Level.Level1()));
            }
        }

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_titleTexture, new Rectangle(0, 0,
				_game.GraphicsDevice.Viewport.Width,
				_game.GraphicsDevice.Viewport.Height), Color.White);

			string text = "Press Enter to Start\n\n  Press R to Reset";

            Vector2 textSize = _font.MeasureString(text);
            Vector2 position = new Vector2(
                (_game.GraphicsDevice.Viewport.Width / 2) - (textSize.X / 2),
                (_game.GraphicsDevice.Viewport.Height / 2) + 50
            );

            spriteBatch.DrawString(_font, text, position, Color.White);


        }
	}
}