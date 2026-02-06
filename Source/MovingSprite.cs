using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntroToMonoGame;

public class MovingSprite : ISprite
{
    private Texture2D _texture;

    private Rectangle source;

    private Vector2 destination;

    private int bound;

    private double timer = 0.0;
    private double duration = 0.02;

    public MovingSprite(Texture2D texture, Rectangle sourceRectangle, Vector2 destinationVector, int boundHeight)
    {
        destination = destinationVector;
        _texture = texture;
        source = sourceRectangle;
        bound = boundHeight;

    }
    public void Update(GameTime gametime)
    {
        timer += gametime.ElapsedGameTime.TotalSeconds;

        if (timer >= duration)
        {
            destination.Y -= 5;
            if(destination.Y < 0)
            {
                destination.Y = bound;
            }
            timer = 0.0;
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(_texture, destination, source, Color.White, 0.0f, 
            new Vector2(source.Width / 2, source.Height / 2), 
            2.0f, SpriteEffects.None, 0.0f);
    }
}
