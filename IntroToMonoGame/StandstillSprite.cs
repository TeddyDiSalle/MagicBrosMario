using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntroToMonoGame;

public class StandstillSprite : ISprite
{
    private Texture2D _texture;

    private Rectangle source;

    private Vector2 destination;

    public StandstillSprite(Texture2D texture, Rectangle sourceRectangle, Vector2 destinationVector)
    {
        destination = destinationVector;
        _texture = texture;
        source = sourceRectangle;


    }
    public void Update(GameTime gametime)
    {
        //Nothing
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(_texture, destination, source, Color.White, 0.0f,
            new Vector2(source.Width / 2, source.Height / 2),
            2.0f, SpriteEffects.None, 0.0f);
    }
}
