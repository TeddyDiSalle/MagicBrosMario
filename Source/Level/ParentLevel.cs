// Made By Teddy

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Level;

public abstract class ParentLevel : ILevel
{
    private static bool _managersInitialized = false;

    protected IBlock[][] blocks;
    protected IEnemy[][] enemies;
    protected IItems[][] items;

    protected string Level1BlockCVS;
    protected string Level1EnemyCVS;
    protected string Level1ItemCVS;

    protected string BackgroundName;
    protected int backWidth;
    protected int backHeight;
    protected MusicType backgroundMusic;
    protected float volume = 0.5f;

    private readonly static int _blockSize = 16;
    private readonly static int _scale = 2;
    protected static int tileSize = _blockSize * _scale;

    protected string[] blockLines;
    protected string[] enemyLines;
    protected string[] itemLines;

    private int levWidth;
    private int levHeight;
    protected List<Point> checkpointPositions = new List<Point>();

    public Point MarioStartPos { get; protected set; }
    public string Name { get; protected set; }
    public int TimeLimit { get; protected set; }

    private readonly DeferredPipeLinkResolver _pipeResolver = new();
    private readonly HashSet<IEnemy> _activatedEnemies = new();
    private int bridgeGroupCounter = 0;
    private int bridgeOrderCounter = 0;

    private ISprite backgroundSprite;

    public void Initialize(
        Microsoft.Xna.Framework.Content.ContentManager contentManager,
        Texture2D bTexture,
        Texture2D eTexture,
        Texture2D iTexture)
    {
        if (MagicBrosMario.INSTANCE.MarioStartPosition.X == -1)
        {
            MagicBrosMario.INSTANCE.MarioStartPosition = MarioStartPos;
        }

        ValidateCsvDimensions();
        InitializeLevelArrays();

        SoundController.PlayMusic(backgroundMusic, volume);

        HUD.Instance.LevelStart();
        HUD.Instance.SetTime(TimeLimit);

        InitializeManagers(bTexture, eTexture, iTexture);
        LoadContent();
    }

    private void ValidateCsvDimensions()
    {
        levHeight = blockLines.Length;

        if (enemyLines.Length != levHeight || itemLines.Length != levHeight)
        {
            throw new Exception("Level: Enemy or Item CSV file has different height than Block CSV file");
        }

        levWidth = blockLines[0].Split(',').Length;

        if (enemyLines[0].Split(',').Length != levWidth || itemLines[0].Split(',').Length != levWidth)
        {
            throw new Exception(
                $"Level: Enemy or Item CSV file has different width than Block CSV file {levWidth} vs {enemyLines[0].Split(',').Length} vs {itemLines[0].Split(',').Length}");
        }
    }

    private void InitializeLevelArrays()
    {
        blocks = new IBlock[levHeight][];
        enemies = new IEnemy[levHeight][];
        items = new IItems[levHeight][];

        for (int i = 0; i < levHeight; i++)
        {
            blocks[i] = new IBlock[levWidth];
            enemies[i] = new IEnemy[levWidth];
            items[i] = new IItems[levWidth];
        }
    }

    private static void InitializeManagers(Texture2D bTexture, Texture2D eTexture, Texture2D iTexture)
    {
        if (_managersInitialized)
            return;

        _managersInitialized = true;
        BlockManager.Initialize(bTexture);
        EnemyManager.Initialize(eTexture);
        ItemManager.Initialize(iTexture);
    }

    public void ReadFromCSV()
    {
        blockLines = File.ReadLines(Level1BlockCVS).ToArray();
        enemyLines = File.ReadLines(Level1EnemyCVS).ToArray();
        itemLines = File.ReadLines(Level1ItemCVS).ToArray();
    }

    protected void LoadBackground()
    {
        Texture2D backgroundTexture =
            MagicBrosMario.INSTANCE.Content.Load<Texture2D>("LevelData/" + Name + "/" + BackgroundName);

        backWidth = backgroundTexture.Width;
        backHeight = backgroundTexture.Height;

        SharedTexture backgroundSharedTexture = new SharedTexture();
        backgroundSharedTexture.BindTexture(backgroundTexture);

        backgroundSprite = backgroundSharedTexture.NewSprite(0, 0, backWidth, backHeight);
        backgroundSprite.Scale = (float)tileSize / _blockSize;
        backgroundSprite.Visible = true;
        backgroundSprite.Background();
    }

    public void Update(GameTime gt)
    {
        Rectangle activationBounds = GetEnemyActivationBounds();
        for (int i = 0; i < checkpointPositions.Count; i++)
        {
            if (MagicBrosMario.INSTANCE.Mario.Position.X > checkpointPositions[i].X)
            {
                //Console.WriteLine("Checkpoint reached at posdition: " + checkpointPositions[i]);
                MagicBrosMario.INSTANCE.MarioStartPosition = checkpointPositions[i];
                checkpointPositions.RemoveAt(i);
            }
        }


        for (int r = 0; r < levHeight; r++)
        {
            for (int c = 0; c < levWidth; c++)
            {
                if (enemies[r][c] != null)
                {
                    IEnemy enemy = enemies[r][c];

                    if (enemy.AlwaysActive || _activatedEnemies.Contains(enemy) ||
                        ShouldActivateEnemy(enemy, activationBounds))
                    {
                        if (_activatedEnemies.Add(enemy))
                            CollisionController.Instance.AddEnemy(enemy);
                        enemy.Update(gt);
                    }
                }

                if (items[r][c] != null)
                {
                    items[r][c].Update(gt);
                    if (items[r][c].getCollected())
                    {
                        Collision.CollisionController.Instance.RemoveItem(items[r][c]);
                        items[r][c] = null;
                    }
                }
            }
        }
    }

