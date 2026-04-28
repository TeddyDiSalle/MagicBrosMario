using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.HUDAndScoring;

//Vincent Do
public class FloatingText(GameEvent gameEvent, int num)
{
    private readonly SpriteFont font = MagicBrosMario.INSTANCE.font;
    private readonly double LifeSpan = 1.0;
    private double LifeTimer = 0;
    public bool Display { get; private set; } = true;

    public void Update(GameTime gametime)
    {
        LifeTimer += gametime.ElapsedGameTime.TotalSeconds;
        if(LifeTimer > LifeSpan)
        {
            Display = false;
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (!Display) { return; }
        string numStr = num.ToString();
        Vector2 pos = new(gameEvent.EventPosition.X - Camera.Instance.Position.X, gameEvent.EventPosition.Y - Camera.Instance.Position.Y);
        _spriteBatch.DrawString(font, numStr, pos, Color.White, 0.0f, Vector2.Zero, .5f, SpriteEffects.None, 0f);
    }
}
