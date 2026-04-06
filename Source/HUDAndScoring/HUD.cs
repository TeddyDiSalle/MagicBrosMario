using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MagicBrosMario.Source.HUDAndScoring;

//Vincent Do
public class HUD
{
    private readonly SpriteFont font = MagicBrosMario.INSTANCE.font;
    private AnimatedSprite coin = new(MagicBrosMario.INSTANCE.ItemTexture, 192, 64, 10, 14, 4, 0.15f);
    private int score = 123;
    private int coinCount = 0;
    private int time = 0;
    private int[] stompScores = { 100, 200, 400, 800, 1000, 2000, 4000, 8000 };
    public static HUD Instance { get; } = new();
    public void SetTime(int time)
    {
        this.time = time;
    }

    public void IncCoin()
    {
        coinCount++;
        if(coinCount == 100)
        {
            coinCount = 0;
            MagicBrosMario.INSTANCE.Mario.Lives++;
        }
    }
    public void Update(GameTime gametime)
    {
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
        _spriteBatch.DrawString(font, MagicBrosMario.INSTANCE.lvl.Name, new Vector2(447, 26), Color.White);

        numStr = time.ToString().PadLeft(3, '0');
        _spriteBatch.DrawString(font, "TIME", new Vector2(565, 10), Color.White);
        _spriteBatch.DrawString(font, numStr, new Vector2(580, 26), Color.White);
    }
}
