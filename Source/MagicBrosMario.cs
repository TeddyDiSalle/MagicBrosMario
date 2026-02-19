using System;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
<<<<<<< Updated upstream
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Reflection.Metadata;
=======
>>>>>>> Stashed changes


namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
	private GraphicsDeviceManager _graphics;
	private SpriteBatch _spriteBatch;
	private MarioGameController Controller;

	public MarioStates.Player Mario;
	private SharedTexture texture;

<<<<<<< Updated upstream
    private int enemyArraySize = 5;
    private int blockArraySize = 4;
    private int enemyIndex = 0;
    private int blockIndex = 0;
    private const int enemyPos = 300;
    private const int blockPos = 200;
    private float[] MarioStartPos = {100,300};
    private IEnemy[] enemy;
    private IBlock[] block;
=======
	private int enemyArraySize = 5;
	private int blockArraySize = 4;
	private int itemArraySize = 14;
	private int enemyIndex = 0;
	private int blockIndex = 0;
	private int itemIndex = 0;
	private const int enemyPos = 300;
	private const int blockPos = 200;
	private float[] MarioStartPos = { 100, 300 };
	private IEnemy[] enemy;
	private IBlock[] block;
	private IItems[] items;
>>>>>>> Stashed changes

	public MagicBrosMario()
	{
		_graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		IsMouseVisible = true;
		enemy = new IEnemy[enemyArraySize];
		block = new IBlock[blockArraySize];
		items = new IItems[itemArraySize];
	}

	protected override void LoadContent()
	{

		_spriteBatch = new SpriteBatch(GraphicsDevice);

		LoadEnemies();
		LoadBlocks();
		LoadItems();

<<<<<<< Updated upstream
        Mario = new Player(texture);
        Mario.SetPositon(new Vector2(MarioStartPos[0],MarioStartPos[1]));
        MarioGameController.Sprint2Controller data = new MarioGameController.Sprint2Controller{
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
            enemyPos,  // X position (pipe location)
            enemyPos - 50   // Y position (top of pipe - adjust based on your screen)
        );
        enemy[3] = new Bowser(
            texture.NewAnimatedSprite(255, 368, 35, 32, 4, 0.2f),
            texture.NewAnimatedSprite(116, 368, 35, 32, 2, 0.2f),
            fireSharedTexture,
            161,   // Fire moving right - X coordinate (2 frames)
            253,   // Fire moving right - Y coordinate
            101,    // Fire moving left - X coordinate (2 frames)
            253,    // Fire moving left - Y coordinate
            24,             // Fire width
            8,             // Fire height
            enemyPos,            // Bowser Y position
            50,             // Left bound
            750             // Right bound
        );
        enemy[4] = new RotatingFireBar(
            fireSharedTexture,
            364,    // Fireball X position in sprite sheet
            188,    // Fireball Y position in sprite sheet
            8,    // Fireball width
            8,    // Fireball height
            enemyPos,  // Center X position on screen
            enemyPos,  // Center Y position on screen
            6,    // Number of fireballs
            24    // Spacing between fireballs
        );
    }
=======
		Content.Load<SpriteFont>("font");
		Content.Load<Texture2D>("characters");
		Texture2D itemsTexture = Content.Load<Texture2D>("items");
		Texture2D MarioSheet = Content.Load<Texture2D>("MarioStarSheet");
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
			enemyPos,  // X position (pipe location)
			enemyPos - 50   // Y position (top of pipe - adjust based on your screen)
		);
		enemy[3] = new Bowser(
			texture.NewAnimatedSprite(255, 368, 35, 32, 4, 0.2f),
			texture.NewAnimatedSprite(116, 368, 35, 32, 2, 0.2f),
			fireSharedTexture,
			161,   // Fire moving right - X coordinate (2 frames)
			253,   // Fire moving right - Y coordinate
			101,    // Fire moving left - X coordinate (2 frames)
			253,    // Fire moving left - Y coordinate
			24,             // Fire width
			8,             // Fire height
			enemyPos,            // Bowser Y position
			50,             // Left bound
			750             // Right bound
		);
		enemy[4] = new RotatingFireBar(
			fireSharedTexture,
			364,    // Fireball X position in sprite sheet
			188,    // Fireball Y position in sprite sheet
			8,    // Fireball width
			8,    // Fireball height
			enemyPos,  // Center X position on screen
			enemyPos,  // Center Y position on screen
			6,    // Number of fireballs
			24    // Spacing between fireballs
		);
	}
