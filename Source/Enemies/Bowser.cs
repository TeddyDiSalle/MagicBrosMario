using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;

public class Bowser : IEnemy, ICollidable
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
        // Boss logic: Player takes damage on contact
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        // Bowser could be hurt by Mario's fireballs here if you want
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        // Bowser doesn't kill enemies here — each enemy handles its own death
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            //UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
        }
    }
}