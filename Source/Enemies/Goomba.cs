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
    private const float SCALE = 2f;
    private const float GRAVITY = 0.35f;
    private Sprite.ISprite[] sprites;
    private bool movingRight = true;
    private bool isAlive = true;
    private float velocityY = 0f;

    public Point Position
    {
        get => sprites[0].Position;
        set 
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
            var sprite = isAlive ? sprites[0] : sprites[1];
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public Goomba(SharedTexture EnemyTexture, int y, int leftBound)
    {
        sprites = [EnemyTexture.NewAnimatedSprite(295, 187, 18, 18, 2, 0.2f), 
                    EnemyTexture.NewSprite(276, 187, 18, 18)];
        foreach (var sprite in sprites)
        {
            sprite.Scale = SCALE;
            sprite.Visible = false;
        }
        Position = new Point(leftBound, y);
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

        velocityY += GRAVITY;
        Position = new Point(Position.X, Position.Y + (int)velocityY);

        sprites[0].Visible = isAlive;
        sprites[1].Visible = !isAlive;
    }

    public void Walking(GameTime gameTime)
    {
        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        var dx = (int)(sec * VELOCITY);

        if (movingRight)
        {
            Position = new Point(Position.X + dx, Position.Y);
        }
        else
        {
            Position = new Point(Position.X - dx, Position.Y);
        }
    }

    public void Kill()
    {
        this.isAlive = false;
        foreach (var sprite in sprites)
        {
            sprite.Drop();
        }
        CollisionController.Instance.RemoveEnemy(this);
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
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        if (enemy is Bowser || (enemy is Koopa koopa && koopa.IsShellMoving()))
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
        if (direction == CollideDirection.Down)
        {
            Rectangle intersect = Rectangle.Intersect(CollisionBox, block.CollisionBox);
            Position = new Point(Position.X, Position.Y - intersect.Height);
            velocityY = 0;
        }
        else if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (block.CollisionBox.Y < Position.Y + CollisionBox.Height - 4)
            {
                UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
            }
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