// wav files downloaded from https://themushroomkingdom.net/media/smb/wav

namespace MagicBrosMario.Source.Sound;

public static class SoundEffectController
{
    private static SoundController INSTANCE = new SoundController();

    public static void LoadSounds()
    {
        INSTANCE.LoadSound(SoundTypes.StageClear, "stage_clear");
        INSTANCE.LoadSound(SoundTypes.WorldClear, "world_clear");
        INSTANCE.LoadSound(SoundTypes.TimeWarning, "time_warning");
        INSTANCE.LoadSound(SoundTypes.GameOver, "game_over");
        INSTANCE.LoadSound(SoundTypes.MarioDie, "mario_die");
        INSTANCE.LoadSound(SoundTypes.OneUp, "one_up");
        INSTANCE.LoadSound(SoundTypes.JumpSmall, "jump_small");
        INSTANCE.LoadSound(SoundTypes.JumpSuper, "jump_super");
        INSTANCE.LoadSound(SoundTypes.BowserFalls, "bowser_falls");
        INSTANCE.LoadSound(SoundTypes.BowserFires, "bowser_fires");
        INSTANCE.LoadSound(SoundTypes.Break, "break");
        INSTANCE.LoadSound(SoundTypes.Bump, "bump");
        INSTANCE.LoadSound(SoundTypes.Coin, "coin");
        INSTANCE.LoadSound(SoundTypes.Flagpole, "flagpole");
        INSTANCE.LoadSound(SoundTypes.Fireball, "fireball");
        INSTANCE.LoadSound(SoundTypes.Fireworks, "fireworks");
        INSTANCE.LoadSound(SoundTypes.Kick, "kick");
        INSTANCE.LoadSound(SoundTypes.PipeTravel, "pipe_travel_power_down");
        INSTANCE.LoadSound(SoundTypes.PowerDown, "pipe_travel_power_down");
        INSTANCE.LoadSound(SoundTypes.PowerUp, "power_up");
        INSTANCE.LoadSound(SoundTypes.PowerUpAppear, "power_up_appear");
        INSTANCE.LoadSound(SoundTypes.Stomp, "stomp");
        INSTANCE.LoadSound(SoundTypes.Vine, "vine");
    }


    public static void PlaySound(SoundTypes type)
    {
        INSTANCE.sounds[type].Play(1.0f, 0.0f, 0.0f);
    }

    public static void PlaySound(SoundTypes type, float volume)
    {
        INSTANCE.sounds[type].Play(volume, 0.0f, 0.0f);
    }
}