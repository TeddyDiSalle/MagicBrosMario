using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MagicBrosMario.Source.MarioStates;
using System;
using MagicBrosMario.Source.Collision;
namespace MagicBrosMario.Source.GameStates
{
	public class PlayingState : IGameState
	{
		public ILevel _level {get; private set;}

		public PlayingState(ILevel level)
		{
			_level = level;
		}
		
		public void Initialize()
		{
			Texture2D blockTex = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("blocks");
			Texture2D enemySheet = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("characters");
			Texture2D itemSheet = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("items");

			_level.Initialize(MagicBrosMario.INSTANCE.Content, blockTex, enemySheet, itemSheet);


            MarioGameController.UnMute();
            MagicBrosMario.INSTANCE.Mario.SetVisibility(true);
			MagicBrosMario.INSTANCE.Mario.EndPhase = Player.EndLevelPhase.None;
            MagicBrosMario.INSTANCE.Mario.SetVelocity(new Vector2(0,0));
            MagicBrosMario.INSTANCE.Mario.SetPositon(new Vector2(MagicBrosMario.INSTANCE.MarioStartPosition.X, MagicBrosMario.INSTANCE.MarioStartPosition.Y));
		}


		public void Update(GameTime gameTime)
		{
			MarioGameController.Update(gameTime);
			_level.Update(gameTime);
            

			
			int cameraX = Math.Max(Camera.Instance.Position.X, (int)MagicBrosMario.INSTANCE.Mario.Position.X - Camera.Instance.WindowSize.X / 2);
			int cameraY = Math.Min(0, (int)MagicBrosMario.INSTANCE.Mario.Position.Y);

            Camera.Instance.Position = new Point(cameraX, cameraY);
			Camera.Instance.Update(gameTime);
            MagicBrosMario.INSTANCE.Mario.Update(gameTime);
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
			CollisionController.Instance.RemoveAll();
        }
    }
}