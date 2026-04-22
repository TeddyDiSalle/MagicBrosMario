using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MagicBrosMario.Source.MarioStates;
using System;
namespace MagicBrosMario.Source.GameStates
{
	public class PlayingState : IGameState
	{
		public ILevel _level {get; private set;}

		public PlayingState(ILevel level)
		{
			_level = level;

			Texture2D blockTex = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("blocks");
			Texture2D enemySheet = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("characters");
			Texture2D itemSheet = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("items");

			_level.Initialize(MagicBrosMario.INSTANCE.Content, blockTex, enemySheet, itemSheet);
			 

            //MagicBrosMario.INSTANCE.Mario = new Player(MagicBrosMario.INSTANCE.MarioTexture); // This will have to change so we can keep track of lives and the power of mario between levels
																							  // But if you die right now, there is no way to come back to live

				
            MagicBrosMario.INSTANCE.Mario.SetPositon(new Vector2(_level.MarioStartPosX, _level.MarioStartPosY));
		}

		public void Update(GameTime gameTime)
		{
			MarioGameController.Update(gameTime);
			_level.Update(gameTime);
            MagicBrosMario.INSTANCE.Mario.Update(gameTime);

			
			int cameraX = Math.Max(Camera.Instance.Position.X, (int)MagicBrosMario.INSTANCE.Mario.Position.X - Camera.Instance.WindowSize.X / 2);
			Camera.Instance.Position = new Point(cameraX, 0);
			Camera.Instance.Update(gameTime);
            HUD.Instance.Update(gameTime);
			Sound.SoundController.Update(gameTime);

            Collision.CollisionController.Instance.Update(gameTime);


		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Camera.Instance.Draw(spriteBatch);
			HUD.Instance.Draw(spriteBatch);
		}

        public void Clear()
        {
            _level.Clear();
			//MagicBrosMario.INSTANCE.Mario.Lives--;
			Camera.Instance.Sprites.Clear();
        }
    }
}