using System.Runtime.ExceptionServices;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Level;
public class Level2 : ParentLevel
{
	
	static int NormalLevelHeight = 10*tileSize;
	Point spawnPoint = new Point(1*tileSize, NormalLevelHeight);
	Point firstCheckpoint = new Point(63*tileSize, NormalLevelHeight);
	Point secondCheckpoint = new Point(100*tileSize, NormalLevelHeight);
	Point thirdCheckpoint = new Point(161*tileSize, 0*tileSize);
	Point fourthCheckpoint = new Point(211*tileSize, NormalLevelHeight);
	Point fifthCheckpoint = new Point(311*tileSize, NormalLevelHeight);
	public Level2()	{
		MarioStartPos = fourthCheckpoint;
		
		checkpointPositions.Add(firstCheckpoint); // after Teddy's
		checkpointPositions.Add(secondCheckpoint); // after Roshan's, would also like to forever kill Meteor Mage after this point
		checkpointPositions.Add(thirdCheckpoint); // after Vincent's
		checkpointPositions.Add(fourthCheckpoint); // after Chuang's
		checkpointPositions.Add(fifthCheckpoint); // after Brian's

		Name = "1-2";
		TimeLimit = 500;
		backgroundMusic = Sound.MusicType.Level1_2;
		
		Level1BlockCVS = "Content/LevelData/1-2/Blocks1-2.csv";
		Level1EnemyCVS = "Content/LevelData/1-2/Enemies1-2.csv";
		Level1ItemCVS = "Content/LevelData/1-2/Items1-2.csv";
		//BackgroundName = "1-1LazyDebugBackground";
		ReadFromCSV();
	}
}