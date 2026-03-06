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
    private SharedTexture texture;
    private SharedTexture ItemTexture;
    private Camera cam;
    private float[] MarioStartPos = { 100, 300 };
    private ILevel lvl;
    private SpriteFont _font;

    private Fireflower fireflower;
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

        ItemTexture = new SharedTexture();
        ItemTexture.BindTexture(itemSheet);

        texture = new SharedTexture();
        texture.BindTexture(marioSheet);

        lvl = new Level1();
        lvl.Initialize(blockTexture);

        Mario = new Player(texture);
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
        fireflower = new Fireflower(ItemTexture, ScreenWidth, ScreenHeight, 600, 200);

        CollisionController.Instance.BindPlayer(Mario);
        CollisionController.Instance.AddItem(fireflower);
    }

    protected override void Update(GameTime gameTime)
    {
        Controller.Update(gameTime);
        lvl.Update(gameTime);
        Mario.Update(gameTime);
        CollisionController.Instance.Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        lvl.Draw(_spriteBatch);

        Mario.Draw(_spriteBatch);
        fireflower.Draw(_spriteBatch);
        _spriteBatch.DrawString(_font, "Super Mario Bros", new Vector2(150, 100), Color.White); //SAMPLE USAGE
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}