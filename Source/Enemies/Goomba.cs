using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;

public class Goomba : IEnemy, ICollidable
{
    private const int VELOCITY = 100;
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

    // ICollidable implementation
    public Rectangle CollisionBox
    {
        get
        {
            return new Rectangle(
                CurrentSprite().Position.X,
                CurrentSprite().Position.Y,
                CurrentSprite().Size.X,
                CurrentSprite().Size.Y
            );
        }
    }

    public Goomba(Sprite.AnimatedSprite aliveSprite, Sprite.Sprite deadSprite, int Y, int leftBound, int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        aliveSprite.Position = new Point(leftBound, Y);
        deadSprite.Position = new Point(leftBound, Y);
        sprites = [aliveSprite, deadSprite];
        this.isAlive = true;
    }

    public void Update(GameTime gametime)
    {
        if (isAlive)
        {
            Walking(gametime);
        }

        CurrentSprite().Update(gametime);
    }

    public void Walking(GameTime gameTime)
    {
        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
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
        sprites[1].Position = sprites[0].Position;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite().Draw(_spriteBatch);
    }

    // ICollidable methods
    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (direction == CollideDirection.Down)
        {
            // Player stomped on Goomba from above
            Kill();
        }
        else
        {
            // Goomba hits player from side - player takes damage
            // Handle player damage here if needed
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        // Goomba doesn't interact with items
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        // Goomba hits another enemy - turn around
        movingRight = !movingRight;
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        // Goomba hits a block - turn around
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            movingRight = !movingRight;
        }
    }
}