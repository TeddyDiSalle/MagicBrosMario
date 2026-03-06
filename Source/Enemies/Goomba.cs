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
    private bool movingRight = true;
    private bool isAlive = true;

    private Sprite.ISprite CurrentSprite() => isAlive ? sprites[0] : sprites[1];

    public Point Position
    {
        get => CurrentSprite().Position;
        private set 
        { 
            // Keep all visual states synced to the same coordinate
            foreach (var sprite in sprites)
            {
                sprite.Position = value;
            }
        }
    }

    public Rectangle CollisionBox
    {
        get
        {
            var sprite = CurrentSprite();
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public Goomba(Sprite.AnimatedSprite aliveSprite, Sprite.Sprite deadSprite, int Y, int leftBound, int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        
        sprites = [aliveSprite, deadSprite];
        Position = new Point(leftBound, Y);
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
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite().Draw(_spriteBatch);
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        

        int pushDistance = 15; 

        if (direction == CollideDirection.Left)
        {
            // Hit on my left side, I must move Right
            movingRight = true;
            Position = new Point(Position.X + pushDistance, Position.Y);
        }
        else if (direction == CollideDirection.Right)
        {
            // Hit on my right side, I must move Left
            movingRight = false;
            Position = new Point(Position.X - pushDistance, Position.Y);
        }
        
        Console.WriteLine($"Goomba hit {enemy.GetType().Name}. Forced direction: {(movingRight ? "Right" : "Left")}");
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            
            int pushDistance = 5; 

            if (direction == CollideDirection.Left)
            {
                // Hit a block on my left, must move Right
                movingRight = true;
                Position = new Point(Position.X + pushDistance, Position.Y);
            }
            else if (direction == CollideDirection.Right)
            {
                // Hit a block on my right, must move Left
                movingRight = false;
                Position = new Point(Position.X - pushDistance, Position.Y);
            }
        }
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (direction == CollideDirection.Down) Kill();
    }

    public void OnCollideItem(IItems item, CollideDirection direction) { }
}