// Made By Teddy

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private string[][] blockCsv;
    private string[][] enemyCsv;
    private string[][] itemCsv;

    private int levWidth;
    private int levHeight;

    protected List<Point> checkpointPositions = new List<Point>();

    public Point MarioStartPos { get; protected set; }
    public string Name { get; protected set; }
    public int TimeLimit { get; protected set; }

    private const string AxeItemId = "06";
    private const string BridgeBlockId = "16";

    private readonly DeferredPipeLinkResolver _pipeResolver = new();
    private readonly HashSet<IEnemy> _activatedEnemies = new();

    private int bridgeGroupCounter = 0;
    private int bridgeOrderCounter = 1;
    private ISprite backgroundSprite;

    public void Initialize(
        Microsoft.Xna.Framework.Content.ContentManager contentManager,
        Texture2D bTexture,
        Texture2D eTexture,
        Texture2D iTexture)
    {
        _ = contentManager;

        if (MagicBrosMario.INSTANCE.MarioStartPosition.X == -1)
        {
            MagicBrosMario.INSTANCE.MarioStartPosition = MarioStartPos;
        }

        EnsureCsvArraysAreLoaded();
        ValidateCsvDimensions();
        InitializeLevelArrays();
        InitializeLevelState();
        InitializeManagers(bTexture, eTexture, iTexture);
        LoadContent();
    }

    public void ReadFromCSV()
    {
        blockLines = File.ReadLines(Level1BlockCVS).ToArray();
        enemyLines = File.ReadLines(Level1EnemyCVS).ToArray();
        itemLines = File.ReadLines(Level1ItemCVS).ToArray();

        blockCsv = ParseCsvLines(blockLines);
        enemyCsv = ParseCsvLines(enemyLines);
        itemCsv = ParseCsvLines(itemLines);
    }

    public void Update(GameTime gt)
    {
        UpdateCheckpoints();
        UpdateEnemies(gt);
        UpdateItems(gt);
        UpdateBlocks(gt);
    }

    public void Draw(SpriteBatch sb) { }

    public void AddItem(IItems item)
    {
        for (int r = 0; r < levHeight; r++)
        {
            for (int c = 0; c < levWidth; c++)
            {
                if (items[r][c] != null)
                    continue;

                items[r][c] = item;
                CollisionController.Instance.AddItem(item);
                return;
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

        backgroundSprite?.Drop();
        backgroundSprite = null;

        HUD.Instance.LevelOver();
        SoundController.StopMusic();
    }

    protected void LoadContent()
    {
        if (BackgroundName != null)
        {
            LoadBackground();
        }

        _pipeResolver.Clear();

        ForEachCell((row, col) =>
        {
            LevelCellToken token = LevelCellTokenParser.ParseBlockToken(blockCsv[row][col]);
            string itemId = itemCsv[row][col];
            string enemyId = enemyCsv[row][col];

            LoadItemCell(token, itemId, row, col);
            LoadBlockCell(token, itemId, row, col);
            LoadEnemyCell(enemyId, row, col);
        });

        _pipeResolver.FinalizeLinks(blocks, tileSize);
    }

    protected void LoadBackground()
    {
        Texture2D backgroundTexture = MagicBrosMario.INSTANCE.Content.Load<Texture2D>(
            "LevelData/" + Name + "/" + BackgroundName);

        backWidth = backgroundTexture.Width;
        backHeight = backgroundTexture.Height;

        SharedTexture backgroundSharedTexture = new SharedTexture();
        backgroundSharedTexture.BindTexture(backgroundTexture);

        backgroundSprite = backgroundSharedTexture.NewSprite(0, 0, backWidth, backHeight);
        backgroundSprite.Scale = (float)tileSize / _blockSize;
        backgroundSprite.Visible = true;
        backgroundSprite.Background();
    }

    private void InitializeLevelState()
    {
        SoundController.PlayMusic(backgroundMusic, volume);
        HUD.Instance.LevelStart();
        HUD.Instance.SetTime(TimeLimit);
    }

    private void EnsureCsvArraysAreLoaded()
    {
        if (blockCsv == null && blockLines != null)
        {
            blockCsv = ParseCsvLines(blockLines);
        }

        if (enemyCsv == null && enemyLines != null)
        {
            enemyCsv = ParseCsvLines(enemyLines);
        }

        if (itemCsv == null && itemLines != null)
        {
            itemCsv = ParseCsvLines(itemLines);
        }

        if (blockCsv == null || enemyCsv == null || itemCsv == null)
        {
            throw new Exception("Level CSV data has not been loaded. Call ReadFromCSV before Initialize.");
        }
    }

    private static string[][] ParseCsvLines(string[] lines)
    {
        return lines
            .Select(line => line.Split(',').Select(cell => cell.Trim()).ToArray())
            .ToArray();
    }

    private void ValidateCsvDimensions()
    {
        levHeight = blockCsv.Length;

        if (levHeight == 0)
        {
            throw new Exception("Level: Block CSV file is empty");
        }

        levWidth = blockCsv[0].Length;

        if (levWidth == 0)
        {
            throw new Exception("Level: Block CSV file has no columns");
        }

        ValidateMatchingHeight(enemyCsv, "Enemy");
        ValidateMatchingHeight(itemCsv, "Item");

        ValidateMatchingWidth(blockCsv, "Block");
        ValidateMatchingWidth(enemyCsv, "Enemy");
        ValidateMatchingWidth(itemCsv, "Item");
    }

    private void ValidateMatchingHeight(string[][] csv, string csvName)
    {
        if (csv.Length != levHeight)
        {
            throw new Exception(
                $"Level: {csvName} CSV file has different height than Block CSV file. Expected {levHeight}, got {csv.Length}");
        }
    }

    private void ValidateMatchingWidth(string[][] csv, string csvName)
    {
        for (int row = 0; row < csv.Length; row++)
        {
            if (csv[row].Length != levWidth)
            {
                throw new Exception(
                    $"Level: {csvName} CSV row {row} has different width than Block CSV file. Expected {levWidth}, got {csv[row].Length}");
            }
        }
    }

    private void InitializeLevelArrays()
    {
        blocks = CreateGrid<IBlock>();
        enemies = CreateGrid<IEnemy>();
        items = CreateGrid<IItems>();
    }

    private T[][] CreateGrid<T>()
    {
        T[][] grid = new T[levHeight][];

        for (int row = 0; row < levHeight; row++)
        {
            grid[row] = new T[levWidth];
        }

        return grid;
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

    private void ForEachCell(Action<int, int> action)
    {
        for (int row = 0; row < levHeight; row++)
        {
            for (int col = 0; col < levWidth; col++)
            {
                action(row, col);
            }
        }
    }

    private void UpdateCheckpoints()
    {
        for (int i = checkpointPositions.Count - 1; i >= 0; i--)
        {
            if (MagicBrosMario.INSTANCE.Mario.Position.X <= checkpointPositions[i].X)
                continue;

            MagicBrosMario.INSTANCE.MarioStartPosition = checkpointPositions[i];
            checkpointPositions.RemoveAt(i);
        }
    }

    private void UpdateEnemies(GameTime gt)
    {
        Rectangle activationBounds = GetEnemyActivationBounds();

        ForEachCell((row, col) =>
        {
            IEnemy enemy = enemies[row][col];

            if (enemy == null || !IsEnemyActive(enemy, activationBounds))
                return;

            if (_activatedEnemies.Add(enemy))
            {
                CollisionController.Instance.AddEnemy(enemy);
            }

            enemy.Update(gt);
        });
    }

    private void UpdateItems(GameTime gt)
    {
        ForEachCell((row, col) =>
        {
            IItems item = items[row][col];

            if (item == null)
                return;

            item.Update(gt);

            if (!item.getCollected())
                return;

            CollisionController.Instance.RemoveItem(item);
            items[row][col] = null;
        });
    }

    private void UpdateBlocks(GameTime gt)
    {
        ForEachCell((row, col) => blocks[row][col]?.Update(gt));
    }

    private bool IsEnemyActive(IEnemy enemy, Rectangle activationBounds)
    {
        return enemy.AlwaysActive
            || _activatedEnemies.Contains(enemy)
            || enemy.CollisionBox.Intersects(activationBounds);
    }

    private void LoadItemCell(LevelCellToken token, string itemId, int row, int col)
    {
        if (string.IsNullOrEmpty(itemId))
        {
            items[row][col] = null;
            return;
        }

        if (!string.IsNullOrEmpty(token.BlockId))
            return;

        int? group = null;

        if (itemId == AxeItemId)
        {
            bridgeGroupCounter++;
            bridgeOrderCounter = 1;
            group = bridgeGroupCounter;
        }

        items[row][col] = ItemManager.CreateItem(itemId, col * tileSize, row * tileSize, group);
    }

    private void LoadBlockCell(LevelCellToken token, string itemId, int row, int col)
    {
        if (string.IsNullOrEmpty(token.BlockId))
        {
            LoadEmptyBlockCell(token, row, col);
            return;
        }

        if (!string.IsNullOrEmpty(token.PipeLabel))
        {
            _pipeResolver.RegisterPipeHalf(token.BlockId, row, col, token.PipeLabel);
            return;
        }

        CreateRegularBlock(token, itemId, row, col);
    }

    private void LoadEmptyBlockCell(LevelCellToken token, int row, int col)
    {
        blocks[row][col] = null;

        if (!string.IsNullOrEmpty(token.PipeLabel))
        {
            _pipeResolver.RegisterMarker(row, col, token.PipeLabel);
        }
    }

    private void CreateRegularBlock(LevelCellToken token, string itemId, int row, int col)
    {
        GetBridgeData(token.BlockId, row, col, out int? group, out int? order);

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

    private void GetBridgeData(string blockId, int row, int col, out int? group, out int? order)
    {
        group = null;
        order = null;

        if (blockId != BridgeBlockId)
            return;

        bool hasBridgeOnLeft = col > 0 && blocks[row][col - 1] is BridgeBlock;

        if (hasBridgeOnLeft)
        {
            bridgeOrderCounter++;
        }

        group = bridgeGroupCounter;
        order = bridgeOrderCounter;
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
}
