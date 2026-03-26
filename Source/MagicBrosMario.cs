using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Level;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private MarioGameController Controller;

    public Player Mario;
    private SharedTexture MarioTexture;
    private SharedTexture ItemTexture;
    private SharedTexture EnemyTexture;
    private SharedTexture FireTexture;
    private float[] MarioStartPos = { 100, 300 };
    private ILevel lvl;
    private SpriteFont _font;

    private List<IItems> ItemsList;
    private List<IEnemy> Enemies;
    private int ScreenWidth;
    private int ScreenHeight;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ScreenWidth = Window.ClientBounds.Width;
        ScreenHeight = Window.ClientBounds.Height;
        new Camera(_graphics);
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("Font");

        Texture2D marioSheet = Content.Load<Texture2D>("MarioSpriteSheet");
        Texture2D blockTexture = Content.Load<Texture2D>("blocks");
        Texture2D itemSheet = Content.Load<Texture2D>("items");
        Texture2D enemySheet = Content.Load<Texture2D>("characters");
        Texture2D fireSheet = Content.Load<Texture2D>("enemies");


        EnemyTexture = new SharedTexture();
        EnemyTexture.BindTexture(enemySheet);

        ItemTexture = new SharedTexture();
        ItemTexture.BindTexture(itemSheet);

        MarioTexture = new SharedTexture();
        MarioTexture.BindTexture(marioSheet);

        FireTexture = new SharedTexture();
        FireTexture.BindTexture(fireSheet);

        lvl = new Level1();
        lvl.Initialize(blockTexture, enemySheet, itemSheet);
        Enemies = lvl.GetEnemies();

        Mario = new Player(MarioTexture);
        Mario.SetPositon(new Vector2(MarioStartPos[0], MarioStartPos[1]));
        Mario.PowerUp(Power.FireFlower);

        MarioGameController.Sprint2Controller data = new MarioGameController.Sprint2Controller
        {
            player = Mario,
            mouse = new MouseInfo(),
            keyb = new KeyboardInfo(),
            halfX = _graphics.PreferredBackBufferWidth / 2,
            halfY = _graphics.PreferredBackBufferHeight / 2
        };
        Controller = new MarioGameController(this, ref data);
        ItemsList = [
            new Fireflower(ItemTexture, 700, 368),
            //new Fireflower_Underground(ItemTexture, ScreenHeight, 600, 150),
            new Mushroom(ItemTexture, 700, 150),
            new OneUp(ItemTexture, 300, 150),
            new CollectableCoin(ItemTexture, 400, 250),
            new Star(ItemTexture, 200, 150),
            //new MovingPlatform_Size1(ItemTexture, ScreenHeight, 300, 300, 1),
            //new MovingPlatform_Size2(ItemTexture, ScreenHeight, 300, 200, 1),
            //new MovingPlatform_Size3(ItemTexture, ScreenHeight, 300, 100, 1),
            //new Cloud(ItemTexture, 0, 200),
        ];
        //Enemies = [
            //new Goomba(EnemyTexture, 300, 100),
            //new Goomba(EnemyTexture, 300, 200),
            //new Koopa(EnemyTexture, 300, 100)
            //];

        CollisionController.Instance.BindPlayer(Mario);
        foreach(IItems item in ItemsList)
            CollisionController.Instance.AddItem(item);
        foreach(IEnemy enemy in Enemies)
            CollisionController.Instance.AddEnemy(enemy);
    }

    protected override void Update(GameTime gameTime)
    {
        Controller.Update(gameTime);
        //lvl.Update(gameTime);
        Mario.Update(gameTime);

        for (int i = 0; i < ItemsList.Count; i++)
        {
            ItemsList[i].Update(gameTime);
            if (ItemsList[i].getCollected())
            {
                CollisionController.Instance.RemoveItem(ItemsList[i]);
            }
        }
        for (int i = 0; i < Enemies.Count; i++)
        {
            Enemies[i].Update(gameTime);
            if (!Enemies[i].GetIsAlive())
            {
                CollisionController.Instance.RemoveEnemy(Enemies[i]);
            }
        }
        int cameraX = Math.Max(Camera.Instance.Position.X, (int)Mario.Position.X - Camera.Instance.WindowSize.X / 2);
        Camera.Instance.Position = new Point(cameraX, 0);
        Camera.Instance.Update(gameTime);
        
        
        CollisionController.Instance.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        Camera.Instance.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}