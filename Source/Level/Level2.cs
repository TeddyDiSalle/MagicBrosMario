namespace MagicBrosMario.Source.Level;
public class Level2 : ParentLevel
{
	public Level2()	{
		MarioStartPosX = 100;
		MarioStartPosY = 300;
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