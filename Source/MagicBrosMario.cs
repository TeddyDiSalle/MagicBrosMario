using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Level;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.GameStates;

namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IGameState _currentState;
    private MarioGameController Controller;

    public Player Mario;
    public SharedTexture MarioTexture { get; }
    public SharedTexture ItemTexture { get; }
    public SharedTexture EnemyTexture { get; }
    public SharedTexture FireTexture { get; }
    public SpriteFont font { get; private set; }
    public ILevel lvl { get; set; }
    public static MagicBrosMario INSTANCE { get; private set; }

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        var _ = new Camera(_graphics);

        EnemyTexture = new SharedTexture();
        ItemTexture = new SharedTexture();
        MarioTexture = new SharedTexture();
        FireTexture = new SharedTexture();

        INSTANCE = this;
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        SoundEffectController.LoadSounds();

        Texture2D marioSheet = Content.Load<Texture2D>("MarioSpriteSheet");
        Texture2D blockTexture = Content.Load<Texture2D>("blocks");
        Texture2D itemSheet = Content.Load<Texture2D>("items");
        Texture2D enemySheet = Content.Load<Texture2D>("characters");
        Texture2D fireSheet = Content.Load<Texture2D>("enemies");

        EnemyTexture.BindTexture(enemySheet);
        ItemTexture.BindTexture(itemSheet);
        MarioTexture.BindTexture(marioSheet);
        FireTexture.BindTexture(fireSheet);
        font = Content.Load<SpriteFont>("Font");

        Mario = new Player(MarioTexture);

        setController();

        _currentState = new TitleScreenState(this);
    }

    public void SetState(IGameState newState)
    {
        _currentState = newState;
    }

    private void setController()
    {
        MarioGameController.Sprint2Controller data = new MarioGameController.Sprint2Controller
        {
            player = Mario,
            mouse = new MouseInfo(),
            keyb = new KeyboardInfo(),
            halfX = Camera.Instance.WindowSize.X / 2,
            halfY = Camera.Instance.WindowSize.Y / 2
        };
        Controller = new MarioGameController(this, ref data);
    }

    protected override void Update(GameTime gameTime)
    {
        _currentState.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear((_currentState is TransitionState) ? Color.Black : Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _currentState.Draw(_spriteBatch);
        Camera.Instance.Draw(_spriteBatch);
        HUD.Instance.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void Debug()
    {
        resetLevel();
        Texture2D blockTexture = Content.Load<Texture2D>("blocks");
        Texture2D itemSheet = Content.Load<Texture2D>("items");
        Texture2D enemySheet = Content.Load<Texture2D>("characters");

        lvl = new DebugRoom();
        lvl.Initialize(Content, blockTexture, enemySheet, itemSheet);
        resetMario();
    }

    public void Level1()
    {
        resetLevel();
        Texture2D blockTexture = Content.Load<Texture2D>("blocks");
        Texture2D itemSheet = Content.Load<Texture2D>("items");
        Texture2D enemySheet = Content.Load<Texture2D>("characters");

        lvl = new Level1();
        lvl.Initialize(Content, blockTexture, enemySheet, itemSheet);

        resetMario();
        resetHUD(1);
    }

    private void resetLevel()
    {
        if (lvl != null) lvl.Clear();

        Camera.Instance.Position = Point.Zero;
        Camera.Instance.Sprites.Clear();
    }

    private void resetMario()
    {
        CollisionController.Instance.RemovePlayer();
        Mario = new Player(MarioTexture);
        Mario.SetPositon(new Vector2(lvl.MarioStartPosX, lvl.MarioStartPosY));
        Mario.PowerUp(Power.FireFlower);
        setController();
    }

    private void resetHUD(int x)
    {
        HUD.Instance.SetLevel(x);
        HUD.Instance.SetTime(200);
    }
}