using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace MagicBrosMario.Source;
public class HUD
{
    private readonly SpriteFont font = MagicBrosMario.INSTANCE.font; 
    private int score = 123;
    private int coinCount = 0;
    private int level = 0;
    private int time = 0;
    public static HUD Instance { get; } = new();

    public void SetLevel(int level)
    {
        this.level = level;
    }
    public void SetTime(int time)
    {
        this.time = time;
    }

    public void IncCoin()
    {
        coinCount++;
    }
    public void Update()
    {
        //Update scores
    }
    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.DrawString(font, "MARIO", new Vector2(100, 10), Color.White);
        string numStr = score.ToString().PadLeft(6, '0');
        _spriteBatch.DrawString(font, numStr, new Vector2(100, 26), Color.White);
        numStr = coinCount.ToString().PadLeft(2, '0');
        _spriteBatch.DrawString(font, "x" + numStr, new Vector2(264, 26), Color.White);
    }
}
