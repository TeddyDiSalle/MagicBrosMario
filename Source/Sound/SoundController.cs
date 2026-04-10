// wav files downloaded from https://themushroomkingdom.net/media/smb/wav
// made by teddy

using System;
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
        string L1Path = "1-1/";
        string BlocksPath = "Blocks/";
        string EnemiesPath = "Enemies/";
        string EnviromentPath = "Enviroment/";
        string ItemsPath = "Items/";
        string MarioPath = "Mario/";
        Instance.LoadMusic(MusicType.Level1_1, L1Path+"smb_overworld");

        Instance.LoadSoundEffect(SoundType.Break, BlocksPath+"break");
        Instance.LoadSoundEffect(SoundType.Vine, BlocksPath+"vine");
        
        Instance.LoadSoundEffect(SoundType.BowserFalls, EnemiesPath+"bowser_falls");
        Instance.LoadSoundEffect(SoundType.BowserFires, EnemiesPath+"bowser_fires");

        Instance.LoadSoundEffect(SoundType.Bump, EnviromentPath+"bump");
        Instance.LoadSoundEffect(SoundType.Fireworks, EnviromentPath+"fireworks");
        Instance.LoadSoundEffect(SoundType.Flagpole, EnviromentPath+"flagpole");
        Instance.LoadSoundEffect(SoundType.GameOver, EnviromentPath+"game_over");
        Instance.LoadSoundEffect(SoundType.Pause, EnviromentPath+"pause");
        Instance.LoadSoundEffect(SoundType.StageClear, EnviromentPath+"stage_clear");
        Instance.LoadSoundEffect(SoundType.TimeWarning, EnviromentPath+"time_warning");
        Instance.LoadSoundEffect(SoundType.WorldClear, EnviromentPath+"world_clear");

        Instance.LoadSoundEffect(SoundType.Coin, ItemsPath+"coin");
        Instance.LoadSoundEffect(SoundType.OneUp, ItemsPath+"one_up");
        Instance.LoadSoundEffect(SoundType.PowerUp, ItemsPath+"power_up");
        Instance.LoadSoundEffect(SoundType.PowerUpAppear, ItemsPath+"power_up_appear");
        
        Instance.LoadSoundEffect(SoundType.Fireball, MarioPath+"fireball");
        Instance.LoadSoundEffect(SoundType.JumpSmall, MarioPath+"jump_small");
        Instance.LoadSoundEffect(SoundType.JumpSuper, MarioPath+"jump_super");
        Instance.LoadSoundEffect(SoundType.Kick, MarioPath+"kick");
        Instance.LoadSoundEffect(SoundType.MarioDie, MarioPath+"mario_die");
        Instance.LoadSoundEffect(SoundType.PipeTravel, MarioPath+"pipe_travel_power_down");
        Instance.LoadSoundEffect(SoundType.PowerDown, MarioPath+"pipe_travel_power_down");
        Instance.LoadSoundEffect(SoundType.Stomp, MarioPath+"stomp");
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

    public static void PlayMusic(MusicType type, float volume = 0.5f)
    {
        var song = Instance.Musics[type];
        
        // Set whether the song should repeat when finished
        MediaPlayer.IsRepeating = true;
        // Adjust the volume (0.0f to 1.0f)
        MediaPlayer.Volume = volume;
        // Check if the media player is already playing, if so, stop it
        if(MediaPlayer.State == MediaState.Playing){
            MediaPlayer.Stop();
        }
        // Start playing the background music
        MediaPlayer.Play(song);
        //Instance.musicInstance.Add(type, null);
    }

    public static bool IsMusicPlaying(MusicType type)
    {
        //return Instance.musicInstance.ContainsKey(type);
        return MediaPlayer.State == MediaState.Playing;
    }

    public static void StopMusic(MusicType type)
    {
        if(MediaPlayer.State == MediaState.Playing){
            MediaPlayer.Stop();
        }
        //Instance.musicInstance.Remove(type);
    }
}