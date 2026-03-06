// Made By Teddy
using System;
using System.IO;
using System.Linq;
using MagicBrosMario.Source.Block;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MagicBrosMario.Source.Level;
public class Level1 : ILevel
{
	private BlockManager _bm;
	private IBlock[][] blocks;
	private string Level1CSVPath = "Content/LevelData/1-1.csv";
	private int _blockSize = 16;
	private string[] lines;
	private int levWidth;
	private int levHeight;
	public Level1()	{
		lines = File.ReadLines(Level1CSVPath).ToArray();
		levHeight = lines.Length;
		levWidth = lines[0].Split(',').Length;
		blocks = new IBlock[levHeight][];
		for(int i = 0; i < levHeight; i++)	{
			blocks[i] = new IBlock[levWidth];
		}
	}
	public void Initialize(Texture2D texture)	{
		_bm = new BlockManager();
		_bm.Initialize(texture);
		LoadContent();
	}
	public void Update(GameTime gt)	{
		//block update
		for(int r = 0; r < levHeight; r++)	{
			for(int c = 0; c < levWidth; c++)	{
				if(blocks[r][c] != null)
					blocks[r][c].Update(gt);
			}
		}
	}

	public void Draw(SpriteBatch sb) {
		//block draw
		for(int r = 0; r < levHeight; r++)	{
			for(int c = 0; c < levWidth; c++)	{
				if(blocks[r][c] != null)
					blocks[r][c].Draw(sb);
			}
		}
	}

	private void LoadContent(){
		
		int tileSize = _blockSize * 4;

		for (int r = 0; r < levHeight; r++)
		{
			string[] blockIds = lines[r].Split(',');

			for (int c = 0; c < levWidth; c++)
			{
				string id = blockIds[c].Trim();

				if (string.IsNullOrEmpty(id))
				{
					blocks[r][c] = null;
					continue;
				}

				blocks[r][c] = _bm.CreateBlock(id, c * tileSize, r * tileSize);// x,y - columnb => x, row => y
			}
		}

		blocks[0][0] = _bm.CreateBlock("04", 0, 0);
	}
}