    public void Draw(SpriteBatch sb) { }

    protected void LoadContent()
    {
        if (BackgroundName != null)
        {
            LoadBackground();
        }

        _pipeResolver.Clear();

        for (int r = 0; r < levHeight; r++)
        {
            string[] blockIds = blockLines[r].Split(',');
            string[] enemyIds = enemyLines[r].Split(',');
            string[] itemIds = itemLines[r].Split(',');

            for (int c = 0; c < levWidth; c++)
            {
                //Console.WriteLine($"Loading cell at row {r}, col {c} with blockId '{blockIds[c]}', enemyId '{enemyIds[c]}', itemId '{itemIds[c]}'");
                LevelCellToken token = LevelCellTokenParser.ParseBlockToken(blockIds[c]);
                string enemyId = enemyIds[c].Trim();
                string itemId = itemIds[c].Trim();

                LoadItemCell(token, itemId, r, c);
                LoadBlockCell(token, itemId, r, c);
                LoadEnemyCell(enemyId, r, c);
            }
        }

        _pipeResolver.FinalizeLinks(blocks, tileSize);
    }

    private void LoadItemCell(LevelCellToken token, string itemId, int row, int col)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            items[row][col] = null;
            return;
        }

        if (string.IsNullOrEmpty(token.BlockId))
        {
            int? group = null;
            if (itemId == "20")
            {
                // axe has appeared so there is a new group of bridge blocks coming up
                bridgeGroupCounter++;
                bridgeOrderCounter = 0;
                group = bridgeGroupCounter;
            }

            items[row][col] = ItemManager.CreateItem(itemId, col * tileSize, row * tileSize, group);
        }
    }

    private void LoadBlockCell(LevelCellToken token, string itemId, int row, int col)
    {
        if (string.IsNullOrEmpty(token.BlockId))
        {
            blocks[row][col] = null;

            if (!string.IsNullOrEmpty(token.PipeLabel))
            {
                _pipeResolver.RegisterMarker(row, col, token.PipeLabel);
            }

            return;
        }

        if (!string.IsNullOrEmpty(token.PipeLabel))
        {
            _pipeResolver.RegisterPipeHalf(token.BlockId, row, col, token.PipeLabel);
            return;
        }

        int? group = null;
        int? order = null;
        if (token.BlockId == "16")
        {
            // bridge block
            if (!(blocks[row][col - 1] is BridgeBlock))
            {
                // new bridge group
                //We take care of a new bridge group when the axe shows up on row-1
            } else
            {
                // there is already a bridge block to the left, same group
                bridgeOrderCounter++;
            }

            group = bridgeGroupCounter;
            order = bridgeOrderCounter;
        }

        blocks[row][col] = BlockManager.CreateBlock(
            token.BlockId,
            col * tileSize,
            row * tileSize,
            LevelCellTokenParser.ToQuestionBlockItem(itemId),
            null,
            group,
            order
        );

        CollisionController.Instance.AddBlock(blocks[row][col]);
    }

    private void LoadEnemyCell(string enemyId, int row, int col)
    {
        if (string.IsNullOrEmpty(enemyId))
        {
            enemies[row][col] = null;
            return;
        }

        enemies[row][col] = EnemyManager.CreateEnemy(enemyId, col * tileSize, row * tileSize);
    }

    private Rectangle GetEnemyActivationBounds()
    {
        const int leftBufferTiles = 2;
        const int rightBufferTiles = 6;
        const int verticalBufferTiles = 2;

        return new Rectangle(
            Camera.Instance.Position.X - leftBufferTiles * tileSize,
            Camera.Instance.Position.Y - verticalBufferTiles * tileSize,
            Camera.Instance.WindowSize.X + (leftBufferTiles + rightBufferTiles) * tileSize,
            Camera.Instance.WindowSize.Y + (2 * verticalBufferTiles) * tileSize
        );
    }

    private bool ShouldActivateEnemy(IEnemy enemy, Rectangle activationBounds)
    {
        return enemy.CollisionBox.Intersects(activationBounds);
    }

    public void AddItem(IItems item)
    {
        for (int r = 0; r < levHeight; r++)
        {
            for (int c = 0; c < levWidth; c++)
            {
                //Console.WriteLine($"Trying to add item at row {r}, col {c}");
                if (items[r][c] == null)
                {
                    items[r][c] = item;
                    CollisionController.Instance.AddItem(item);
                    return;
                }
            }
        }

        throw new Exception("No empty slot available to add item");
    }

    public void Clear()
    {
        _activatedEnemies.Clear();
        CollisionController.Instance.RemoveAll();
        blocks = [];
        items = [];
        enemies = [];
        if (backgroundSprite != null)
        {
            backgroundSprite.Drop();
            backgroundSprite = null;
        }

        HUD.Instance.LevelOver();
        SoundController.StopMusic();
    }

    public void GoToNextLevel()
    {
        // need further implmentaition
        _activatedEnemies.Clear();
        CollisionController.Instance.RemoveAll();
        blocks = [];
        items = [];
        enemies = [];
        if (backgroundSprite != null)
        {
            backgroundSprite.Drop();
            backgroundSprite = null;
        }

        HUD.Instance.LevelOver();
        SoundController.StopMusic();
    }

    public void showPositionOnScreen()
    {
        Vector2 exactPosition = MagicBrosMario.INSTANCE.Mario.Position;
        Vector2 tilePosition = new Vector2((int)(exactPosition.X / tileSize), (int)(exactPosition.Y / tileSize));
        Console.WriteLine($"Mario is at pixel position {exactPosition} and tile position {tilePosition}");
    }
}