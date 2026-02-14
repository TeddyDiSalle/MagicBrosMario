using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source;

public class Goomba : IEnemy
{
    private const int VELOCITY = 5;
    private readonly int leftBound;
    private readonly int rightBound;
    private Sprite.ISprite[] sprites;

    private Sprite.ISprite CurrentSprite()
    {
        if (isAlive)
        {
            return sprites[0]; // Alive sprite
        }
        else
        {
            return sprites[1]; // Dead sprite
        }
    }

    private Boolean movingRight = true;

    public Point Position
    {
        get { return CurrentSprite().Position; }
        private set { CurrentSprite().Position = value; }
    }
    private Boolean isAlive;

    public Goomba(Sprite.AnimatedSprite aliveSprite, Sprite.Sprite deaedSprite, int Y, int leftBound, int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        aliveSprite.Position = new Point(leftBound, Y);
        deaedSprite.Position = new Point(leftBound, Y);
        sprites = [aliveSprite, deaedSprite];
    }

    public void Update(GameTime gametime)
    {
        if (isAlive) // Replace later with condition to check if Goomba is alive or not
        {
            Walking(gametime);
        }

        CurrentSprite().Update(gametime);
    }

    //This method also updates the current frame of the Goomba's animation and moves left or right
    //Right now its set on bound(which is screen width)
    public void Walking(GameTime gameTime)
    {
        var sec = (double)gameTime.ElapsedGameTime.Milliseconds / 1000.0;
        var dx = (int)(sec * VELOCITY);

        if (movingRight)
        {
            Position = new Point(Position.X + dx, Position.Y);

            if (Position.X >= rightBound)
            {
                Position = new Point(rightBound, Position.Y);
                movingRight = false;
            }
        }
        else
        {
            Position = new Point(Position.X - dx, Position.Y);

            if (Position.X <= leftBound)
            {
                Position = new Point(leftBound, Position.Y);
                movingRight = true;
            }
        }
    }

    public void Kill()
    {
        this.isAlive = false;
        sprites[1].Position = sprites[0].Position; // Set the dead sprite's position to the current position of the alive sprite
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite().Draw(_spriteBatch);
    }
}
