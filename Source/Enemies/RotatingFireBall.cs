using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;

public class RotatingFireBar : IEnemy, ICollidable
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

    // ICollidable implementation
    // Note: We need to check collision for EACH fireball, not just one box
    public Rectangle CollisionBox
    {
        get
        {
            // Return the bounding box that contains all fireballs
            // This is used for broad-phase collision detection
            int maxDistance = fireballCount * fireballSpacing;
            return new Rectangle(
                centerPosition.X - maxDistance,
                centerPosition.Y - maxDistance,
                maxDistance * 2,
                maxDistance * 2
            );
        }
    }

    public RotatingFireBar(
        Sprite.SharedTexture sharedTexture,
        int fireballX,
        int fireballY,
        int fireballWidth,
        int fireballHeight,
        int centerX,
        int centerY,
        int fireballCount,
        int fireballSpacing)
    {
        this.centerPosition = new Point(centerX, centerY);
        this.fireballCount = fireballCount;
        this.fireballSpacing = fireballSpacing;

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
    public bool GetIsAlive()
    {
        return true;
    }
    public void Kill()
    {
        // Fire bars cannot be killed - this does nothing
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        foreach (var fireball in fireballs)
        {
            fireball.Draw(_spriteBatch);
        }
    }

    // Helper method to check collision with individual fireballs
    public bool IsCollidingWithFireballs(Rectangle otherBox)
    {
        foreach (var fireball in fireballs)
        {
            Rectangle fireballBox = new Rectangle(
                fireball.Position.X,
                fireball.Position.Y,
                fireball.Size.X,
                fireball.Size.Y
            );

            if (fireballBox.Intersects(otherBox))
            {
                return true;
            }
        }
        return false;
    }

    // ICollidable methods
    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        // Fire bar damages player on any collision
        // Need to check each individual fireball for accurate collision
        Rectangle playerBox = new Rectangle(
            (int)player.Position.X,
            (int)player.Position.Y,
            // You'll need to get player width/height somehow
            16 * 3, // Assuming scaled player size
            16 * 3
        );

        if (IsCollidingWithFireballs(playerBox))
        {
            // Player takes damage from fire bar
            // Handle player damage here if needed
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        // Fire bars don't interact with items
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        // Fire bars don't interact with other enemies
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        // Fire bars don't interact with blocks
        // They're attached to a fixed center point
    }
}