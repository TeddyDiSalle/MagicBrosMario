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
        Texture2D MarioSheet = Content.Load<Texture2D>("MarioStarSheet");
        texture = new SharedTexture();
        texture.BindTexture(MarioSheet);

        Mario = new Player(texture);
        Debug.WriteLine(halfX +  " " + halfY);

    }

    protected override void Update(GameTime gameTime)
    {
        //Code

            InputCheck(gameTime);

        Mario.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        //Code
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Mario.Draw(_spriteBatch);
        _spriteBatch.End();

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
        }
        else if (keyboardInput.IsKeyDown(Keys.F))
        {
            Mario.Attack();
        }
        else if (keyboardInput.IsKeyDown(Keys.D1))
        {
            Mario.ChangeState(new SmallMarioIdleState(Mario, texture, 0.15, 3));
        }
        else if (keyboardInput.IsKeyDown(Keys.D2))
        {
            Mario.ChangeState(new BigMarioIdleState(Mario, texture, 0.15, 3));
        }
        else if (keyboardInput.IsKeyDown(Keys.D3))
        {
            Mario.PowerUp(Power.FireFlower);
        }
        else if (keyboardInput.IsKeyDown(Keys.D4))
        {
            Mario.PowerUp(Power.Star);
        }
        else
        {
            Mario.ReleaseCrouch();
            Mario.Idle();
        }
       
    }
}

