using MagicBrosMario.Source.Level;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


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
    private Camera cam;
    private float[] MarioStartPos = { 100, 300 };
    private ILevel lvl;
    private SpriteFont _font;

    private IItems[] CollectableItems;
    private IEnemy[] Enemies;
    private int ScreenWidth;
    private int ScreenHeight;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        cam = new Camera(_graphics);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        ScreenWidth = Window.ClientBounds.Width;
        ScreenHeight = Window.ClientBounds.Height;
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _font = Content.Load<SpriteFont>("Font");

        Texture2D marioSheet = Content.Load<Texture2D>("MarioSpriteSheet");
        Texture2D blockTexture = Content.Load<Texture2D>("blocks");
        Texture2D itemSheet = Content.Load<Texture2D>("items");
        Texture2D enemySheet = Content.Load<Texture2D>("characters");

        EnemyTexture = new SharedTexture();
        EnemyTexture.BindTexture(enemySheet);

        ItemTexture = new SharedTexture();
        ItemTexture.BindTexture(itemSheet);

        MarioTexture = new SharedTexture();
        MarioTexture.BindTexture(marioSheet);

        lvl = new Level1();
        lvl.Initialize(blockTexture);

        Mario = new Player(MarioTexture);
        Mario.SetPositon(new Vector2(MarioStartPos[0], MarioStartPos[1]));

        MarioGameController.Sprint2Controller data = new MarioGameController.Sprint2Controller
        {
            player = Mario,
            mouse = new MouseInfo(),
            keyb = new KeyboardInfo(),
            halfX = _graphics.PreferredBackBufferWidth / 2,
            halfY = _graphics.PreferredBackBufferHeight / 2
        };
        Controller = new MarioGameController(this, ref data);
        CollectableItems = [
            new Fireflower(ItemTexture, ScreenWidth, ScreenHeight, 700, 150),
            new Fireflower_Underground(ItemTexture, ScreenWidth, ScreenHeight, 600, 150),
            new Mushroom(ItemTexture, ScreenWidth, ScreenHeight, 100, 150),
            new OneUp(ItemTexture, ScreenWidth, ScreenHeight, 0, 150),
            new Star(ItemTexture, ScreenWidth, ScreenHeight, 50, 150),
        ];
        Enemies = [
            new Goomba(EnemyTexture.NewAnimatedSprite(295, 187, 18, 18, 2, 0.2f),EnemyTexture.NewSprite(276, 187, 18, 18), 250, 500, 550),
            new Goomba(EnemyTexture.NewAnimatedSprite(295, 187, 18, 18, 2, 0.2f),EnemyTexture.NewSprite(276, 187, 18, 18), 250, 400, 550),
            new Koopa(EnemyTexture.NewAnimatedSprite(296, 206, 18, 25, 2, 0.2f), // walkling right
            EnemyTexture.NewAnimatedSprite(182, 206, 18, 25, 2, 0.2f), // walking left
            EnemyTexture.NewSprite(144, 216, 16, 14), //  shell idle
            EnemyTexture.NewSprite(144, 216, 16, 14), // repeate of shell idle
            EnemyTexture.NewSprite(163, 215, 16, 15), // stomped
            EnemyTexture.NewSprite(334, 215, 16, 15), // shell dead
            250,
            300,
            550)
            ];

        CollisionController.Instance.BindPlayer(Mario);
        foreach(IItems item in CollectableItems)
            CollisionController.Instance.AddItem(item);
        foreach(IEnemy enemy in Enemies)
            CollisionController.Instance.AddEnemy(enemy);
    }

    protected override void Update(GameTime gameTime)
    {
        Controller.Update(gameTime);
        lvl.Update(gameTime);
        Mario.Update(gameTime);
        CollisionController.Instance.Update(gameTime);
        foreach (IItems item in CollectableItems)
        {
            item.Update(gameTime);
        }
        foreach (IEnemy enemy in Enemies)
        {
            enemy.Update(gameTime);
        }
            
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        lvl.Draw(_spriteBatch);

        Mario.Draw(_spriteBatch);
        foreach (IItems item in CollectableItems)
            item.Draw(_spriteBatch);
        foreach (IEnemy enemy in Enemies)
            enemy.Draw(_spriteBatch);
        _spriteBatch.DrawString(_font, "Super Mario Bros", new Vector2(150, 100), Color.White); //SAMPLE USAGE
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}