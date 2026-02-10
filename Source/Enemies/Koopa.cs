using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
    private Boolean movingRight = true;

    private Vector2 destination;
    private Boolean isAlive;

    public Koopa(Texture2D texture, Rectangle sourceRectangle1, Rectangle sourceRectangle2, 
        Rectangle sourceRectangle3, Vector2 destinationVector, int boundWidth)
    {
        destination = destinationVector;
        _texture = texture;

        sources[0] = sourceRectangle1;//Walking right1
        sources[1] = sourceRectangle2;//Walking right2
        sources[2] = sourceRectangle3;//Walking left1
        sources[3] = sourceRectangle3;//Walking left2
        sources[4] = sourceRectangle1;//Hit1
        sources[5] = sourceRectangle2;//Hit2
        sources[6] = sourceRectangle3;//Dead1
        sources[7] = sourceRectangle3;//Dead2

        bound = boundWidth;
        this.isAlive = true;

    }
    public void Update(GameTime gametime)
    {
        timer += gametime.ElapsedGameTime.TotalSeconds;
        if (isAlive) // Replace later with condition to check if Koopa is alive or not or is hit or not
        {
            Walking();
        }
        else //Koopa is Dead
        {
            currentFrame = 6; // Set to dead frame if killed
        }
    }

    //This method also updates the current frame of the Koopa's animation and moves left or right
    //Right now its set on bound(which is screen width)
    public void Walking()
    {
        if (movingRight)
        {
            destination.X += 5;
            if (destination.X >= bound)
            {
                destination.X = bound;
                movingRight = false;
            }
            if(timer >= duration){
                if(currentFrame == 0)
                {
                    currentFrame = 1;
                }
                else
                {
                    currentFrame = 0;
                }
                timer = 0.0;
            }
        }
        else
        {
            destination.X -= 5;
            if (destination.X <= 0)
            {
                destination.X = 0;
                movingRight = true;
            }
            if(timer >= duration)
            {
                if(currentFrame == 2)
                {
                    currentFrame = 3;
                }
                else
                {
                    currentFrame = 2;
                }
                timer = 0.0;
            }
        }
    }

    public void Kill()
    {
        this.isAlive = false;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        
        _spriteBatch.Draw(_texture, destination, sources[currentFrame], Color.White, 0.0f, 
            new Vector2(sources[currentFrame].Width / 2, sources[currentFrame].Height / 2), 
            2.0f, SpriteEffects.None, 0.0f);
    }
}
