using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

namespace MagicBrosMario.Source.Sound;

public class SoundController
{
    public readonly Dictionary<SoundTypes, SoundEffect> sounds = new();
    public void LoadSound(SoundTypes type, string soundName)
    {
        sounds.Add(type, MagicBrosMario.INSTANCE.Content.Load<SoundEffect>("Sounds/"+soundName));
    }

}