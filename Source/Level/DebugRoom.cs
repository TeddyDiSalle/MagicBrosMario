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
    public DebugRoom()	{
		blockLines = File.ReadLines(Level1BlockCVS).ToArray();
		enemyLines = File.ReadLines(Level1EnemyCVS).ToArray();
		itemLines = File.ReadLines(Level1ItemCVS).ToArray();

		
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