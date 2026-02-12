using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private ISprite currentSprite;
    private StandstillSprite sprite1;
    private AnimatedSprite sprite2;
    private MovingSprite sprite3;
    private AnimatedMovingSprite sprite4;

    private ISprite[] Sprites;
    private IController Controller;

    private Font font;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
    }

    protected override void Initialize()
    {

        Sprites = new ISprite[] {sprite1, sprite2, sprite3, sprite4};
        IController[] controllers =  {new KeyboardInfo(), new MouseInfo()};
        int[] cords = {Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2};
        Controller = new MarioGameController(this, Sprites, controllers, cords);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D _texture = Content.Load<Texture2D>("characters");
        SpriteFont fontDesc = Content.Load<SpriteFont>("font");

        Vector2 center = new Vector2(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);
        sprite1 = new StandstillSprite(_texture, new Rectangle(276, 43, 13, 17), center);
        sprite2 = new AnimatedSprite(_texture, new Rectangle(291, 44, 13, 16),
            new Rectangle(306, 43, 12, 17), new Rectangle(320, 43, 16, 17), center);
        sprite3 = new MovingSprite(_texture, new Rectangle(485, 45, 15, 15), center, Window.ClientBounds.Height);
        sprite4 = new AnimatedMovingSprite(_texture, new Rectangle(291, 44, 13, 16),
            new Rectangle(306, 43, 12, 17), new Rectangle(320, 43, 16, 17), center, Window.ClientBounds.Width);

        font = new Font(fontDesc, new Vector2(200, 400));

        currentSprite = sprite1;
    }

    protected override void Update(GameTime gameTime)
    {
        Controller.Update();

        currentSprite.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        currentSprite.Draw(_spriteBatch);

        font.Draw(_spriteBatch);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    public void SetCurrentSprite(ISprite sprite)
    {
        currentSprite = sprite;
    }
}

