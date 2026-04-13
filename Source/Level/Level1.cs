
namespace MagicBrosMario.Source.Level;
public class Level1 : ParentLevel
{
	public Level1()	{
		MarioStartPosX = 100;
		MarioStartPosY = 300;
		Name = "1-1";
		TimeLimit = 400;
		backgroundMusic = Sound.MusicType.Level1_1;
		
		Level1BlockCVS = "Content/LevelData/1-1/Blocks1-1.csv";
		Level1EnemyCVS = "Content/LevelData/1-1/Enemies1-1.csv";
		Level1ItemCVS = "Content/LevelData/1-1/Items1-1.csv";
		BackgroundPath = "Content/LevelData/1-1/1-1LazyDebugBackground.png";
		ReadFromCSV();
	}
	

	//private void JustTheFloor(){
	//	int floorLevel = 10;
	//	for (int c = 0; c < levWidth; c++)
	//	{
	//		blocks[levHeight - floorLevel][c] =   BlockManager.CreateBlock("04", c * tileSize, (levHeight - floorLevel) * tileSize);
	//		Camera.Instance.Sprites.Add(blocks[levHeight - floorLevel][c].Sprite);
	//	}	
	//}
}