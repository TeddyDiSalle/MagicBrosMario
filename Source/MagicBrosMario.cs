using System;
using System.Dynamic;
using MagicBrosMario.Source.GameStates;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MagicBrosMario.Source;

public class MagicBrosMario : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private IGameState _currentStateDONOUTUSE;
    private bool isPaused = false;
    public bool finishedLevel1 = false;
    private readonly Color backgroundColor = new Color(146, 144, 255);
    public IGameState CurrentState
    {
        get => _currentStateDONOUTUSE;
        set
        {
            if (_currentStateDONOUTUSE is PlayingState playingState)
            {
                playingState.Clear();
            }

            if (value is PlayingState newPlayingState)
            {
                if(level == null || level.GetType() != newPlayingState._level.GetType()){
                    //Console.WriteLine("Switching to level: " + newPlayingState._level.Name);
                    MarioStartPosition = new Point(-1, -1); // let go control of mario's start position
                }
                level = newPlayingState._level;
                newPlayingState.Initialize();
            }
            if(value is TitleScreenState)
            {
                MarioStartPosition = new Point(-1, -1);
            }

            Camera.Instance.Position = Point.Zero; // Camera has to go to the beginning of the level
            _currentStateDONOUTUSE = value;
        }
    }

    public Player Mario;

    public SharedTexture MarioTexture { get; }
    public SharedTexture ItemTexture { get; }
    public SharedTexture EnemyTexture { get; }
    public SharedTexture FireTexture { get; }

    public Texture2D TitleScreen { get; private set; }
    public SpriteFont font { get; private set; }

    //Use this for now
    public ILevel level { get; set; }

    // Set to -1, -1 to indicate that it has not been set yet
    public Point MarioStartPosition {get; set;} = new Point(-1,-1);
    //Keep Just In Case
    //private ILevel lvl;

    public static MagicBrosMario INSTANCE { get; private set; }


    public enum GameState
    {
        TitleScreen,
        Loading, // The black transition screen
        Playing,
        Paused,
        GameOver
    }

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

        Mario = new Player(MagicBrosMario.INSTANCE.MarioTexture);
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Texture2D marioSheet = Content.Load<Texture2D>("MarioSpriteSheet");
        Texture2D blockTexture = Content.Load<Texture2D>("blocks");
        Texture2D itemSheet = Content.Load<Texture2D>("items");
        Texture2D enemySheet = Content.Load<Texture2D>("characters");
        Texture2D fireSheet = Content.Load<Texture2D>("enemies");

        SoundController.LoadSounds();


        TitleScreen = Content.Load<Texture2D>("MarioTitleScreen");

        EnemyTexture.BindTexture(enemySheet);
        ItemTexture.BindTexture(itemSheet);
        MarioTexture.BindTexture(marioSheet);
        FireTexture.BindTexture(fireSheet);
        font = Content.Load<SpriteFont>("Font");

        setController();
        CurrentState = new TitleScreenState();
        this.Mario = new Player(MarioTexture);
    }


    private void setController()
    {
        MarioGameController.Sprint5Controller data = new MarioGameController.Sprint5Controller
        {
            mouse = new MouseInfo(),
            keyb = new KeyboardInfo(),
            gamepad = new GamePadInfo(),
            halfX = Camera.Instance.WindowSize.X / 2,
            halfY = Camera.Instance.WindowSize.Y / 2
        };
        MarioGameController.Initialize(ref data);
    }

    protected override void Update(GameTime gameTime)
    {
        MarioGameController.Update(gameTime);

        if (!isPaused)
        {
            CurrentState.Update(gameTime);
        }

        //Temp stuff may need some may not
        //Controller.Update(gameTime);
        //lvl.Update(gameTime);
        //Mario.Update(gameTime);

        //int cameraX = Math.Max(Camera.Instance.Position.X, (int)Mario.Position.X - Camera.Instance.WindowSize.X / 2);
        //Camera.Instance.Position = new Point(cameraX, 0);
        //Camera.Instance.Update(gameTime);
        //HUD.Instance.Update(gameTime);

        //CollisionController.Instance.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(backgroundColor);
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, sortMode: SpriteSortMode.BackToFront);
        
        CurrentState.Draw(_spriteBatch);
        Camera.Instance.Draw(_spriteBatch);
        _spriteBatch.End();


        base.Draw(gameTime);
    }


    public void changePaused()
    {
        isPaused = !isPaused;
    }

    public bool getPaused()
    {
        return isPaused;
    }

    /*
        public void Debug()
        {
            resetLevel();
            Texture2D blockTexture = Content.Load<Texture2D>("blocks");
            Texture2D itemSheet = Content.Load<Texture2D>("items");
            Texture2D enemySheet = Content.Load<Texture2D>("characters");

            level = new DebugRoom();
            level.Initialize(Content, blockTexture, enemySheet, itemSheet);
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
            HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.StartLevel });
            resetMario();
        }

        private void resetLevel()
        {
            if (level != null) level.Clear();

            Camera.Instance.Position = Point.Zero;
            Camera.Instance.Sprites.Clear();
        }

        private void resetMario()
        {
            CollisionController.Instance.RemovePlayer();
            Mario = new Player(MarioTexture);
            Mario.SetPositon(new Vector2(level.MarioStartPosX, level.MarioStartPosY));
            Mario.PowerUp(Power.FireFlower);
            setController();
        }

        private void resetHUD(int x)
        {
            HUD.Instance.SetLevel(x);
            HUD.Instance.SetTime(200);
        }

    */
}