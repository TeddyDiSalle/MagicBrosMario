using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source;

public class Koopa : IEnemy
{
    private Texture2D _texture;

    private Rectangle[] sources = new Rectangle[3];

    private int currentFrame = 0;

    private double timer = 0.0;
    private double duration = 0.04;

    private int bound;

    private Vector2 destination;

    public Koopa(Texture2D texture, Rectangle sourceRectangle1, Rectangle sourceRectangle2, 
        Rectangle sourceRectangle3, Vector2 destinationVector, int boundWidth)
    {
        destination = destinationVector;
        _texture = texture;

        sources[0] = sourceRectangle1;//Alive Walking
        sources[1] = sourceRectangle2;//Alive Walking
        sources[2] = sourceRectangle3;//Dead

        bound = boundWidth;
        

    }
    public void Update(GameTime gametime)
    {
        timer += gametime.ElapsedGameTime.TotalSeconds;
        if (true) // Replace later with condition to check if Goomba is alive or not
        {
            Walking();
        }
        else
        {
            currentFrame = 2; // Set to dead frame if killed
        }
    }
    public void Walking()
    {
        if (timer >= duration)
        {
            currentFrame++;
            if (currentFrame == 1)
            {
                currentFrame = 0;
            }

            destination.X += 5;
            if (destination.X > bound)
            {
                destination.X = 0;
            }
            timer = 0.0;
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        
        _spriteBatch.Draw(_texture, destination, sources[currentFrame], Color.White, 0.0f, 
            new Vector2(sources[currentFrame].Width / 2, sources[currentFrame].Height / 2), 
            2.0f, SpriteEffects.None, 0.0f);
    }
}
