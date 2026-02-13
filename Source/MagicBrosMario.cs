using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;


namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private KeyboardInfo keyboardInput;
    private MouseInfo mouseInput;

    private MarioStates.Player Mario;
    private SharedTexture texture;
    private int inputCount = 0;

    private int halfX;
    private int halfY;

    public MagicBrosMario()
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

        SpriteFont fontDesc = Content.Load<SpriteFont>("font");
        Texture2D characterSheet = Content.Load<Texture2D>("characters");
        texture = new SharedTexture();
        texture.BindTexture(characterSheet);

        Mario = new Player(texture);
        Debug.WriteLine(halfX +  " " + halfY);

    }

    protected override void Update(GameTime gameTime)
    {
        //Code
        if (inputCount < 2)
        {
            inputCount++;
            InputCheck(gameTime);
        }
        Mario.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        //Code
        Mario.Draw(_spriteBatch);

        base.Draw(gameTime);
    }

    private void InputCheck(GameTime gameTime)
    {
        keyboardInput.Update();
        mouseInput.Update();
        Point position = mouseInput.Position;


        if (keyboardInput.IsKeyDown(Keys.D0))
        {
            Exit();
        }
        else if (keyboardInput.IsKeyDown(Keys.W))
        {
            Mario.Jump(gameTime);
        }
        else if (keyboardInput.IsKeyDown(Keys.A))
        {
            Mario.Left(gameTime);
        }
        else if (keyboardInput.IsKeyDown(Keys.D))
        {
            Mario.Right(gameTime);
        }
        else if (keyboardInput.IsKeyDown(Keys.S))
        {
            Mario.Crouch(gameTime);
        }
        else if (keyboardInput.IsKeyDown(Keys.X))
        {
            Mario.TakeDamage();
        }else if (keyboardInput.IsKeyDown(Keys.R))
        {
            Mario.ChangeState(new RightSmallMarioIdleState(Mario, texture));
        }
        else
        {
            Mario.Idle();
        }
        inputCount--;
    }
}

