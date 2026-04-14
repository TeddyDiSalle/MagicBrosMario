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

    private readonly int _blockSize = 16;
    private readonly int _scale = 2;
    private int tileSize;

    protected string[] blockLines;
    protected string[] enemyLines;
    protected string[] itemLines;

    private int levWidth;
    private int levHeight;

    public int MarioStartPosX { get; protected set; }
    public int MarioStartPosY { get; protected set; }
    public string Name { get; protected set; }
    public int TimeLimit { get; protected set; }

    private readonly DeferredPipeLinkResolver _pipeResolver = new();
    private readonly HashSet<IEnemy> _activatedEnemies = new();

    public void Initialize(
        Microsoft.Xna.Framework.Content.ContentManager contentManager,
        Texture2D bTexture,
        Texture2D eTexture,
        Texture2D iTexture)
    {
        tileSize = _blockSize * _scale;

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
            throw new Exception("Level: Enemy or Item CSV file has different width than Block CSV file");
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

        ISprite backgroundSprite = backgroundSharedTexture.NewSprite(0, 0, backWidth, backHeight);
        backgroundSprite.Scale = (float)tileSize / _blockSize;
        backgroundSprite.Visible = true;
    }

    public void Update(GameTime gt)
    {
        Rectangle activationBounds = GetEnemyActivationBounds();

        for (int r = 0; r < levHeight; r++)
        {
            for (int c = 0; c < levWidth; c++)
            {
                if (enemies[r][c] != null)
                {
                    IEnemy enemy = enemies[r][c];

                    if (_activatedEnemies.Contains(enemy) || ShouldActivateEnemy(enemy, activationBounds))
                    {
                        _activatedEnemies.Add(enemy);
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

    public void Draw(SpriteBatch sb)
    {
    }

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
            items[row][col] = ItemManager.CreateItem(itemId, col * tileSize, row * tileSize);
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

        blocks[row][col] = BlockManager.CreateBlock(
            token.BlockId,
            col * tileSize,
            row * tileSize,
            LevelCellTokenParser.ToQuestionBlockItem(itemId),
            null,
            tileSize
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
        CollisionController.Instance.AddEnemy(enemies[row][col]);
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

        for (int r = 0; r < levHeight; r++)
        {
            for (int c = 0; c < levWidth; c++)
            {
                if (blocks[r][c] != null)
                {
                    CollisionController.Instance.RemoveBlock(blocks[r][c]);
                    blocks[r][c] = null;
                }
                if (enemies[r][c] != null)
                {
                    CollisionController.Instance.RemoveEnemy(enemies[r][c]);
                    enemies[r][c] = null;
                }
                if (items[r][c] != null)
                {
                    CollisionController.Instance.RemoveItem(items[r][c]);
                    items[r][c] = null;
                }
            }
        }

        HUD.Instance.LevelOver();
        SoundController.StopMusic();
    }
}