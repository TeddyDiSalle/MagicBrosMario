using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source;

public class MarioFireball
{
    private const int VELOCITY = 4;
    private const float LIFETIME = 3.0f;

    private ISprite CurrentSprite;
    private readonly ISprite[] Sprites;
    private readonly bool movingRight;
    private float lifetimeRemaining;

    private Vector2 position;
    private Vector2 velocity;
    private readonly float GroundY;
    private const float Gravity = 0.35f;
    public MarioFireball(Sprite.AnimatedSprite fireball, Sprite.Sprite explosion, Vector2 position, bool movingRight, float groundY)
    {
        this.movingRight = movingRight;
        this.lifetimeRemaining = LIFETIME;
        Sprites = [fireball, explosion];
        CurrentSprite = Sprites[0];
        this.position = position;
        this.GroundY = groundY+32;
        if (movingRight)
        {
            velocity = new Vector2(VELOCITY, 0);
        }
        else
        {
            velocity = new Vector2(-VELOCITY, 0);
        }
    }

    public void Update(GameTime gameTime)
    {
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        lifetimeRemaining -= time;


        float distanceMoved = (float)(time * 100 * VELOCITY);

        if (CurrentSprite.Position.Y >= GroundY)
        {
            velocity -= new Vector2(0, distanceMoved);
        }
        if (CurrentSprite.Position.Y < GroundY)
        {
            velocity += new Vector2(0, Gravity);
        }
        position += velocity;
        if (CurrentSprite.Position.Y > GroundY)
        {
            position = new Vector2(position.X, GroundY);
            velocity -= new Vector2(0, velocity.Y);
        }
        CurrentSprite.Position = new Point((int)position.X, (int)position.Y);
        CurrentSprite.Update(gameTime);
    }

    public void Contact()
    {
        CurrentSprite = Sprites[1];
        lifetimeRemaining = 0.15f;
    }
    public bool IsExpired()
    {
        return lifetimeRemaining <= 0;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite.Draw(_spriteBatch);
    }
}