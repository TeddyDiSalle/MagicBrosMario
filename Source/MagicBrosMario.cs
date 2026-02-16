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

<<<<<<< HEAD
    private ISprite currentSprite;
    private StandstillSprite sprite1;
    private AnimatedSprite sprite2;
    private MovingSprite sprite3;
    private AnimatedMovingSprite sprite4;

    private ISprite[] Sprites;
    private IController Controller;

    private Font font;
=======
    private SharedTexture sharedTexture;
    private SharedTexture fireSharedTexture;

    private Goomba goomba;
    private Koopa koopa;
    private PiranhaPlant piranhaPlant;
    private RotatingFireBar rotatingFireBar;
    private Bowser bowser;
    private KeyboardState previousKeyboardState;
>>>>>>> WorkingOnEnemies

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
<<<<<<< HEAD
        
    }
=======
>>>>>>> WorkingOnEnemies

        sharedTexture = new SharedTexture();
        fireSharedTexture = new SharedTexture();
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

<<<<<<< HEAD
        Sprites = new ISprite[] {sprite1, sprite2, sprite3, sprite4};
        IController[] controllers =  {new KeyboardInfo(), new MouseInfo()};
        int[] cords = {Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2};
        Controller = new MarioGameController(this, Sprites, controllers, cords);
=======
        koopaWalkingRight.Scale = 3f;
        koopaWalkingLeft.Scale = 3f;
        koopaShellIdle.Scale = 3f;
        koopaShellMoving.Scale = 3f;
        koopaStomped.Scale = 3f;
        koopaShellDeath.Scale = 3f;
>>>>>>> WorkingOnEnemies

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
        // Piranha Plant
        var piranhaAliveSprite = sharedTexture.NewAnimatedSprite(125, 180, 16, 23, 2, 0.2f);
        piranhaAliveSprite.Scale = 3f;

        piranhaPlant = new PiranhaPlant(
            piranhaAliveSprite,
            250,  // X position (pipe location)
            200   // Y position (top of pipe - adjust based on your screen)
        );
        // Rotating Fire Bar
        rotatingFireBar = new RotatingFireBar(
            fireSharedTexture,
            364,    // Fireball X position in sprite sheet
            188,    // Fireball Y position in sprite sheet
            8,    // Fireball width
            8,    // Fireball height
            400,  // Center X position on screen
            300,  // Center Y position on screen
            6,    // Number of fireballs
            24    // Spacing between fireballs
        );
        // Bowser
        var bowserWalkingRight = sharedTexture.NewAnimatedSprite(255, 368, 35, 32, 4, 0.2f);
        var bowserWalkingLeft = sharedTexture.NewAnimatedSprite(116, 368, 35, 32, 2, 0.2f);  // Left animation

        bowserWalkingRight.Scale = 3f;
        bowserWalkingLeft.Scale = 3f;

        bowser = new Bowser(
            bowserWalkingRight,
            bowserWalkingLeft,
            fireSharedTexture,
            161,   // Fire moving right - X coordinate (2 frames)
            253,   // Fire moving right - Y coordinate
            101,    // Fire moving left - X coordinate (2 frames)
            253,    // Fire moving left - Y coordinate
            24,             // Fire width
            8,             // Fire height
            100,            // Bowser Y position
            50,             // Left bound
            750             // Right bound
        );
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D _texture = Content.Load<Texture2D>("characters");
        Texture2D _fireTexture = Content.Load<Texture2D>("enemies");

        sharedTexture.BindTexture(_texture);
        
        fireSharedTexture.BindTexture(_fireTexture);
    }

    protected override void Update(GameTime gameTime)
{
    KeyboardState currentKeyboardState = Keyboard.GetState();

    // Check if K was just pressed (not held)
    if (currentKeyboardState.IsKeyDown(Keys.K) && previousKeyboardState.IsKeyUp(Keys.K))
    {
<<<<<<< HEAD
        Controller.Update();

        currentSprite.Update(gameTime);

        base.Update(gameTime);
=======
        goomba.Kill();
        koopa.Kill();
        piranhaPlant.Kill();
        bowser.Kill();
>>>>>>> WorkingOnEnemies
    }

    // Check if M was just pressed (not held)
    if (currentKeyboardState.IsKeyDown(Keys.M) && previousKeyboardState.IsKeyUp(Keys.M))
    {
        koopa.KickShell(true);
    }

    goomba.Update(gameTime);
    koopa.Update(gameTime);
    piranhaPlant.Update(gameTime);
    rotatingFireBar.Update(gameTime);
    bowser.Update(gameTime);

    previousKeyboardState = currentKeyboardState;

    base.Update(gameTime);
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

        _spriteBatch.End();

        base.Draw(gameTime);
    }

<<<<<<< HEAD
    public void SetCurrentSprite(ISprite sprite)
    {
        currentSprite = sprite;
    }
}

=======
}
>>>>>>> WorkingOnEnemies
