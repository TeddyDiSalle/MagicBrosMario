using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace MagicBrosMario.Source;

public class Bowser : IEnemy
{
    private const int VELOCITY = 100;
    private const float FIRE_COOLDOWN = 3.0f;

    private readonly int leftBound;
    private readonly int rightBound;
    private Sprite.AnimatedSprite walkingRightSprite;
    private Sprite.AnimatedSprite walkingLeftSprite;
    private Sprite.SharedTexture sharedTexture;
    private int fireballRightX, fireballRightY;
    private int fireballLeftX, fireballLeftY;
    private int fireballWidth, fireballHeight;

    private List<Fireball> activeFireballs = new List<Fireball>();

    private Boolean movingRight = true;
    private Boolean isAlive;
    private float fireCooldownTimer = 0f;

    public Point Position
    {
        get { return movingRight ? walkingRightSprite.Position : walkingLeftSprite.Position; }
        private set
        {
            walkingRightSprite.Position = value;
            walkingLeftSprite.Position = value;
        }
    }

    public Bowser(
        Sprite.AnimatedSprite walkingRightSprite,
        Sprite.AnimatedSprite walkingLeftSprite,
        Sprite.SharedTexture sharedTexture,
        int fireballRightX,
        int fireballRightY,
        int fireballLeftX,
        int fireballLeftY,
        int fireballWidth,
        int fireballHeight,
        int y,
        int leftBound,
        int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        this.walkingRightSprite = walkingRightSprite;
        this.walkingLeftSprite = walkingLeftSprite;
        this.sharedTexture = sharedTexture;
        this.fireballRightX = fireballRightX;
        this.fireballRightY = fireballRightY;
        this.fireballLeftX = fireballLeftX;
        this.fireballLeftY = fireballLeftY;
        this.fireballWidth = fireballWidth;
        this.fireballHeight = fireballHeight;

        Position = new Point(leftBound, y);
        this.isAlive = true;
        this.fireCooldownTimer = FIRE_COOLDOWN;
    }

    public void Update(GameTime gameTime)
    {
        if (!isAlive)
        {
            return; // Don't update if dead
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Move Bowser
        Move(gameTime);

        // Check if it's time to breathe fire
        fireCooldownTimer -= deltaTime;
        if (fireCooldownTimer <= 0)
        {
            ShootFireball();
            fireCooldownTimer = FIRE_COOLDOWN;
        }

        // Update all active fireballs
        for (int i = activeFireballs.Count - 1; i >= 0; i--)
        {
            activeFireballs[i].Update(gameTime);

            if (activeFireballs[i].IsExpired())
            {
                activeFireballs.RemoveAt(i);
            }
        }

        walkingRightSprite.Update(gameTime);
        walkingLeftSprite.Update(gameTime);
    }

    private void ShootFireball()
    {
        var fireballSpriteRight = sharedTexture.NewAnimatedSprite(
            fireballRightX, fireballRightY, fireballWidth, fireballHeight, 2, 0.1f);
        var fireballSpriteLeft = sharedTexture.NewAnimatedSprite(
            fireballLeftX, fireballLeftY, fireballWidth, fireballHeight, 2, 0.1f);
        
        fireballSpriteRight.Scale = 3f;
        fireballSpriteLeft.Scale = 3f;

        var fireball = new Fireball(
            fireballSpriteRight,
            fireballSpriteLeft,
            Position.X,
            Position.Y,
            movingRight);
        activeFireballs.Add(fireball);
    }

    private void Move(GameTime gameTime)
    {
        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        var dx = (int)(sec * VELOCITY);

        if (movingRight)
        {
            int newX = Position.X + dx;

            if (newX >= rightBound)
            {
                Position = new Point(rightBound, Position.Y);
                movingRight = false;
            }
            else
            {
                Position = new Point(newX, Position.Y);
            }
        }
        else
        {
            int newX = Position.X - dx;

            if (newX <= leftBound)
            {
                Position = new Point(leftBound, Position.Y);
                movingRight = true;
            }
            else
            {
                Position = new Point(newX, Position.Y);
            }
        }
    }

    public void Kill()
    {
        this.isAlive = false;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (isAlive)
        {
            // Draw the correct sprite based on direction
            if (movingRight)
            {
                walkingRightSprite.Draw(_spriteBatch);
            }
            else
            {
                walkingLeftSprite.Draw(_spriteBatch);
            }

            // Draw all active fireballs
            foreach (var fireball in activeFireballs)
            {
                fireball.Draw(_spriteBatch);
            }
        }
        // Don't draw anything if dead (like Piranha Plant)
    }
}