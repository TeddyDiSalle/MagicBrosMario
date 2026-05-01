using MagicBrosMario.Source.GameStates;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
using Microsoft.Xna.Framework;
using System;

namespace MagicBrosMario.Source.HUDAndScoring;

//Vincent Do
internal sealed class EventManager(HUD hud)
{
    private readonly int[] stompScores = { 100, 200, 400, 800, 1000, 2000, 4000, 8000 };
    private int StompChain = 0;
    private readonly double stompChainCD = 1.0;
    private double stompChainTimer = 0;
    public void KilledBowser()
    {
        hud.LevelOver();
        SendEvent(new GameEvent { EventType = GameEventType.EndOfLevel });
        MagicBrosMario.INSTANCE.Mario.Invincible = true;
        MagicBrosMario.INSTANCE.finishedLevel2 = true;
        SoundController.PlaySound(SoundType.BowserFires, 0.8f);
        SoundController.PlaySound(SoundType.WorldClear, 1.0f);
    }
    public void SendEvent(GameEvent gameEvent)
    {
        switch (gameEvent.EventType)
        {
            case GameEventType.StartLevel:
                SoundController.PlayMusic(MusicType.Level1_1, 1f);
                break;
            case GameEventType.EnemyStomped:
                SoundController.PlaySound(SoundType.Stomp, 1.0f);
                if (gameEvent.Data is Bowser)
                {
                    hud.Score+= 5000;
                    hud.DisplayScoreGain(gameEvent, 5000);
                    StompChain++;
                    KilledBowser();
                    return;
                }
                if (stompChainTimer >= stompChainCD)
                {
                    StompChain = 0;
                }
                stompChainTimer = 0;
                if (StompChain < stompScores.Length)
                {
                    hud.Score+= stompScores[StompChain];
                    hud.DisplayScoreGain(gameEvent, stompScores[StompChain]);
                    StompChain++;
                }
                else
                {
                    MagicBrosMario.INSTANCE.Mario.Lives++;
                    SoundController.PlaySound(SoundType.OneUp, 0.4f);
                }
                break;
            case GameEventType.LandedOnGround:
                if (MagicBrosMario.INSTANCE.Mario.GetCurrentPower() != Enums.Star)
                    StompChain = 0;
                break;
            case GameEventType.EnemyKilledByFireball:
                if (gameEvent.Data is not Bowser)
                {
                    hud.Score+= 100;
                    hud.DisplayScoreGain(gameEvent, 100);
                }
                else
                {
                    hud.Score+= 5000;
                    hud.DisplayScoreGain(gameEvent, 5000);
                    KilledBowser();
                }
                SoundController.PlaySound(SoundType.Stomp, 1.0f);
                break;
            case GameEventType.CoinCollected:
                hud.CoinCount++;
                hud.Score+= 200;
                hud.DisplayScoreGain(gameEvent, 200);
                SoundController.PlaySound(SoundType.Coin, 1.0f);
                if (hud.CoinCount == 100)
                {
                    hud.CoinCount = 0;
                    MagicBrosMario.INSTANCE.Mario.Lives++;
                    SoundController.PlaySound(SoundType.OneUp, 0.4f);
                }
                break;
            case GameEventType.BlockBroken:
                hud.Score+= 50;
                SoundController.PlaySound(SoundType.Break, 1.0f);
                hud.DisplayScoreGain(gameEvent, 50);
                break;
            case GameEventType.PowerupAppears:
                SoundController.PlaySound(SoundType.PowerUpAppear, 1.0f);
                hud.Score+= 1000;
                hud.DisplayScoreGain(gameEvent, 1000);
                break;
            case GameEventType.PowerupCollected:
                if (gameEvent.Data is not OneUp)
                {
                    SoundController.PlaySound(SoundType.PowerUp, 1.0f);
                    hud.Score+= 1000;
                    hud.DisplayScoreGain(gameEvent, 1000);
                }
                else
                {
                    SoundController.PlaySound(SoundType.OneUp, 1.0f);
                }
                break;
            case GameEventType.FlagpoleReached:
                SoundController.StopMusic();
                SoundController.PlaySound(SoundType.Flagpole, 1.0f);
                var (YContact, flagCollisionBox) = ((float, Rectangle))gameEvent.Data;
                float yContactDiff = (float)gameEvent.EventPosition.Y - YContact;
                float flagPoleHeight = flagCollisionBox.Height;
                float ratio = Math.Abs(yContactDiff / flagPoleHeight);

                gameEvent.EventPosition += new Point(0, -8);
                if (ratio <= 0.2f)
                {
                    hud.Score+= 5000;
                    hud.DisplayScoreGain(gameEvent, 5000);
                }
                else if (ratio <= 0.4f)
                {
                    hud.Score+= 2000;
                    hud.DisplayScoreGain(gameEvent, 2000);
                }
                else if (ratio <= 0.6f)
                {
                    hud.Score+= 1000;
                    hud.DisplayScoreGain(gameEvent, 1000);
                }
                else if (ratio <= 0.8f)
                {
                    hud.Score+= 500;
                    hud.DisplayScoreGain(gameEvent, 500);
                }
                else
                {
                    hud.Score+= 100;
                    hud.DisplayScoreGain(gameEvent, 100);
                }
                break;
            case GameEventType.EndOfLevel:
                hud.LevelOverState = true;
                SoundController.StopMusic();
                break;
            case GameEventType.Death:
                hud.Dead = true;
                hud.GoToTransition = true;
                break;
            default:
                break;
        }
    }

    public void Update(GameTime gametime)
    {
        stompChainTimer += gametime.ElapsedGameTime.TotalSeconds;
        if (hud.time == 100 && hud.playtimewarning && !hud.levelOver)
        {
            hud.playtimewarning = false;
            SoundController.PauseMusic();
            SoundController.PlaySound(SoundType.TimeWarning, 1.0f);
        }
        else if (!hud.playtimewarning && !SoundController.IsSoundOnCoolDown(SoundType.TimeWarning))
        {
            SoundController.ResumeMusic();
        }
        if (hud.levelOver && !hud.dead && !hud.GoToTransition)
        {
            hud.time--;
            hud.Score += 50;
            if (hud.time == 0)
            {
                hud.WaitForNextLevel = true;
                hud.GoToTransition = true;
            }
        }
        else if (hud.time == 0 && !hud.levelOver) { MagicBrosMario.INSTANCE.Mario.KillMario(); }

        if (hud.GoToTransition)
        {
            MagicBrosMario.INSTANCE.Mario.SetVisibility(false);
            MarioGameController.Mute();
            hud.TransitionTimer -= (float)gametime.ElapsedGameTime.TotalSeconds;

            if (hud.TransitionTimer <= 0)
            {
                if (!MagicBrosMario.INSTANCE.finishedLevel1)
                {
                    MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level.Level1());
                }
                else if (!MagicBrosMario.INSTANCE.finishedLevel2)
                {
                    MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level.Level2());
                }
                else
                {
                    MagicBrosMario.INSTANCE.CurrentState = new TitleScreenState();
                }
                hud.GoToTransition = false;
                hud.TransitionTimer = 3f;
            }
        }
    }
}
