using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace MagicBrosMario.Source.Sound;

public class SoundController
{
    private static SoundController INSTANCE = new SoundController();

    private readonly Dictionary<SoundTypes, SoundEffect> sounds = new();

    public static void LoadSounds()
    {
        INSTANCE.LoadSound(SoundTypes.OneUp, "one_up");
        INSTANCE.LoadSound(SoundTypes.JumpSmall, "jump_small");
        INSTANCE.LoadSound(SoundTypes.JumpSuper, "jump_super");
        INSTANCE.LoadSound(SoundTypes.GameOver, "game_over");
    }

    private void LoadSound(SoundTypes type, string soundName)
    {
        sounds.Add(type, MagicBrosMario.INSTANCE.Content.Load<SoundEffect>("Sounds/"+soundName));
    }

    public static void PlaySound(SoundTypes type, float volume)
    {
        INSTANCE.sounds[type].Play(volume, 0.0f, 0.0f);
    }
}