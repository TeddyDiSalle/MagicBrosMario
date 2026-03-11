using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;

public class Fireball : ICollidable
{
    private const int VELOCITY = 150;
    private const float LIFETIME = 3.0f;

    private Sprite.AnimatedSprite spriteRight;
    private Sprite.AnimatedSprite spriteLeft;
    private Boolean movingRight;
    private float lifetimeRemaining;
    private bool isActive = true;

    public Point Position
    {
        get { return movingRight ? spriteRight.Position : spriteLeft.Position; }
    }

    public Rectangle CollisionBox
    {
        get
        {
            if (!isActive)
            {
                return Rectangle.Empty;
            }

            var currentSprite = movingRight ? spriteRight : spriteLeft;
            return new Rectangle(
                currentSprite.Position.X,
                currentSprite.Position.Y,
                currentSprite.Size.X,
                currentSprite.Size.Y
            );
        }
    }

    public Fireball(
        Sprite.AnimatedSprite fireballSpriteRight,
        Sprite.AnimatedSprite fireballSpriteLeft,
        int startX,
        int startY,
        bool movingRight)
    {
        this.spriteRight = fireballSpriteRight;
        this.spriteLeft = fireballSpriteLeft;
        this.movingRight = movingRight;
        this.lifetimeRemaining = LIFETIME;
        
        spriteRight.Position = new Point(startX, startY);
        spriteLeft.Position = new Point(startX, startY);
    }

    public void Update(GameTime gameTime)
    {
        if (!isActive)
        {
            return;
        }

        lifetimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (lifetimeRemaining <= 0)
        {
            isActive = false;
            return;
        }

        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        var dx = (int)(sec * VELOCITY);

        if (movingRight)
        {
            var newPos = new Point(spriteRight.Position.X + dx, spriteRight.Position.Y);
            spriteRight.Position = newPos;
            spriteLeft.Position = newPos;
        }
        else
        {
            var newPos = new Point(spriteLeft.Position.X - dx, spriteLeft.Position.Y);
            spriteRight.Position = newPos;
            spriteLeft.Position = newPos;
        }

        spriteRight.Update(gameTime);
        spriteLeft.Update(gameTime);
    }

    public bool IsExpired()
    {
        return !isActive || lifetimeRemaining <= 0;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (!isActive)
        {
            return;
        }

        if (movingRight)
        {
            spriteRight.Draw(_spriteBatch);
        }
        else
        {
            spriteLeft.Draw(_spriteBatch);
        }
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        isActive = false;
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        // Fireballs don't interact with items
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        // Fireball just deactivates itself — the enemy handles its own death
        isActive = false;
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        isActive = false;
    }
}