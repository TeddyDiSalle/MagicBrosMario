using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicBrosMario.Source.Sprite;

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

			// Load level textures
			Texture2D blockTex = _game.Content.Load<Texture2D>("blocks");
			Texture2D enemySheet = _game.Content.Load<Texture2D>("characters");
			Texture2D itemSheet = _game.Content.Load<Texture2D>("items");

			// Initialize the level using the objects from Game1
			_level.Initialize(_game.Content, blockTex, enemySheet, itemSheet);

			// Set Mario's start position based on the specific Level class data
			_game.Mario.SetPositon(new Vector2(_level.MarioStartPosX, _level.MarioStartPosY));
		}

		public void Update(GameTime gameTime)
		{
			// Run all gameplay logic via the main game's controllers
			_game.Controller.Update(gameTime);
			_level.Update(gameTime);
			_game.Mario.Update(gameTime);

			// Camera Logic: Follow Mario
			int cameraX = Math.Max(Camera.Instance.Position.X, (int)_game.Mario.Position.X - Camera.Instance.WindowSize.X / 2);
			Camera.Instance.Position = new Point(cameraX, 0);
			Camera.Instance.Update(gameTime);

			// Run collision detection
			Collision.CollisionController.Instance.Update(gameTime);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			// Draw everything through the Camera system
			Camera.Instance.Draw(spriteBatch);
		}
	}
}