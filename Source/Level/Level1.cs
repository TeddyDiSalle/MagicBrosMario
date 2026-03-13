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
namespace MagicBrosMario.Source.Level;
public class Level1 : ILevel
{
	private IBlock[][] blocks;
	private IEnemy[][] enemies;
	private IItems[][] items;
	private string Level1BlockCVS = "Content/LevelData/1-1/Blocks1-1.csv";
	private string Level1EnemyCVS = "Content/LevelData/1-1/Enemies1-1.csv";
	private string Level1ItemCVS = "Content/LevelData/1-1/Items1-1.csv";
	private static int _blockSize = 16;
	private static int _scale = 2;
	private int tileSize = _blockSize * _scale;
	private string[] blockLines;
	private string[] enemyLines;
	private string[] itemLines;
	private int levWidth;
	private int levHeight;
	public Level1()	{
		blockLines = File.ReadLines(Level1BlockCVS).ToArray();
		enemyLines = File.ReadLines(Level1EnemyCVS).ToArray();
		itemLines = File.ReadLines(Level1ItemCVS).ToArray();

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
		
	}
	public void Initialize(Texture2D bTexture, Texture2D eTexture, Texture2D iTexture)	{
		BlockManager.Initialize(bTexture);
		EnemyManager.Initialize(eTexture);
		ItemManager.Initialize(iTexture);
		LoadContent();
	}
	public void Update(GameTime gt)	{
		//block update
		for(int r = 0; r < levHeight; r++)	{
			for(int c = 0; c < levWidth; c++)	{
				//if(blocks[r][c] != null)
					//blocks[r][c].Update(gt);
				if(enemies[r][c] != null)
					enemies[r][c].Update(gt);
				if(items[r][c] != null)
					items[r][c].Update(gt);
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

	private void LoadContent(){
		
		for (int r = 0; r < levHeight; r++){
			string[] blockIds = blockLines[r].Split(',');
			string[] enemyIds = enemyLines[r].Split(',');
			string[] itemIds = itemLines[r].Split(',');

			for (int c = 0; c < levWidth; c++){
				string blockId = blockIds[c].Trim();
				string enemyId = enemyIds[c].Trim();
				string itemId = itemIds[c].Trim();
				if (string.IsNullOrEmpty(blockId)){
					blocks[r][c] = null;
					
				}else{
					blocks[r][c] =   BlockManager.CreateBlock(blockId, c * tileSize, r * tileSize);// x,y - columnb => x, row => y

					CollisionController.Instance.AddBlock(blocks[r][c]);
					
				}

				if (string.IsNullOrEmpty(enemyId)){
					enemies[r][c] = null;
				}else{
					enemies[r][c] = EnemyManager.CreateEnemy(enemyId, c * tileSize, r * tileSize);
					CollisionController.Instance.AddEnemy(enemies[r][c]);
				}

				if (string.IsNullOrEmpty(itemId)){
					items[r][c] = null;
				}else{
					items[r][c] = ItemManager.CreateItem(itemId, c * tileSize, r * tileSize);
					CollisionController.Instance.AddItem(items[r][c]);
				}
			}
		}

		//JustTheFloor();
	}

	private void JustTheFloor(){
		int floorLevel = 10;
		for (int c = 0; c < levWidth; c++)
		{
			blocks[levHeight - floorLevel][c] =   BlockManager.CreateBlock("04", c * tileSize, (levHeight - floorLevel) * tileSize);
			Camera.Instance.Sprites.Add(blocks[levHeight - floorLevel][c].Sprite);
		}
		
	}
}