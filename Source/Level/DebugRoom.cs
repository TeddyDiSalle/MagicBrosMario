// Made By Teddy

namespace MagicBrosMario.Source.Level;

public class DebugRoom : ParentLevel
{
    public DebugRoom()	{

		//If you wish to edit the debug room for your own purposes
		// Look at Blocks, Items, or Enemies.xml in the LevelData folder for the correct object ids
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
	}
}