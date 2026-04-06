//Made by Teddy

namespace MagicBrosMario.Source.Sound;
public static class Music
{
    private static float _volume = 0.5f;
   private static SoundController INSTANCE = new SoundController();        
    public static void LoadMusic()
    {
        INSTANCE.LoadSound(SoundTypes.BackGroundMusic, "1-1/smb overworld.mp3");
    }

    
    public static void LoopMusic()
    {
        INSTANCE.sounds[SoundTypes.BackGroundMusic].Play(_volume, 0.0f, 0.0f);
    }
}