using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

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

    public Goomba(SharedTexture EnemyTexture, int leftBound, int rightBound)
    {
        int Y = 250;
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        
        sprites = [EnemyTexture.NewAnimatedSprite(295, 187, 18, 18, 2, 0.2f), 
                    EnemyTexture.NewSprite(276, 187, 18, 18)];
        Position = new Point(leftBound, Y);
        this.isAlive = true;
    }

    public bool GetIsAlive()
    {
        return isAlive;
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

    private void UnCollide(Rectangle intersect, CollideDirection direction)
    {
        if (direction == CollideDirection.Left)
        {
            Position = new Point(Position.X + intersect.Width, Position.Y);
            movingRight = true;
        }
        else if (direction == CollideDirection.Right)
        {
            Position = new Point(Position.X - intersect.Width, Position.Y);
            movingRight = false;
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite().Draw(_spriteBatch);
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        if (enemy is Bowser)
        {
            Kill();
            return;
        }
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            UnCollide(Rectangle.Intersect(CollisionBox, enemy.CollisionBox), direction);
        }
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            //UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
        }
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (direction == CollideDirection.Top){
            Kill();
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction) { }
}