>>>>>>> Stashed changes

	private void LoadItems()
	{
		SharedTexture itemsTexture = new SharedTexture();
		itemsTexture.BindTexture(Content.Load<Texture2D>("items"));

		//Using this one to test, reminder to delete later
		//items[0] = new Spring(itemsTexture);

<<<<<<< Updated upstream
    private static long nanoTime() {
        long nano = 10000L * Stopwatch.GetTimestamp();
        nano /= TimeSpan.TicksPerMillisecond;
        nano *= 100L;
        return nano;
    }

    protected override void Update(GameTime gameTime){
        Controller.Update(gameTime); 
        Mario.Update(gameTime);
        enemy[enemyIndex].Update(gameTime);
        // block[blockIndex].Update(gameTime);
        
    }
=======
		items[0] = new Coin(itemsTexture);
		items[1] = new Fireflower_Underground(itemsTexture);
		items[2] = new QuestionBlock(itemsTexture);
		items[3] = new Coin(itemsTexture);
		items[4] = new Mushroom(itemsTexture);
		items[5] = new OneUp(itemsTexture);
		items[6] = new Star(itemsTexture);
		items[7] = new Spring(itemsTexture);
		items[8] = new PrincessPeach(itemsTexture);
		items[9] = new Toad(itemsTexture);
		items[10] = new MovingPlatform_Size1(itemsTexture);
		items[11] = new MovingPlatform_Size2(itemsTexture);
		items[12] = new MovingPlatform_Size3(itemsTexture);
		items[13] = new Cloud_Maybe_RenameLater(itemsTexture);
>>>>>>> Stashed changes

	}

	private void LoadBlocks()
	{

	}

	private static long nanoTime()
	{
		long nano = 10000L * Stopwatch.GetTimestamp();
		nano /= TimeSpan.TicksPerMillisecond;
		nano *= 100L;
		return nano;
	}

<<<<<<< Updated upstream
    public void incrementEnemy(){
        enemyIndex = (enemyIndex + 1) % enemyArraySize;
    }
    public void decrementEnemy(){
        enemyIndex = (enemyIndex - 1 + enemyArraySize) % enemyArraySize;
    }
    public void incrementBlock(){
        blockIndex = (blockIndex + 1) % blockArraySize;
    }
    public void decrementBlock(){
        blockIndex = (blockIndex - 1 + blockArraySize) % blockArraySize;
    }
    public void incrementItem(){
        //TO DO
    }
    public void decrementItem(){
        //TO DO
    }
    public void displayPowerUp(int index){
        switch (index)
        {
            case 0:
                Mario.ChangeState(new SmallMarioIdleState(Mario, texture, 0.15, 3));
                break;
            case 1:
                Mario.ChangeState(new BigMarioIdleState(Mario, texture, 0.15, 3));
                break;
            case 2:
                Mario.PowerUp(Power.FireFlower);
                break;
            case 3:
                Mario.PowerUp(Power.Star);
                break;
        }
    }
    public void resetGame(){
        displayPowerUp(0);
        Mario.SetPositon(new Vector2(MarioStartPos[0],MarioStartPos[1]));
        for(int i = 0; i <enemyArraySize; i++)
        {
            //enemy[i].setPostion(enemyPos);
            if(i < blockArraySize) // because blockArraySize < enemyArraySize
            {
                // block[i].setPosition(blockPos);
            }
        }
        enemyIndex = 0;
        blockIndex = 0;
    }
=======
	protected override void Update(GameTime gameTime)
	{
		Controller.Update(gameTime);
		Mario.Update(gameTime);
		enemy[enemyIndex].Update(gameTime);
		items[itemIndex].Update(gameTime);
		// block[blockIndex].Update(gameTime);

	}

	protected override void Draw(GameTime gameTime)
	{
		GraphicsDevice.Clear(Color.CornflowerBlue);

		_spriteBatch.Begin(samplerState: SamplerState.PointClamp);

		enemy[enemyIndex].Draw(_spriteBatch);
		// lblock[blockIndex].Draw(_spriteBatch);

		items[itemIndex].Draw(_spriteBatch);

		Mario.Draw(_spriteBatch);
		_spriteBatch.End();

		base.Draw(gameTime);
	}

	public void incrementEnemy()
	{
		enemyIndex = (enemyIndex + 1) % enemyArraySize;
	}
	public void decrementEnemy()
	{
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
		itemIndex = (itemIndex - 1 + itemArraySize) % itemArraySize;
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
				Mario.ChangeState(new SmallMarioIdleState(Mario, texture, 0.15, 3));
				break;
			case 1:
				Mario.ChangeState(new BigMarioIdleState(Mario, texture, 0.15, 3));
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
		blockIndex = 0;
	}
>>>>>>> Stashed changes
}
