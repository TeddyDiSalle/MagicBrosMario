using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
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
    private bool Dead = false;

    public void Update(GameTime gametime)
    {
        LifeTimer += gametime.ElapsedGameTime.TotalSeconds;
        if(LifeTimer > LifeSpan)
        {
            Dead = true;
        }
    }

    public bool getStatus() => Dead;
    public void Draw(SpriteBatch _spriteBatch)
    {
        if(Dead) return;
        string numStr = num.ToString();
        Vector2 pos = new Vector2(gameEvent.EventPosition.X, gameEvent.EventPosition.Y);
        _spriteBatch.DrawString(font, numStr, pos, Color.White, 0.0f, Vector2.Zero, 0.5f, SpriteEffects.None, 0.0f);

    }
}
