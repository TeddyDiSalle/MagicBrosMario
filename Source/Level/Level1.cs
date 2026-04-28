using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Level;
public class Level1 : ParentLevel
{
	
	static int NormalLevelHeight = 10*tileSize;
	Point spawnPoint = new Point(3*tileSize, NormalLevelHeight);
	Point Checkpoint1 = new Point(82*tileSize, NormalLevelHeight);
	//These other points are for debugging purposes
	//Point rightBeforeFlagpole = new Point(195*tileSize, NormalLevelHeight);
	//Point rightAfterFlagpole = new Point(200*tileSize, NormalLevelHeight);
	//Point secretRoom = new Point(230*tileSize, NormalLevelHeight);
	public Level1()	{
		MarioStartPos = spawnPoint;

		checkpointPositions.Add(Checkpoint1); // from the og game
		
		Name = "1-1";
		TimeLimit = 400;
		backgroundMusic = Sound.MusicType.Level1_1;
		
		Level1BlockCVS = "Content/LevelData/1-1/Blocks1-1.csv";
		Level1EnemyCVS = "Content/LevelData/1-1/Enemies1-1.csv";
		Level1ItemCVS = "Content/LevelData/1-1/Items1-1.csv";
		BackgroundName = "1-1LazyDebugBackground";
		ReadFromCSV();
		MagicBrosMario.INSTANCE.Mario.ChangeState(new SmallMarioIdleState(MagicBrosMario.INSTANCE.Mario));
	}
}