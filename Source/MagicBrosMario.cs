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

    private SharedTexture sharedTexture;

    private Goomba goomba;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        sharedTexture = new SharedTexture();

        goomba = new Goomba(sharedTexture.NewAnimatedSprite(0, 0, 100, 100, 2, 0.1f), sharedTexture.NewSprite(0, 100, 100, 100), 300, 0, 800);
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D _texture = Content.Load<Texture2D>("characters");

        sharedTexture.BindTexture(_texture);

    }

    protected override void Update(GameTime gameTime)
    {

        goomba.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        goomba.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

}

