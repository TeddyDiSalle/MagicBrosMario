using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class RotatingFireBar : IEnemy, ICollidable
{
    private const float ROTATION_SPEED = 1.0f;
    private const float SCALE = 4f;

    private Sprite.ISprite[] fireballs;
    private readonly Point centerPosition;
    private int fireballCount;
    private int fireballSpacing;
    private float currentAngle = 0f;

    public Point Position
    {
        get { return centerPosition; }
    }

    public Rectangle CollisionBox
    {
        get
        {
            int maxDistance = fireballCount * fireballSpacing;
            return new Rectangle(
                centerPosition.X - maxDistance,
                centerPosition.Y - maxDistance,
                maxDistance * 2,
                maxDistance * 2
            );
        }
    }

    public RotatingFireBar(SharedTexture FireTexture, int centerX, int centerY, int fireballCount, int fireballSpacing)
    {
        this.centerPosition = new Point(centerX, centerY);
        this.fireballCount = fireballCount;
        this.fireballSpacing = fireballSpacing;

        fireballs = new Sprite.ISprite[fireballCount];
        for (int i = 0; i < fireballCount; i++)
        {
            var fireball = FireTexture.NewSprite(364, 188, 8, 8);
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
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        foreach (var fireball in fireballs)
        {
            fireball.Draw(_spriteBatch);
        }
    }

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

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
    }
}