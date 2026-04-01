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

public class DebugRoom : ILevel
{
	private string[] blockLines;
	private string[] enemyLines;
	private string[] itemLines;
	private int levWidth;
	private int levHeight;
	private IBlock[][] blocks;
	private IEnemy[][] enemies;
	private IItems[][] items;
    public DebugRoom()	{
		blockLines =  ["  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,04,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,02,  ,  ,07,  ,07,  ",
					   "  ,  ,  ,04,  ,  ,  ,02,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,02,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,02,  ,  ,  ,  ,  ,02",
					   "02,02,02,02,02,02,02,02,02,02,02,02,02,02",];

		enemyLines =  ["  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,00,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,01,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",];

		itemLines =  ["  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,00,  ,  ,  ", // one coin outside a mystery block
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,00,  ,01,  ", // coin inside a mystery block and mush/ff in a mb
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",
					   "  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ,  ",];

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
	public void Initialize(Microsoft.Xna.Framework.Content.ContentManager contentManager, Texture2D tilesetTexture, Texture2D characterTexture, Texture2D itemTexture)
	{
	}

	public void Update(GameTime gameTime)
	{
	}

	public void Draw(SpriteBatch spriteBatch)
	{
	}
}