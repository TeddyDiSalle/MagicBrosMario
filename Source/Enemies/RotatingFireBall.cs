using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source;

public class RotatingFireBar : IEnemy
{
    private const float ROTATION_SPEED = 1.0f;
    private const float SCALE = 3f; 

    private Sprite.ISprite[] fireballs;
    private readonly Point centerPosition;
    private int fireballCount;
    private int fireballSpacing;
    private float currentAngle = 0f;

    public Point Position
    {
        get { return centerPosition; }
    }

    public RotatingFireBar(
        Sprite.SharedTexture sharedTexture,
        int fireballX,      // X coordinate in sprite sheet
        int fireballY,      // Y coordinate in sprite sheet
        int fireballWidth,
        int fireballHeight,
        int centerX,        // X position on screen
        int centerY,        // Y position on screen
        int fireballCount,
        int fireballSpacing)
    {
        this.centerPosition = new Point(centerX, centerY);
        this.fireballCount = fireballCount;
        this.fireballSpacing = fireballSpacing;

        // Create separate sprite instances for each fireball
        fireballs = new Sprite.ISprite[fireballCount];
        for (int i = 0; i < fireballCount; i++)
        {
            var fireball = sharedTexture.NewSprite(fireballX, fireballY, fireballWidth, fireballHeight);
            fireball.Scale = SCALE;
            fireballs[i] = fireball;
        }
    }

    public void Update(GameTime gameTime)
    {

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        currentAngle += ROTATION_SPEED * deltaTime;

        if (currentAngle >= MathHelper.TwoPi)
        {
            currentAngle -= MathHelper.TwoPi;
        }

        for (int i = 0; i < fireballCount; i++)
        {
            int distanceFromCenter = (i + 1) * fireballSpacing;

            int fireballXPos = centerPosition.X + (int)(Math.Cos(currentAngle) * distanceFromCenter);
            int fireballYPos = centerPosition.Y + (int)(Math.Sin(currentAngle) * distanceFromCenter);

            fireballs[i].Position = new Point(fireballXPos, fireballYPos);
        }

        foreach (var fireball in fireballs)
        {
            fireball.Update(gameTime);
        }
    }

    public void Kill()
    {
        //Needed for IEnemy interface
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        foreach (var fireball in fireballs)
        {
            fireball.Draw(_spriteBatch);
        }
        
    }
}