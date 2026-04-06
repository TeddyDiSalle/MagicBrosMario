//Made by Teddy

namespace MagicBrosMario.Source.Sound;

public static class SoundEffectController
{
   private static SoundController INSTANCE = new SoundController();        
    public static void LoadSounds()
    {
        INSTANCE.LoadSound(SoundTypes.OneUp, "one_up");
        INSTANCE.LoadSound(SoundTypes.JumpSmall, "jump_small");
        INSTANCE.LoadSound(SoundTypes.JumpSuper, "jump_super");
        INSTANCE.LoadSound(SoundTypes.GameOver, "game_over");
    }

    
    public static void PlaySound(SoundTypes type, float volume)
    {
        INSTANCE.sounds[type].Play(volume, 0.0f, 0.0f);
    }
}