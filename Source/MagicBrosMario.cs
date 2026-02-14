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
    private Koopa koopa;
    private KeyboardState previousKeyboardState;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        sharedTexture = new SharedTexture();
        // Goomba
        var aliveSprite = sharedTexture.NewAnimatedSprite(295, 187, 18, 18, 2, 0.2f);
        var deadSprite = sharedTexture.NewSprite(276, 187, 18, 18);
        aliveSprite.Scale = 3f;  
        deadSprite.Scale = 3f;
        goomba = new Goomba(
            aliveSprite,
            deadSprite,
            400,    // Y position (moved down from 300)
            50,     // Left bound (added margin)
            750     // Right bound (window width minus margin)
        );
        // Koopa
        var koopaWalkingRight = sharedTexture.NewAnimatedSprite(296, 206, 18, 25, 2, 0.2f);
        var koopaWalkingLeft = sharedTexture.NewAnimatedSprite(182, 206, 18, 25, 2, 0.2f);
        var koopaShellIdle = sharedTexture.NewSprite(144, 216, 16, 14);
        var koopaShellMoving = sharedTexture.NewSprite(144, 216, 16, 14);//same as shellIdle for now but it moves
        var koopaStomped = sharedTexture.NewSprite(163, 215, 16, 15);
        var koopaShellDeath = sharedTexture.NewSprite(334, 215, 16, 15);

        koopaWalkingRight.Scale = 3f;
        koopaWalkingLeft.Scale = 3f;
        koopaShellIdle.Scale = 3f;
        koopaShellMoving.Scale = 3f;
        koopaStomped.Scale = 3f;
        koopaShellDeath.Scale = 3f;

        koopa = new Koopa(
            koopaWalkingRight,
            koopaWalkingLeft,
            koopaShellIdle,
            koopaShellMoving,
            koopaStomped,
            koopaShellDeath,
            200,
            50,
            750
        );
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D _texture = Content.Load<Texture2D>("characters");

        sharedTexture.BindTexture(_texture);
    }

    protected override void Update(GameTime gameTime)
{
    KeyboardState currentKeyboardState = Keyboard.GetState();

    // Check if K was just pressed (not held)
    if (currentKeyboardState.IsKeyDown(Keys.K) && previousKeyboardState.IsKeyUp(Keys.K))
    {
        goomba.Kill();
        koopa.Kill();
    }

    // Check if M was just pressed (not held)
    if (currentKeyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
    {
        koopa.KickShell(true);
    }

    goomba.Update(gameTime);
    koopa.Update(gameTime);

    previousKeyboardState = currentKeyboardState;

    base.Update(gameTime);
}

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        goomba.Draw(_spriteBatch);
        koopa.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }

}