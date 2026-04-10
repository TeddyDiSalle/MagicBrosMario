using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace MagicBrosMario.Source.GameStates
{
	public class PlayingState : IGameState
	{
		private MagicBrosMario _game;
		private ILevel _level;

		public PlayingState(MagicBrosMario game, ILevel level)
		{
			_game = game;
			_level = level;

			Texture2D blockTex = _game.Content.Load<Texture2D>("blocks");
			Texture2D enemySheet = _game.Content.Load<Texture2D>("characters");
			Texture2D itemSheet = _game.Content.Load<Texture2D>("items");

			_level.Initialize(_game.Content, blockTex, enemySheet, itemSheet);

			_game.Mario.SetPositon(new Vector2(_level.MarioStartPosX, _level.MarioStartPosY));
		}

		public void Update(GameTime gameTime)
		{
			_game.Controller.Update(gameTime);
			_level.Update(gameTime);
			_game.Mario.Update(gameTime);

			int cameraX = Math.Max(Camera.Instance.Position.X, (int)_game.Mario.Position.X - Camera.Instance.WindowSize.X / 2);
			Camera.Instance.Position = new Point(cameraX, 0);
			Camera.Instance.Update(gameTime);

			Collision.CollisionController.Instance.Update(gameTime);


        }

		public void Draw(SpriteBatch spriteBatch)
		{
			Camera.Instance.Draw(spriteBatch);
		}
	}
}