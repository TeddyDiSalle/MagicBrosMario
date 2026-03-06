using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Sprite;

/// <summary>
/// camera class for drawing all the sprites and updating position based on screen
/// this class should be intialized right after graphics is created
/// </summary>
public class Camera {
    private readonly GraphicsDeviceManager graphics;

    public Camera(GraphicsDeviceManager graphics) {
        this.graphics = graphics;
        Instance = this;
    }

    public static Camera Instance {
        get {
            if (field == null) {
                throw new
                    Exception("camera has not been initialized, create camera should be create right after graphics");
            }

            return field;
        }
        private set;
    }

    public List<ISprite> Sprites { get; } = [];

    public Point WindowSize {
        get => new(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

        set {
            graphics.PreferredBackBufferWidth = value.X;
            graphics.PreferredBackBufferHeight = value.Y;
            foreach (var sprite in Sprites) {
                sprite.UpdateDestRect();
            }
            graphics.ApplyChanges();
        }
    }

    /// <summary>
    /// top-left corner of the screen.
    /// when Position.X increases, the screen moves down, blocks move up
    /// when Position.Y increases, the screen moves right, blocks move left
    /// </summary>
    public Point Position { get; set; } = Point.Zero;

    public bool ShouldDraw(Rectangle destRect) {
        return destRect.X < WindowSize.X &&
               destRect.Y < WindowSize.Y &&
               destRect.X + destRect.Width > 0 &&
               destRect.Y + destRect.Height > 0;
    }

    public void Update(GameTime gameTime) {
        foreach (var sprite in Sprites) {
            sprite.Update(gameTime);
        }
    }
    
    public void Draw(SpriteBatch spriteBatch) {
        foreach (var sprite in Sprites) {
            sprite.Draw(spriteBatch);
        }
    }
}