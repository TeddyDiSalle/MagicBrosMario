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
                    hud.KilledBowser();
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
                if (MagicBrosMario.INSTANCE.Mario.GetCurrentPower() != Power.Star)
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
                    hud.KilledBowser();
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

    public void UpdateStompChainTimer(GameTime gametime)
    {
        stompChainTimer += gametime.ElapsedGameTime.TotalSeconds;
    }
}
