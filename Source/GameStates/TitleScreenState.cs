using MagicBrosMario.Source.Level;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MagicBrosMario.Source.GameStates
{
	public class TitleScreenState : IGameState
	{
		private Texture2D _titleTexture;
        private SpriteFont _font;

        public TitleScreenState()
        {

            _titleTexture = MagicBrosMario.INSTANCE.TitleScreen;
            _font = MagicBrosMario.INSTANCE.font;
            //_font = _game.Content.Load<SpriteFont>("font");

            //_game.Mario.Lives = 3;


		}

        public void Update(GameTime gameTime)
		{
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level.Level1());
            }
        }

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(_titleTexture, new Rectangle(0, 0,
                MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Width,
                MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Height), Color.White);

			string text = "Press Enter to Start\n\n  Press R to Reset";

            Vector2 textSize = _font.MeasureString(text);
            Vector2 position = new Vector2(
                (MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Width / 2) - (textSize.X / 2),
                (MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Height / 2) + 50
            );

            spriteBatch.DrawString(_font, text, position, Color.White);


        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}