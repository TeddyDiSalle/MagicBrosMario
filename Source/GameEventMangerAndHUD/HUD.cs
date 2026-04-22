using MagicBrosMario.Source.GameStates;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
namespace MagicBrosMario.Source.HUDAndScoring;

//Vincent Do
public class HUD
{
    private readonly SpriteFont font = MagicBrosMario.INSTANCE.font;
    private readonly AnimatedSprite coin = new(MagicBrosMario.INSTANCE.ItemTexture, 192, 64, 10, 14, 4, 0.15f);
    private int score = 0;
    private int coinCount = 82; //For testing and functionality check in
    private int time = 150;
    private int FrameCount = 0;
    private bool levelOver = false;
    private bool goToTransition = false;
    private float TransitionTimer = 3f;
    public bool dead { get; private set; } = false;
    public bool waitForNextLevel { get; private set; } = false;
    private readonly int[] stompScores = { 100, 200, 400, 800, 1000, 2000, 4000, 8000 };
    private int StompChain = 0;
    private readonly double stompChainCD = 1.0;
    private double stompChainTimer = 0;
    private bool playtimewarning = true;
    public static HUD Instance { get; } = new();
    private List<FloatingText> textsList = [];

    public void LevelOver()
    {
        levelOver = true;
        waitForNextLevel = true;
    }
    public void LevelStart()
    {
        levelOver = false;
        waitForNextLevel = false;
        dead = false;
    }
    public void SetTime(int time)
    {
        this.time = time;
    }
    public void DisplayScoreGain(GameEvent gameEvent, int num)
    {
        FloatingText text = new(gameEvent, num);
        textsList.Add(text);
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
                if (stompChainTimer >= stompChainCD)
                {
                    StompChain = 0;
                }
                stompChainTimer = 0;
                if (StompChain < stompScores.Length)
                {
                    score += stompScores[StompChain];
                    DisplayScoreGain(gameEvent, stompScores[StompChain]);
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
                    score += 100;
                    DisplayScoreGain(gameEvent, 100);
                }
                else
                {
                    score += 5000;
                    DisplayScoreGain(gameEvent, 5000);
                }
                SoundController.PlaySound(SoundType.Stomp, 1.0f);
                break;
            case GameEventType.CoinCollected:
                coinCount++;
                score += 200;
                DisplayScoreGain(gameEvent, 200);
                SoundController.PlaySound(SoundType.Coin, 1.0f);
                if (coinCount == 100)
                {
                    coinCount = 0;
                    MagicBrosMario.INSTANCE.Mario.Lives++;
                    SoundController.PlaySound(SoundType.OneUp, 0.4f);
                }
                break;
            case GameEventType.BlockBroken:
                score += 50;
                SoundController.PlaySound(SoundType.Break, 1.0f);
                DisplayScoreGain(gameEvent, 50);
                break;
            case GameEventType.PowerupAppears:
                SoundController.PlaySound(SoundType.PowerUpAppear, 1.0f);
                score += 1000;
                DisplayScoreGain(gameEvent, 1000);
                break;
            case GameEventType.PowerupCollected:
                if (gameEvent.Data is not OneUp)
                {
                    SoundController.PlaySound(SoundType.PowerUp, 1.0f);
                    score += 1000;
                    DisplayScoreGain(gameEvent, 1000);
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
                    score += 5000;
                    DisplayScoreGain(gameEvent, 5000);
                }
                else if (ratio <= 0.4f)
                {
                    score += 2000;
                    DisplayScoreGain(gameEvent, 2000);
                }
                else if (ratio <= 0.6f)
                {
                    score += 1000;
                    DisplayScoreGain(gameEvent, 1000);
                }
                else if (ratio <= 0.8f)
                {
                    score += 500;
                    DisplayScoreGain(gameEvent, 500);
                }
                else
                {
                    score += 100;
                    DisplayScoreGain(gameEvent, 100);
                }
                break;
            case GameEventType.EndOfLevel:
                levelOver = true;
                SoundController.StopMusic();
                SoundController.PlaySound(SoundType.GameOver, 1.0f);
                break;
            case GameEventType.Death:
                dead = true;
                goToTransition = true;
                break;
            default:
                break;
        }
    }
    public void Update(GameTime gametime)
    {
        stompChainTimer += gametime.ElapsedGameTime.TotalSeconds;
        for (int i = textsList.Count - 1; i >= 0; i--)
        {
            textsList[i].Update(gametime);
            if (!textsList[i].Display)
            {
                textsList.RemoveAt(i);
            }
        }
        if (waitForNextLevel) { return; }
        FrameCount++;
        if (FrameCount >= 24 && time >0 && !levelOver)
        {
            time--;
            FrameCount = 0;
        }
        if(time == 100 && playtimewarning && !levelOver) 
        { 
            playtimewarning = false;  
            SoundController.PauseMusic();
            SoundController.PlaySound(SoundType.TimeWarning, 1.0f);
        }else if (!playtimewarning && !SoundController.IsSoundOnCoolDown(SoundType.TimeWarning))
        {
            SoundController.ResumeMusic();
        }
        if (levelOver && !dead)
        {
            time--;
            score += 50;
            //Point incrementing sound
            if (time == 0)
            {
                waitForNextLevel = true;
            }
        }
        else if (time == 0) { MagicBrosMario.INSTANCE.Mario.KillMario(); }
        coin.Position = new Point(Camera.Instance.Position.X + 250, 27);
        coin.Update(gametime);

        if (goToTransition)
        {
            TransitionTimer -= (float)gametime.ElapsedGameTime.TotalSeconds;

            if (TransitionTimer <= 0)
            {
                MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level.Level1());
                goToTransition = false;
                TransitionTimer = 3f;
}
        }

    }
    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.DrawString(font, "MARIO", new Vector2(110, 10), Color.White);
        string numStr = score.ToString().PadLeft(6, '0');
        _spriteBatch.DrawString(font, numStr, new Vector2(110, 26), Color.White);

        numStr = coinCount.ToString().PadLeft(2, '0');
        coin.Draw(_spriteBatch);
        _spriteBatch.DrawString(font, "x" + numStr, new Vector2(275, 26), Color.White);

        _spriteBatch.DrawString(font, "LEVEL", new Vector2(425, 10), Color.White);
        _spriteBatch.DrawString(font, MagicBrosMario.INSTANCE.level.Name, new Vector2(459, 26), Color.White);

        numStr = time.ToString().PadLeft(3, '0');
        _spriteBatch.DrawString(font, "TIME", new Vector2(575, 10), Color.White);
        _spriteBatch.DrawString(font, numStr, new Vector2(590, 26), Color.White);

        for (int i = textsList.Count - 1; i >= 0; i--)
        {
            textsList[i].Draw(_spriteBatch);
        }
    }
}
