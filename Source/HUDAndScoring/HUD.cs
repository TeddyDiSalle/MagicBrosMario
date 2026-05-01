using MagicBrosMario.Source.GameStates;
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
    private int coinCount = 0;
    public int time = 150;
    private int FrameCount = 0;
    public bool levelOver = false;
    private bool goToTransition = false;
    public float TransitionTimer = 3f;
    public bool dead { get; private set; } = false;
    public bool WaitForNextLevel { get; set; } = false;
    public bool playtimewarning = true;
    private readonly List<FloatingText> textsList = [];
    public static HUD Instance { get; } = new();
    internal int Score { get => score; set => score = value; }
    internal int CoinCount { get => coinCount; set => coinCount = value; }
    internal bool LevelOverState { get => levelOver; set => levelOver = value; }
    internal bool Dead { set => dead = value; }
    internal bool GoToTransition { get => goToTransition; set => goToTransition = value; }
    private readonly EventManager eventManager;
    private HUD()
    {
        eventManager = new EventManager(this);
    }

    public void LevelOver()
    {
        levelOver = true;
        WaitForNextLevel = true;
    }
    public void LevelStart()
    {
        levelOver = false;
        WaitForNextLevel = false;
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
    public void ResetScoreAndCoins()
    {
        score = 0;
        coinCount = 0;
    }
    public void SendEvent(GameEvent gameEvent) => eventManager.SendEvent(gameEvent);

    private void UpdateHelperText(GameTime gametime)
    {
        for (int i = textsList.Count - 1; i >= 0; i--)
        {
            textsList[i].Update(gametime);
            if (!textsList[i].Display)
            {
                textsList.RemoveAt(i);
            }
        }
    }
    public void Update(GameTime gametime)
    {
        UpdateHelperText(gametime);
        FrameCount++;
        if (FrameCount >= 24 && time > 0 && !levelOver)
        {
            time--;
            FrameCount = 0;
        }
        coin.Position = new Point(Camera.Instance.Position.X + 260, Camera.Instance.Position.Y + 27);
        coin.Update(gametime);
        eventManager.Update(gametime);
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
        _spriteBatch.DrawString(font, MagicBrosMario.INSTANCE.level.Name, new Vector2(442, 26), Color.White);

        numStr = time.ToString().PadLeft(3, '0');
        _spriteBatch.DrawString(font, "TIME", new Vector2(575, 10), Color.White);
        _spriteBatch.DrawString(font, numStr, new Vector2(590, 26), Color.White);

        for (int i = textsList.Count - 1; i >= 0; i--)
        {
            textsList[i].Draw(_spriteBatch);
        }
    }
}
