// wav files downloaded from https://themushroomkingdom.net/media/smb/wav
// made by teddy

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace MagicBrosMario.Source.Sound;

public class SoundController
{
    private static readonly SoundController Instance = new();

    private readonly Dictionary<SoundType, SoundEffect> SoundEffects = new();
    private readonly Dictionary<MusicType, Song> Musics = new();

    private void LoadSoundEffect(SoundType type, string soundName)
    {
        SoundEffects.Add(type, MagicBrosMario.INSTANCE.Content.Load<SoundEffect>("Sounds/" + soundName));
    }

    private void LoadMusic(MusicType type, string soundName)
    {
        Musics.Add(type, MagicBrosMario.INSTANCE.Content.Load<Song>("Sounds/" + soundName));
    }

    public static void LoadSounds()
    {
        Instance.LoadSoundEffect(SoundType.StageClear, "stage_clear");
        Instance.LoadSoundEffect(SoundType.WorldClear, "world_clear");
        Instance.LoadSoundEffect(SoundType.TimeWarning, "time_warning");
        Instance.LoadSoundEffect(SoundType.GameOver, "game_over");
        Instance.LoadSoundEffect(SoundType.MarioDie, "mario_die");
        Instance.LoadSoundEffect(SoundType.OneUp, "one_up");
        Instance.LoadSoundEffect(SoundType.JumpSmall, "jump_small");
        Instance.LoadSoundEffect(SoundType.JumpSuper, "jump_super");
        Instance.LoadSoundEffect(SoundType.BowserFalls, "bowser_falls");
        Instance.LoadSoundEffect(SoundType.BowserFires, "bowser_fires");
        Instance.LoadSoundEffect(SoundType.Break, "break");
        Instance.LoadSoundEffect(SoundType.Bump, "bump");
        Instance.LoadSoundEffect(SoundType.Coin, "coin");
        Instance.LoadSoundEffect(SoundType.Flagpole, "flagpole");
        Instance.LoadSoundEffect(SoundType.Fireball, "fireball");
        Instance.LoadSoundEffect(SoundType.Fireworks, "fireworks");
        Instance.LoadSoundEffect(SoundType.Kick, "kick");
        Instance.LoadSoundEffect(SoundType.PipeTravel, "pipe_travel_power_down");
        Instance.LoadSoundEffect(SoundType.PowerDown, "pipe_travel_power_down");
        Instance.LoadSoundEffect(SoundType.PowerUp, "power_up");
        Instance.LoadSoundEffect(SoundType.PowerUpAppear, "power_up_appear");
        Instance.LoadSoundEffect(SoundType.Stomp, "stomp");
        Instance.LoadSoundEffect(SoundType.Vine, "vine");

        Instance.LoadMusic(MusicType.Level1_1, "1-1/smb overworld");
    }

    public static void Update(GameTime gameTime)
    {
        var ms = gameTime.ElapsedGameTime.Milliseconds;

        foreach (var type in Instance.soundEffectCooldown.Keys)
        {
            Instance.soundEffectCooldown[type] -= ms;
            if (Instance.soundEffectCooldown[type] <= 0)
            {
                Instance.soundEffectCooldown.Remove(type);
            }
        }
    }

    private readonly Dictionary<SoundType, int> soundEffectCooldown = new();

    public static void PlaySound(SoundType type, float volume)
    {
        if (IsSoundOnCoolDown(type))
            return;

        var soundEffect = Instance.SoundEffects[type];
        soundEffect.Play(volume, 0.0f, 0.0f);
        Instance.soundEffectCooldown.Add(type, 50);
    }

    public static bool IsSoundOnCoolDown(SoundType type)
    {
        return Instance.soundEffectCooldown.ContainsKey(type);
    }

    private readonly Dictionary<MusicType, SoundEffectInstance> musicInstance = new();

    public static void PlayMusic(MusicType type, float volume)
    {
        //var soundInstance = Instance.Musics[type].CreateInstance();
        //soundInstance.IsLooped = true;
        //Instance.musicInstance.Add(type, soundInstance);
        MediaPlayer.Volume = volume;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(Instance.Musics[type]);
    }

    public static bool IsMusicPlaying(MusicType type)
    {
        //return Instance.musicInstance.ContainsKey(type);
        return MediaPlayer.State == MediaState.Playing;
    }

    public static void StopMusic(MusicType type)
    {
        //Instance.musicInstance[type].Stop();
        //Instance.musicInstance.Remove(type);
        MediaPlayer.Stop();
    }
}