using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace MagicBrosMario.Source.GameStates
{
    internal class TransitionState : IGameState
    {
        private ILevel _nextLevel;
        private Texture2D _transitionTexture;
        private float _timer = 1.5f;
		private SpriteFont _font;


		public TransitionState(ILevel nextLevel)
        {
            _nextLevel = nextLevel;
            _transitionTexture = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("TransitionScreen");

            _font = MagicBrosMario.INSTANCE.font;

			MagicBrosMario.INSTANCE.Mario.ResetPlayer();
            MagicBrosMario.INSTANCE.Mario.SetVisibility(false);


        }

        public void Update(GameTime gameTime)
        {
            if (_timer == 1.5f && MagicBrosMario.INSTANCE.Mario.Lives == 0) { SoundController.PlaySound(SoundType.GameOver, 1.0f); }
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            if (_timer <= 0)
            {
                if (MagicBrosMario.INSTANCE.Mario.Lives > 0) {
				    MagicBrosMario.INSTANCE.CurrentState = new PlayingState(_nextLevel);
			    }
                else
                {
                    MagicBrosMario.INSTANCE.CurrentState = new TitleScreenState();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_transitionTexture, new Rectangle(0, 0,
                 MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Width,
                 MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Height), Color.White);


			string text = "Lives: " + MagicBrosMario.INSTANCE.Mario.Lives;

			Vector2 textSize = _font.MeasureString(text);
			Vector2 position = new Vector2((MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Width / 2) - (textSize.X / 2), (MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Height / 2));

			spriteBatch.DrawString(MagicBrosMario.INSTANCE.font, text, position, Color.White);
		}

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}