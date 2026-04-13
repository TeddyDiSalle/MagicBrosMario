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
	protected string BackgroundPath;
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

    protected void LoadBackground(SpriteBatch sb, Texture2D backgroundTexture)	{
        //sb.Draw(backgroundTexture, new Rectangle(0, 0, levWidth * tileSize, levHeight * tileSize), Color.White);
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
		//LoadBackground(Camera.Instance.SpriteBatch, MagicBrosMario.INSTANCE.Content.Load<Texture2D>(BackgroundPath));
		// TODO
		for (int r = 0; r < levHeight; r++){
			string[] blockIds = blockLines[r].Split(',');
			string[] enemyIds = enemyLines[r].Split(',');
			string[] itemIds = itemLines[r].Split(',');

			for (int c = 0; c < levWidth; c++){
				string blockId = blockIds[c].Trim();
				string enemyId = enemyIds[c].Trim();
				string itemId = itemIds[c].Trim();
				if (string.IsNullOrEmpty(itemId)){
					items[r][c] = null;
				}else{
					if(itemId == "00") // a coin is the only thing we place in the world right now, ?markblock takes care of the rest
					{
						items[r][c] = ItemManager.CreateItem(itemId, c * tileSize, r * tileSize);
						//CollisionController.Instance.AddItem(items[r][c]);
					}
				}
				
				if (string.IsNullOrEmpty(blockId)){
					blocks[r][c] = null;	
				}else{
					//have to translate our item type to QuestionMarkBlock.InnerItem enum
					QuestionMarkBlock.InnerItem qItem =itemId is "00" ? QuestionMarkBlock.InnerItem.Coin :
						itemId is "01" ? QuestionMarkBlock.InnerItem.Mushroom : // no fireflower in q block, but a mushroom is in
						itemId is "02" ? QuestionMarkBlock.InnerItem.Star :
						itemId is "03" ? QuestionMarkBlock.InnerItem.OneUp :
						itemId is "04" ? QuestionMarkBlock.InnerItem.Mushroom : // OneUp is not implemented yey
						QuestionMarkBlock.InnerItem.Coin; // Default to Coin if not matched
					
					blocks[r][c] =   BlockManager.CreateBlock(blockId, c * tileSize, r * tileSize, qItem);
					
					CollisionController.Instance.AddBlock(blocks[r][c]);
					
				}

				if (string.IsNullOrEmpty(enemyId)){
					enemies[r][c] = null;
				}else{
					enemies[r][c] = EnemyManager.CreateEnemy(enemyId, c * tileSize, r * tileSize);
					CollisionController.Instance.AddEnemy(enemies[r][c]);
				}
			}
		}

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
		SoundController.StopMusic();
	}
}