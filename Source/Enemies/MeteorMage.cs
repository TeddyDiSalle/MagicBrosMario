// MeteorMage.cs
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

public class MeteorMage : IEnemy, ICollidable
{
    private const float METEOR_COOLDOWN = 2.0f;
    private const int MAX_HEALTH = 6;
    private const float GRAVITY = 0.35f;

    private ISprite sprite; 
    private SharedTexture meteorTexture;
    private List<Meteor> activeMeteors = new();
    private Random random = new();

    private bool isAlive = true;
    private float meteorCooldownTimer = 0f;
    private int health = MAX_HEALTH;
    private float velocityY = 0f;

    public Point Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    public Rectangle CollisionBox
    {
        get
        {
            if (!isAlive) return Rectangle.Empty;
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public MeteorMage(SharedTexture enemyTexture, int y, int x)
    {
        meteorTexture = enemyTexture;

        sprite = enemyTexture.NewAnimatedSprite(257, 286, 16, 25, 2, .5f);
        sprite.Scale = 2f;
        Position = new Point(x, y);
    }

    public bool GetIsAlive() => isAlive;

    public void Update(GameTime gameTime)
    {
        if (!isAlive) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        meteorCooldownTimer -= deltaTime;
        if (meteorCooldownTimer <= 0)
        {
            SpawnMeteor();
            meteorCooldownTimer = METEOR_COOLDOWN;
        }

        // Update and clean up expired meteors
        for (int i = activeMeteors.Count - 1; i >= 0; i--)
        {
            activeMeteors[i].Update(gameTime);
            if (activeMeteors[i].IsExpired())
                activeMeteors.RemoveAt(i);
        }

        // Apply gravity
        velocityY += GRAVITY;
        Position = new Point(Position.X, Position.Y + (int)velocityY);

        sprite.Visible = true;
    }

    private void SpawnMeteor()
    {
        int marioX = Camera.Instance.Position.X + Camera.Instance.WindowSize.X / 2;
        int targetX = marioX + random.Next(-25, 25);
        int spawnY = Camera.Instance.Position.Y - 20;

        var meteorSprite = meteorTexture.NewSprite(372, 241, 14, 16);
        meteorSprite.Scale = 2f;

        var meteor = new Meteor(meteorSprite, targetX, spawnY);
        activeMeteors.Add(meteor);
        CollisionController.Instance.AddEnemy(meteor);
    }

    private void TakeDamage()
    {
        health--;
        if (health <= 0) Kill();
    }

    public void Kill()
    {
        isAlive = false;
        sprite.Visible = false;
        sprite.Drop();
        CollisionController.Instance.RemoveEnemy(this);

        foreach (var meteor in activeMeteors)
            meteor.Kill();
        activeMeteors.Clear();
    }

    private void UnCollide(Rectangle intersect, CollideDirection direction)
    {
        if (direction == CollideDirection.Left)
            Position = new Point(Position.X + intersect.Width, Position.Y);
        else if (direction == CollideDirection.Right)
            Position = new Point(Position.X - intersect.Width, Position.Y);
    }

    // Camera handles drawing
    public void Draw(SpriteBatch _spriteBatch) { }

    public void OnCollidePlayer(Player player, CollideDirection direction) { }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item is MarioFireball)
            TakeDamage();
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

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
                UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
        }
    }
}