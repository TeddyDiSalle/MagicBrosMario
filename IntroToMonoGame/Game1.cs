using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;


namespace IntroToMonoGame;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private KeyboardInfo keyboardInput;
    private MouseInfo mouseInput;

    private ISprite currentSprite;
    private StandstillSprite sprite1;
    private AnimatedSprite sprite2;
    private MovingSprite sprite3;
    private AnimatedMovingSprite sprite4;

    private Font font;

    private int halfX;
    private int halfY;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        halfX = Window.ClientBounds.Width / 2;
        halfY = Window.ClientBounds.Height / 2;
    }

    protected override void Initialize()
    {

        keyboardInput = new KeyboardInfo();
        mouseInput = new MouseInfo();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D _texture = Content.Load<Texture2D>("characters");
        SpriteFont fontDesc = Content.Load<SpriteFont>("font");

        Vector2 center = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
        sprite1 = new StandstillSprite(_texture, new Rectangle(276, 43, 13, 17), center);
        sprite2 = new AnimatedSprite(_texture, new Rectangle(291, 44, 13, 16),
            new Rectangle(306, 43, 12, 17), new Rectangle(320, 43, 16, 17), center);
        sprite3 = new MovingSprite(_texture, new Rectangle(485, 45, 15, 15), center, Window.ClientBounds.Height);
        sprite4 = new AnimatedMovingSprite(_texture, new Rectangle(291, 44, 13, 16),
            new Rectangle(306, 43, 12, 17), new Rectangle(320, 43, 16, 17), center, Window.ClientBounds.Width);

        font = new Font(fontDesc, new Vector2(200, 400));

        currentSprite = sprite1;
    }

    protected override void Update(GameTime gameTime)
    {
        InputCheck();

        currentSprite.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        currentSprite.Draw(_spriteBatch);

        font.Draw(_spriteBatch);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void InputCheck()
    {
        keyboardInput.Update();
        mouseInput.Update();
        Point position = mouseInput.Position;


        if (keyboardInput.IsKeyDown(Keys.D0) || mouseInput.IsButtonDown(MouseButton.Right))
        {
            Exit();
        }
        else if (keyboardInput.IsKeyDown(Keys.D1) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X < halfX && position.Y < halfY))
        {
            currentSprite = sprite1;
        }
        else if (keyboardInput.IsKeyDown(Keys.D2) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X > halfX && position.Y < halfY))
        {
            currentSprite = sprite2;
        }
        else if (keyboardInput.IsKeyDown(Keys.D3) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X < halfX && position.Y > halfY))
        {
            currentSprite = sprite3;

        }
        else if (keyboardInput.IsKeyDown(Keys.D4) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X > halfX && position.Y > halfY))
        {
            currentSprite = sprite4;
        }
    }
}

