using MagicBrosMario.Source.Sprite;
﻿using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IController Controller;

    private MarioStates.Player Mario;
    private SharedTexture texture;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D _texture = Content.Load<Texture2D>("characters");
        Texture2D _fireTexture = Content.Load<Texture2D>("enemies");

        sharedTexture.BindTexture(_texture);
        
        fireSharedTexture.BindTexture(_fireTexture);
        SpriteFont fontDesc = Content.Load<SpriteFont>("font");
        Texture2D characterSheet = Content.Load<Texture2D>("characters");
        Texture2D MarioSheet = Content.Load<Texture2D>("MarioStarSheet");
        texture = new SharedTexture();
        texture.BindTexture(MarioSheet);

        Mario = new Player(texture);
        Debug.WriteLine(halfX +  " " + halfY);

    }

    protected override void Update(GameTime gameTime){
  
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        goomba.Draw(_spriteBatch);
        koopa.Draw(_spriteBatch);
        piranhaPlant.Draw(_spriteBatch);
        rotatingFireBar.Draw(_spriteBatch);
        bowser.Draw(_spriteBatch);

        //Code
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Mario.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

