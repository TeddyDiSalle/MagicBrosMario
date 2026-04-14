using System;
using System.Collections.Generic;
using System.Text;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace MagicBrosMario.Source.GameStates
{
    internal class TransitionState : IGameState
    {
        private MagicBrosMario _game;
        private ILevel _nextLevel;
        private Texture2D _transitionTexture;
        private float _timer = 1.5f;
		private SpriteFont _font;


		public TransitionState(MagicBrosMario game, ILevel nextLevel)
        {
            _game = game;
            _nextLevel = nextLevel;
            _transitionTexture = _game.Content.Load<Texture2D>("TransitionScreen");

			_font = _game.Content.Load<SpriteFont>("Font");

		}

        public void Update(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                _game.CurrentState = new PlayingState(_game, _nextLevel);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _game.CurrentState = new TitleScreenState(_game);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_transitionTexture, new Rectangle(0, 0,
                _game.GraphicsDevice.Viewport.Width,
                _game.GraphicsDevice.Viewport.Height), Color.White);


			string text = "Lives: " + _game.Mario.Lives;

			Vector2 textSize = _font.MeasureString(text);
			Vector2 position = new Vector2((_game.GraphicsDevice.Viewport.Width / 2) - (textSize.X / 2), (_game.GraphicsDevice.Viewport.Height / 2));

			spriteBatch.DrawString(_game.font, text, position, Color.White);
		}

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}