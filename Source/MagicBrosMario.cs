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

namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private MarioGameController Controller;

    public Player Mario;
    public SharedTexture MarioTexture { get; }
    public SharedTexture ItemTexture { get; }
    public SharedTexture EnemyTexture { get; }
    public SharedTexture FireTexture { get; }
    public SpriteFont font { get; private set; }
    private ILevel lvl;
    public static MagicBrosMario INSTANCE { get; private set; }
    public List<IItems> items { get; } = new();

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
        SoundController.LoadSounds();

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
        Level1(); // Intializes mario where level1 wants

        setController();
    }

    private void setController()	{
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
        Controller.Update(gameTime);
        lvl.Update(gameTime);
        Mario.Update(gameTime);

        int cameraX = Math.Max(Camera.Instance.Position.X, (int)Mario.Position.X - Camera.Instance.WindowSize.X / 2);
        Camera.Instance.Position = new Point(cameraX, 0);
        Camera.Instance.Update(gameTime);
        HUD.Instance.Update(gameTime);

        CollisionController.Instance.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        HUD.Instance.Draw(_spriteBatch);
        Camera.Instance.Draw(_spriteBatch);
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
    }

    private void resetLevel()
    {
        if(lvl != null) lvl.Clear();
        
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
}