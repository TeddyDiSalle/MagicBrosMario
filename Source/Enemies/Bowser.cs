using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

namespace MagicBrosMario.Source;

public class Bowser : IEnemy, ICollidable
{
    private const int VELOCITY = 100;
    private const float FIRE_COOLDOWN = 3.0f;

    private readonly int leftBound;
    private readonly int rightBound;
    private Sprite.AnimatedSprite walkingRightSprite;
    private Sprite.AnimatedSprite walkingLeftSprite;
    private SharedTexture fireTexture;

    private List<Fireball> activeFireballs = new List<Fireball>();

    private bool movingRight = true;
    public bool isAlive = true;
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

    public Rectangle CollisionBox
    {
        get
        {
            if (!isAlive) return Rectangle.Empty;

            var currentSprite = movingRight ? walkingRightSprite : walkingLeftSprite;
            return new Rectangle(
                currentSprite.Position.X,
                currentSprite.Position.Y,
                currentSprite.Size.X,
                currentSprite.Size.Y
            );
        }
    }

    public Bowser(SharedTexture EnemyTexture, SharedTexture FireTexture, int y, int leftBound, int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        this.fireTexture = FireTexture;

        walkingRightSprite = EnemyTexture.NewAnimatedSprite(255, 368, 35, 32, 4, 0.2f);
        walkingLeftSprite = EnemyTexture.NewAnimatedSprite(116, 368, 35, 32, 2, 0.2f);

        Position = new Point(leftBound, y);
        this.fireCooldownTimer = FIRE_COOLDOWN;
    }

    public bool GetIsAlive()
    {
        return isAlive;
    }

    public void Update(GameTime gameTime)
    {
        if (!isAlive) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        Move(gameTime);

        fireCooldownTimer -= deltaTime;
        if (fireCooldownTimer <= 0)
        {
            ShootFireball();
            fireCooldownTimer = FIRE_COOLDOWN;
        }

        for (int i = activeFireballs.Count - 1; i >= 0; i--)
        {
            activeFireballs[i].Update(gameTime);
            if (activeFireballs[i].IsExpired()) activeFireballs.RemoveAt(i);
        }

        walkingRightSprite.Update(gameTime);
        walkingLeftSprite.Update(gameTime);
    }

    private void ShootFireball()
    {
        var fireballSpriteRight = fireTexture.NewAnimatedSprite(161, 253, 24, 8, 2, 0.1f);
        var fireballSpriteLeft = fireTexture.NewAnimatedSprite(101, 253, 24, 8, 2, 0.1f);

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
        var sec = gameTime.ElapsedGameTime.TotalSeconds;
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

    public void Kill() => this.isAlive = false;

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
        if (!isAlive) return;

        if (movingRight) walkingRightSprite.Draw(_spriteBatch);
        else walkingLeftSprite.Draw(_spriteBatch);

        foreach (var fireball in activeFireballs) fireball.Draw(_spriteBatch);
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
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            //UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
        }
    }
}