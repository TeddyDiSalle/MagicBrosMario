using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source;

public class Font : ISprite
{
    private SpriteFont font;

    private Vector2 destination;

    public Font(SpriteFont fontText, Vector2 destinationVector)
    {
        destination = destinationVector;
        font = fontText;
    }
    public void Update(GameTime gametime)
    {
        //Nothing
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.DrawString(font, "Credits\nProgram Made By: Vincent Do\nSprites from: https://www.mariouniverse.com/sprites-nes-smb/",
            destination, Color.White, 0.0f, 
            Vector2.Zero, 
            Vector2.One, SpriteEffects.None, 0.0f);
    }
}
