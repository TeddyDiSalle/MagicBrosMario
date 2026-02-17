using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source;

public class Fireball
{
    private const int VELOCITY = 150;
    private const float LIFETIME = 3.0f;

    private Sprite.AnimatedSprite spriteRight;
    private Sprite.AnimatedSprite spriteLeft;
    private Boolean movingRight;
    private float lifetimeRemaining;

    public Point Position
    {
        get { return movingRight ? spriteRight.Position : spriteLeft.Position; }
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
        lifetimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;

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
        return lifetimeRemaining <= 0;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (movingRight)
        {
            spriteRight.Draw(_spriteBatch);
        }
        else
        {
            spriteLeft.Draw(_spriteBatch);
        }
    }
}