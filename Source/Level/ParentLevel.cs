// Made By Teddy
using System;
using System.IO;
using System.Linq;
using MagicBrosMario.Source.Block; 
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.HUDAndScoring;
using System.Collections.Generic;

namespace MagicBrosMario.Source.Level;
public abstract class ParentLevel : ILevel
{
	static bool _ManagersIntialized = false;
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
	private int _blockSize = 16;
	private int _scale = 2;
	private int tileSize; // _blockSize * _scale
	protected string[] blockLines;
	protected string[] enemyLines;
	protected string[] itemLines;
	private int levWidth;
	private int levHeight;
	public int MarioStartPosX {get; protected set; }
	public int MarioStartPosY {get; protected set; }
	public String Name {get; protected set; }
	public int TimeLimit {get; protected set; }
	protected readonly Dictionary<string, BlockManager.PipeTeleport> PipeLinks = new();

	private sealed class PendingPipeEntrance
{
    public string Label { get; }
    public Dictionary<string, Point> HalvesByBlockId { get; } = new();

    public PendingPipeEntrance(string label)
    {
        Label = label;
    }
}

private sealed class PendingPipePair
{
    public PendingPipeEntrance End0 { get; set; }
    public PendingPipeEntrance End1 { get; set; }
}

private readonly Dictionary<string, PendingPipeEntrance> _pendingPipeEntrances = new();

    public void Initialize(Microsoft.Xna.Framework.Content.ContentManager contentManager, Texture2D bTexture, Texture2D eTexture, Texture2D iTexture)	{
		tileSize = _blockSize * _scale;

		levHeight = blockLines.Length;
		if(enemyLines.Length != levHeight || itemLines.Length != levHeight)	{
			throw new Exception("Level1: Enemy or Item CSV file has different height than Block CSV file");
		}
		levWidth = blockLines[0].Split(',').Length;
		if(enemyLines[0].Split(',').Length != levWidth || itemLines[0].Split(',').Length != levWidth)	{
			throw new Exception("Level1: Enemy or Item CSV file has different width than Block CSV file");
		}
		
		blocks = new IBlock[levHeight][];
		enemies = new IEnemy[levHeight][];
		items = new IItems[levHeight][];
		for(int i = 0; i < levHeight; i++)	{
			blocks[i] = new IBlock[levWidth];
			enemies[i] = new IEnemy[levWidth];
			items[i] = new IItems[levWidth];

		}

		SoundController.PlayMusic(backgroundMusic, volume);

		HUD.Instance.LevelStart();
        HUD.Instance.SetTime(TimeLimit);

		InitializeManagers(bTexture, eTexture, iTexture);
		LoadContent();
	}

	private static void InitializeManagers(Texture2D bTexture, Texture2D eTexture, Texture2D iTexture)	{
		if(_ManagersIntialized)	{
			return;
		}		
		_ManagersIntialized = true;
		BlockManager.Initialize(bTexture);
		EnemyManager.Initialize(eTexture);
		ItemManager.Initialize(iTexture);
	}

// if you want to read from csv, must be before Initialize
    public void ReadFromCSV()	{
        blockLines = File.ReadLines(Level1BlockCVS).ToArray();
		enemyLines = File.ReadLines(Level1EnemyCVS).ToArray();
		itemLines = File.ReadLines(Level1ItemCVS).ToArray();
    }

