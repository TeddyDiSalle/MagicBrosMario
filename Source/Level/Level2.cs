using System.Runtime.ExceptionServices;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Level;
public class Level2 : ParentLevel
{
	Point spawnPoint = new Point(1*tileSize, 10*tileSize);
	Point firstCheckpoint = new Point(63*tileSize, 10*tileSize);
	Point secondCheckpoint = new Point(100*tileSize, 10*tileSize);
	Point thirdCheckpoint = new Point(161*tileSize, 0*tileSize);
	Point fourthCheckpoint = new Point(211*tileSize, 10*tileSize);
	public Level2()	{
		MarioStartPos = spawnPoint;
		
		checkpointPositions.Add(firstCheckpoint); // after Teddy's
		checkpointPositions.Add(secondCheckpoint); // after Roshan's
		checkpointPositions.Add(thirdCheckpoint); // after Vincent's
		checkpointPositions.Add(fourthCheckpoint); // after Chuang.s

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