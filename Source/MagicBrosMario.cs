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
    private Camera cam;
    private int enemyArraySize = 5;
    private int blockArraySize = 9;
    private int itemArraySize = 16;
    private int enemyIndex = 0;
    private int blockIndex = 0;
    private int itemIndex = 0;
    private const int enemyPos = 300;
    private const int blockPos = 200;
    private float[] MarioStartPos = { 100, 300 };
    private IEnemy[] enemy;
    private IBlock[] blocks;
    private IItems[] items;
    private SpriteFont _font;

    public MagicBrosMario()
    {
        _graphics = new GraphicsDeviceManager(this);
        cam = new Camera(_graphics);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        enemy = new IEnemy[enemyArraySize];
        blocks = new IBlock[blockArraySize];
        items = new IItems[itemArraySize];
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        LoadEnemies();
        LoadBlocks();
        LoadItems();
        _font = Content.Load<SpriteFont>("Font");
        Texture2D MarioSheet = Content.Load<Texture2D>("MarioSpriteSheet");
        texture = new SharedTexture();
        texture.BindTexture(MarioSheet);

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
    }

    // Make sure texture is set to characters
    private void LoadEnemies()
    {
        texture = new SharedTexture();
        texture.BindTexture(Content.Load<Texture2D>("characters"));
        SharedTexture fireSharedTexture = new SharedTexture();
        fireSharedTexture.BindTexture(Content.Load<Texture2D>("enemies"));
        // initialize enemies
        // all the sprite scales have to be set to 3f, bruh hooooooow
        enemy[0] = new Goomba(
            texture.NewAnimatedSprite(295, 187, 18, 18, 2, 0.2f), // alive
            texture.NewSprite(276, 187, 18, 18), // or maybe dead
            enemyPos,
            50,
            750
        );
        enemy[1] = new Koopa(
            texture.NewAnimatedSprite(296, 206, 18, 25, 2, 0.2f), // walkling right
            texture.NewAnimatedSprite(182, 206, 18, 25, 2, 0.2f), // walking left
            texture.NewSprite(144, 216, 16, 14), //  shell idle
            texture.NewSprite(144, 216, 16, 14), // repeate of shell idle
            texture.NewSprite(163, 215, 16, 15), // stomped
            texture.NewSprite(334, 215, 16, 15), // shell dead
            enemyPos,
            50,
            750
        );
        enemy[2] = new PiranhaPlant(
            texture.NewAnimatedSprite(125, 180, 16, 23, 2, 0.2f),
            enemyPos, // X position (pipe location)
            enemyPos - 50 // Y position (top of pipe - adjust based on your screen)
        );
        enemy[3] = new Bowser(
            texture.NewAnimatedSprite(255, 368, 35, 32, 4, 0.2f),
            texture.NewAnimatedSprite(116, 368, 35, 32, 2, 0.2f),
            fireSharedTexture,
            161, // Fire moving right - X coordinate (2 frames)
            253, // Fire moving right - Y coordinate
            101, // Fire moving left - X coordinate (2 frames)
            253, // Fire moving left - Y coordinate
            24, // Fire width
            8, // Fire height
            enemyPos, // Bowser Y position
            50, // Left bound
            750 // Right bound
        );
        enemy[4] = new RotatingFireBar(
            fireSharedTexture,
            364, // Fireball X position in sprite sheet
            188, // Fireball Y position in sprite sheet
            8, // Fireball width
            8, // Fireball height
            enemyPos, // Center X position on screen
            enemyPos, // Center Y position on screen
            6, // Number of fireballs
            24 // Spacing between fireballs
        );
    }

    private void LoadItems()
    {
        SharedTexture itemsTexture = new SharedTexture();
        itemsTexture.BindTexture(Content.Load<Texture2D>("items"));
        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;
        int positionX = 400;
        int positionY = 265;


		items[0] = new Fireflower(itemsTexture, screenWidth, screenHeight, positionX, positionY);
		//CollisionController.Instance.AddItem((Fireflower)items[0]);
		items[1] = new Fireflower_Underground(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[2] = new QuestionBlock(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[3] = new Coin(itemsTexture, screenWidth, screenHeight, positionX, positionY); 
        items[4] = new Mushroom(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[5] = new OneUp(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[6] = new Star(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[7] = new Spring_Start(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[8] = new Spring_Compressed(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[9] = new Spring_Stretched(itemsTexture, screenWidth, screenHeight, positionX, positionY);

        //The last parameter is for direction, -1 means up and 1 means down
        items[10] = new MovingPlatform_Size1(itemsTexture, screenWidth, screenHeight, positionX, positionY, -1);
        items[11] = new MovingPlatform_Size2(itemsTexture, screenWidth, screenHeight, positionX, positionY, -1);
        items[12] = new MovingPlatform_Size3(itemsTexture, screenWidth, screenHeight, positionX, positionY, -1);

        items[13] = new Cloud(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[14] = new PrincessPeach(itemsTexture, screenWidth, screenHeight, positionX, positionY);
        items[15] = new Toad(itemsTexture, screenWidth, screenHeight, positionX, positionY);
    }

    private void LoadBlocks()
    {
        BlockFactory.BindTexture(Content.Load<Texture2D>(BlockFactory.TEXTURE_NAME));
        blocks =
        [
            BlockFactory.VoidBlock().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.SkyBlock().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.GroundBlock().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.BlueGroundBlock().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.Bricks().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.BlueBricks().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.BaseBlock().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.QuestionMarkBlock().WithPosition(blockPos, blockPos).WithScale(4),
            BlockFactory.EmptyQuestionMarkBlock().WithPosition(blockPos, blockPos).WithScale(4),
        ];
    }

    protected override void Update(GameTime gameTime)
    {
        Controller.Update(gameTime);
        Mario.Update(gameTime);
		CollisionController.Instance.Update(gameTime);
		enemy[enemyIndex].Update(gameTime);
        items[itemIndex].Update(gameTime);
        blocks[blockIndex].Update(gameTime);

    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        enemy[enemyIndex].Draw(_spriteBatch);
        blocks[blockIndex].Draw(_spriteBatch);

        items[itemIndex].Draw(_spriteBatch);

        Mario.Draw(_spriteBatch);

        _spriteBatch.DrawString(_font, "Super Mario Bros", new Vector2(150, 100), Color.White); //SAMPLE USAGE
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void incrementEnemy()
    {
            enemyIndex = (enemyIndex + 1) % enemyArraySize;

    }

    public void decrementEnemy(){

            enemyIndex = (enemyIndex - 1 + enemyArraySize) % enemyArraySize;

    }

    public void incrementBlock()
    {
            blockIndex = (blockIndex + 1) % blockArraySize;

    }

    public void decrementBlock()
    {
            blockIndex = (blockIndex - 1 + blockArraySize) % blockArraySize;

    }

    public void incrementItem()
    {
            itemIndex = (itemIndex + 1) % itemArraySize;

    }

    public void decrementItem()
    {
            itemIndex = (itemIndex - 1 + itemArraySize) % itemArraySize;

    }

    public void displayPowerUp(int index)
    {
        switch (index)
        {
            case 0:
                Mario.ChangeState(new SmallMarioIdleState(Mario, texture, 0.15f, 3));
                break;
            case 1:
                Mario.ChangeState(new BigMarioIdleState(Mario, texture, 0.15f, 3));
                break;
            case 2:
                Mario.PowerUp(Power.FireFlower);
                break;
            case 3:
                Mario.PowerUp(Power.Star);
                break;
        }
    }

    public void resetGame()
    {
        displayPowerUp(0);
        Mario.SetPositon(new Vector2(MarioStartPos[0], MarioStartPos[1]));
        for (int i = 0; i < enemyArraySize; i++)
        {
            //enemy[i].setPostion(enemyPos);
            if (i < blockArraySize) // because blockArraySize < enemyArraySize
            {
                // block[i].setPosition(blockPos);
            }
        }

        enemyIndex = 0;
        itemIndex = 0;
        blockIndex = 0;
    }
}