    protected void LoadBackground()	{
    
        Texture2D backgroundTexture = MagicBrosMario.INSTANCE.Content.Load<Texture2D>("LevelData/"+Name+"/"+BackgroundName);
        backWidth = backgroundTexture.Width;
        backHeight = backgroundTexture.Height;
        SharedTexture backgroundSharedTexture = new SharedTexture();
        backgroundSharedTexture.BindTexture(backgroundTexture);
        ISprite backgroundSprite = backgroundSharedTexture.NewSprite(0, 0, backWidth, backHeight);
        backgroundSprite.Scale = (float)tileSize / _blockSize; // scale background to match block size
        backgroundSprite.Visible = true;
		//TODO
    }
	public void Update(GameTime gt)	{
		//block update
		for(int r = 0; r < levHeight; r++)	{
			for(int c = 0; c < levWidth; c++)	{
				//if(blocks[r][c] != null)
					//blocks[r][c].Update(gt);
				if(enemies[r][c] != null)
					enemies[r][c].Update(gt);
				if (items[r][c] != null){
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

	public void Draw(SpriteBatch sb) {
		//block draw
		for(int r = 0; r < levHeight; r++)	{
			for(int c = 0; c < levWidth; c++)	{
				//if(blocks[r][c] != null)
					//blocks[r][c].Draw(sb);
			}
		}
	}

	protected void LoadContent(){
		if(BackgroundName != null)	{
            LoadBackground();
        }
		// TODO
        _pendingPipeEntrances.Clear();

        for (int r = 0; r < levHeight; r++)
        {
            string[] blockIds = blockLines[r].Split(',');
            string[] enemyIds = enemyLines[r].Split(',');
            string[] itemIds = itemLines[r].Split(',');

            for (int c = 0; c < levWidth; c++)
            {
                string rawBlockToken = blockIds[c].Trim();
                string enemyId = enemyIds[c].Trim();
                string itemId = itemIds[c].Trim();

                var (blockId, pipeLabel) = ParseBlockToken(rawBlockToken);

                if (string.IsNullOrEmpty(itemId))
                {
                    items[r][c] = null;
                }
                else
                {
                    if (string.IsNullOrEmpty(blockId))
                    {
                        items[r][c] = ItemManager.CreateItem(itemId, c * tileSize, r * tileSize);
                        // CollisionController.Instance.AddItem(items[r][c]);
                    }
                }

                if (string.IsNullOrEmpty(blockId))
                {
                    blocks[r][c] = null;
                }
                else
                {
                    // If this is a tagged pipe entry tile, defer it until all blocks are scanned
                    if (!string.IsNullOrEmpty(pipeLabel))
                    {
                        if (!IsPipeEntryBlockId(blockId))
                        {
                            throw new Exception(
                                $"Block token '{rawBlockToken}' uses a pipe label, " +
                                $"but block id '{blockId}' is not a pipe entry block.");
                        }

                        RegisterPendingPipeHalf(blockId, r, c, pipeLabel);
                    }
                    else
                    {
                        QuestionMarkBlock.InnerItem qItem =
                            itemId is "00" ? QuestionMarkBlock.InnerItem.Coin :
                            itemId is "01" ? QuestionMarkBlock.InnerItem.Mushroom :
                            itemId is "02" ? QuestionMarkBlock.InnerItem.Star :
                            itemId is "03" ? QuestionMarkBlock.InnerItem.OneUp :
                            itemId is "04" ? QuestionMarkBlock.InnerItem.Mushroom :
                            QuestionMarkBlock.InnerItem.Coin;

                        blocks[r][c] = BlockManager.CreateBlock(
                            blockId,
                            c * tileSize,
                            r * tileSize,
                            qItem,
                            null,
                            tileSize
                        );

                        CollisionController.Instance.AddBlock(blocks[r][c]);
                    }
                }

                if (string.IsNullOrEmpty(enemyId))
                {
                    enemies[r][c] = null;
                }
                else
                {
                    enemies[r][c] = EnemyManager.CreateEnemy(enemyId, c * tileSize, r * tileSize);
                    CollisionController.Instance.AddEnemy(enemies[r][c]);
                }
            }
        }

        FinalizePendingPipeEntrances();

	}

	public void AddItem(IItems item)
	{
		// Find the first empty slot in the items array and add the item there
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
	public void Clear()	{
		// Clears enemies, blocks, and items from the level and collision controller
		for(int r = 0; r < levHeight; r++)	{
			for(int c = 0; c < levWidth; c++)	{
				if(blocks[r][c] != null)	{
					CollisionController.Instance.RemoveBlock(blocks[r][c]);
					blocks[r][c] = null;
				}
				if(enemies[r][c] != null)	{
					CollisionController.Instance.RemoveEnemy(enemies[r][c]);
					enemies[r][c] = null;
				}
				if(items[r][c] != null)	{
					CollisionController.Instance.RemoveItem(items[r][c]);
					items[r][c] = null;
				}
			}
		}
		// Stop our music
		HUD.Instance.LevelOver();
		SoundController.StopMusic();
	}


    private static (string blockId, string? pipeLabel) ParseBlockToken(string token)
    {
        token = token.Trim();

        if (string.IsNullOrEmpty(token))
            return ("", null);

        int open = token.IndexOf('[');
        if (open < 0)
            return (token, null);

        int close = token.IndexOf(']', open + 1);
        if (close < 0)
            throw new FormatException($"Invalid block token '{token}'. Expected format like 10[A0].");

        string blockId = token.Substring(0, open).Trim();
        string pipeLabel = token.Substring(open + 1, close - open - 1).Trim();

        return (blockId, pipeLabel);
    }

    private static bool IsPipeEntryBlockId(string blockId)
    {
        return blockId is "10" or "11" or "14" or "15";
    }

    private void RegisterPendingPipeHalf(string blockId, int row, int col, string pipeLabel)
    {
        if (!_pendingPipeEntrances.TryGetValue(pipeLabel, out var entrance))
        {
            entrance = new PendingPipeEntrance(pipeLabel);
            _pendingPipeEntrances[pipeLabel] = entrance;
        }

        if (entrance.HalvesByBlockId.ContainsKey(blockId))
        {
            throw new Exception($"Pipe label '{pipeLabel}' has duplicate block id '{blockId}'.");
        }

        entrance.HalvesByBlockId[blockId] = new Point(col, row); // tile coordinates
    }

    private static void ParsePipeLabel(string pipeLabel, out string group, out int endpoint)
    {
        if (string.IsNullOrWhiteSpace(pipeLabel) || pipeLabel.Length < 2)
            throw new Exception($"Invalid pipe label '{pipeLabel}'. Use labels like A0 or B1.");

        char last = pipeLabel[^1];
        if (last != '0' && last != '1')
            throw new Exception($"Invalid pipe label '{pipeLabel}'. Final character must be 0 or 1.");

        group = pipeLabel.Substring(0, pipeLabel.Length - 1);
        endpoint = last - '0';
    }

        private static bool IsUpPipeEntrance(PendingPipeEntrance entrance)
        {
            return entrance.HalvesByBlockId.ContainsKey("10") && entrance.HalvesByBlockId.ContainsKey("11");
        }

        private static bool IsLeftPipeEntrance(PendingPipeEntrance entrance)
        {
            return entrance.HalvesByBlockId.ContainsKey("14") && entrance.HalvesByBlockId.ContainsKey("15");
        }

    private static void ValidateEntrance(PendingPipeEntrance entrance)
    {
        bool upPipe = IsUpPipeEntrance(entrance);
        bool leftPipe = IsLeftPipeEntrance(entrance);

        if (upPipe && entrance.HalvesByBlockId.Count != 2)
            throw new Exception($"Pipe '{entrance.Label}' must contain exactly 10 and 11.");

        if (leftPipe && entrance.HalvesByBlockId.Count != 2)
            throw new Exception($"Pipe '{entrance.Label}' must contain exactly 14 and 15.");

        if (!upPipe && !leftPipe)
        {
            throw new Exception(
                $"Pipe '{entrance.Label}' is invalid. " +
                $"It must contain either [10,11] or [14,15].");
        }
    }

    private static Point GetAnchorTile(PendingPipeEntrance entrance)
    {
        // top-left tile of the two-tile entrance
        int minX = int.MaxValue;
        int minY = int.MaxValue;

        foreach (Point p in entrance.HalvesByBlockId.Values)
        {
            if (p.X < minX) minX = p.X;
            if (p.Y < minY) minY = p.Y;
        }

        return new Point(minX, minY);
    }

    private static PipeEntryBlock.PipeDirection GetExitDirectionFromDestination(PendingPipeEntrance destination)
    {
        if (IsUpPipeEntrance(destination))
            return PipeEntryBlock.PipeDirection.Up;

        if (IsLeftPipeEntrance(destination))
            return PipeEntryBlock.PipeDirection.Left;

        throw new Exception($"Could not determine exit direction for pipe '{destination.Label}'.");
    }

    private void BuildPipeEntranceBlocks(PendingPipeEntrance source, PendingPipeEntrance destination)
    {
        Point exitTile = GetAnchorTile(destination);
        PipeEntryBlock.PipeDirection exitDirection = GetExitDirectionFromDestination(destination);

        BlockManager.PipeTeleport teleport = new BlockManager.PipeTeleport(exitTile, exitDirection);

        foreach (var kvp in source.HalvesByBlockId)
        {
            string blockId = kvp.Key;
            Point tile = kvp.Value;

            IBlock block = BlockManager.CreateBlock(
                blockId,
                tile.X * tileSize,
                tile.Y * tileSize,
                QuestionMarkBlock.InnerItem.Coin,
                teleport,
                tileSize
            );

            blocks[tile.Y][tile.X] = block;
            CollisionController.Instance.AddBlock(block);
        }
    }

    private void FinalizePendingPipeEntrances()
    {
        var grouped = new Dictionary<string, PendingPipePair>();

        foreach (var entrance in _pendingPipeEntrances.Values)
        {
            ValidateEntrance(entrance);

            ParsePipeLabel(entrance.Label, out string group, out int endpoint);

            if (!grouped.TryGetValue(group, out var pair))
            {
                pair = new PendingPipePair();
                grouped[group] = pair;
            }

            if (endpoint == 0)
            {
                if (pair.End0 != null)
                    throw new Exception($"Pipe group '{group}' has more than one 0 endpoint.");
                pair.End0 = entrance;
            }
            else
            {
                if (pair.End1 != null)
                    throw new Exception($"Pipe group '{group}' has more than one 1 endpoint.");
                pair.End1 = entrance;
            }
        }

        foreach (var kvp in grouped)
        {
            string group = kvp.Key;
            PendingPipePair pair = kvp.Value;

            if (pair.End0 == null || pair.End1 == null)
            {
                throw new Exception(
                    $"Pipe group '{group}' is incomplete. " +
                    $"Both {group}0 and {group}1 must exist.");
            }

            // 0 goes to 1
            BuildPipeEntranceBlocks(pair.End0, pair.End1);

            // 1 goes to 0
            BuildPipeEntranceBlocks(pair.End1, pair.End0);
        }

        _pendingPipeEntrances.Clear();
    }
}