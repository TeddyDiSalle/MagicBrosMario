using MagicBrosMario.Source.HUDAndScoring;
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
            MagicBrosMario.INSTANCE.Mario.SetVisibility(false);
            MagicBrosMario.INSTANCE.Mario.Lives = 3;
            HUD.Instance.ResetScoreAndCoins();
            MarioGameController.UnMute();
            MagicBrosMario.INSTANCE.finishedLevel1 = false;
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
            MagicBrosMario.INSTANCE.Mario.SetVisibility(false);
            spriteBatch.Draw(_titleTexture,
                new Rectangle(0, 0,
                    MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Width,
                    MagicBrosMario.INSTANCE.GraphicsDevice.Viewport.Height),
                new Rectangle(0, 0, _titleTexture.Width, _titleTexture.Height),
                Color.White, 0f, Vector2.Zero, SpriteEffects.None, 1.0f);

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