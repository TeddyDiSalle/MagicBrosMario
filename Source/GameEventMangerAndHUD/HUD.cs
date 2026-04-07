using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
namespace MagicBrosMario.Source.HUDAndScoring;

//Vincent Do
public class HUD
{
    private readonly SpriteFont font = MagicBrosMario.INSTANCE.font;
    private readonly AnimatedSprite coin = new(MagicBrosMario.INSTANCE.ItemTexture, 192, 64, 10, 14, 4, 0.15f);
    private int score = 0;
    private int coinCount = 70; //For testing and functionality check in
    private int level = 0;
    private int time = 200;
    private int FrameCount = 0;
    private bool levelOver = false;
    private bool waitForNextLevel = false;
    private readonly int[] stompScores = { 100, 200, 400, 800, 1000, 2000, 4000, 8000 };
    private int StompChain = 0;
    private readonly double stompChainCD = 1.0;
    private double stompChainTimer = 0;
    public static HUD Instance { get; } = new();
    private List<FloatingText> textsList;
    public void SetLevel(int level)
    {
        levelOver = false;
        waitForNextLevel = false;
        this.level = level;
    }
    public void SetTime(int time)
    {
        this.time = time;
    }

    public void SendEvent(GameEvent gameEvent)
    {
        switch (gameEvent.EventType)
        {
            case GameEventType.EnemyStomped:

                if(stompChainTimer >= stompChainCD)
                {
                    StompChain = 0;
                }
                stompChainTimer = 0;
                if (StompChain < stompScores.Length)
                {
                    score += stompScores[StompChain];
                    StompChain++;
                }
                else
                {
                    MagicBrosMario.INSTANCE.Mario.Lives++;
                    SoundEffectController.PlaySound(SoundTypes.OneUp, 0.4f);
                }
                break;
            case GameEventType.LandedOnGround:
                if (MagicBrosMario.INSTANCE.Mario.GetCurrentPower() != Power.Star)
                    StompChain = 0;
                break;
            case GameEventType.EnemyKilledByFireball:
                if(gameEvent.Data is not Bowser)
                    score += 100;
                else 
                    score += 5000;
                break;
            case GameEventType.CoinCollected:
                coinCount++;
                score += 200;
                if (coinCount == 100)
                {
                    coinCount = 0;
                    MagicBrosMario.INSTANCE.Mario.Lives++;
                    SoundEffectController.PlaySound(SoundTypes.OneUp, 0.4f);
                }
                break;
            case GameEventType.BlockBroken:
                score += 50;
                break;
            case GameEventType.PowerupAppears:
                score += 1000;
                break;
            case GameEventType.PowerupCollected:
                if(gameEvent.Data is not OneUp)
                    score += 1000;
                break;
            case GameEventType.FlagpoleReached:

                break;
            case GameEventType.EndOfLevel:
                levelOver = true;
                SoundEffectController.PlaySound(SoundTypes.GameOver, 1.0f);
                break;
            default:
                break;
        }
    }
    public void Update(GameTime gametime)
    {
        if (waitForNextLevel) { return; }
        FrameCount++;
        if (FrameCount == 24 && time >0 && !levelOver)
        {
            SendEvent(new GameEvent { EventType = GameEventType.CoinCollected});//For testing and functionality check in
            time--;
            FrameCount = 0;
        }

        if (levelOver)
        {
            time--;
            score += 50;
            //Point incrementing sound
            if (score == 0)
            {
                waitForNextLevel = true;
            }
        }
        else if (time == 0) { MagicBrosMario.INSTANCE.Mario.KillMario(); }
        coin.Position = new Point(Camera.Instance.Position.X + 250, 27);
        coin.Update(gametime);
    }
    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.DrawString(font, "MARIO", new Vector2(100, 10), Color.White);
        string numStr = score.ToString().PadLeft(6, '0');
        _spriteBatch.DrawString(font, numStr, new Vector2(100, 26), Color.White);

        numStr = coinCount.ToString().PadLeft(2, '0');
        coin.Draw(_spriteBatch);
        _spriteBatch.DrawString(font, "x" + numStr, new Vector2(265, 26), Color.White);

        _spriteBatch.DrawString(font, "LEVEL", new Vector2(415, 10), Color.White);
        _spriteBatch.DrawString(font, level.ToString(), new Vector2(447, 26), Color.White);

        numStr = time.ToString().PadLeft(3, '0');
        _spriteBatch.DrawString(font, "TIME", new Vector2(565, 10), Color.White);
        _spriteBatch.DrawString(font, numStr, new Vector2(580, 26), Color.White);
    }
}
