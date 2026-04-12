using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagicBrosMario.Source.GameStates
{
    internal class TransitionState : IGameState
    {
        private MagicBrosMario _game;
        private ILevel _nextLevel;
        private Texture2D _transitionTexture;
        private float _timer = 1.5f;

        public TransitionState(MagicBrosMario game, ILevel nextLevel)
        {
            _game = game;
            _nextLevel = nextLevel;
            _transitionTexture = _game.Content.Load<Texture2D>("TransitionScreen");
        }

        public void Update(GameTime gameTime)
        {
            _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer <= 0)
            {
                _game.SetState(new PlayingState(_game, _nextLevel));
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                _game.SetState(new TitleScreenState(_game));
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_transitionTexture, new Rectangle(0, 0,
                _game.GraphicsDevice.Viewport.Width,
                _game.GraphicsDevice.Viewport.Height), Color.White);
        }
    }
}