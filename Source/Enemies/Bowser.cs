using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class Bowser : IEnemy, ICollidable
{
    public bool AlwaysActive => false;
    private const int VELOCITY = 100;
    private const float FIRE_COOLDOWN = 3.0f;
    private const float GRAVITY = 0.35f;
    private const int MAX_HEALTH = 20;
    private const float SCALE = 2f;
    private const float MIN_DIRECTION_CHANGE_TIME = 1.0f;
    private const float MAX_DIRECTION_CHANGE_TIME = 3.0f;

    private Sprite.AnimatedSprite walkingRightSprite;
    private Sprite.AnimatedSprite walkingLeftSprite;
    private SharedTexture fireTexture;
    private List<Fireball> activeFireballs = new();
    private BowserCollisionHandler collisionHandler;

    private bool movingRight = true;
    private bool isAlive = true;
    private float fireCooldownTimer = 0f;
    private float velocityY = 0f;
    private int health = MAX_HEALTH;
    private static readonly System.Random random = new System.Random();
    private float directionChangeTimer;

    public Point Position
    {
        get => walkingRightSprite.Position;
        set
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
            return new Rectangle(Position.X, Position.Y, currentSprite.Size.X, currentSprite.Size.Y);
        }
    }

    public Bowser(SharedTexture EnemyTexture, SharedTexture FireTexture, int y, int x)
    {
        fireTexture = FireTexture;
        walkingRightSprite = EnemyTexture.NewAnimatedSprite(257, 368, 32, 32, 4, 0.2f);
        walkingLeftSprite = EnemyTexture.NewAnimatedSprite(117, 368, 32, 32, 2, 0.2f);
        walkingRightSprite.Visible = true;
        walkingLeftSprite.Visible = false;
        walkingRightSprite.Scale = SCALE;
        walkingLeftSprite.Scale = SCALE;
        Position = new Point(x, y);
        fireCooldownTimer = FIRE_COOLDOWN;
        directionChangeTimer = GetRandomDirectionTime();
        collisionHandler = new BowserCollisionHandler(this);
    }

    private float GetRandomDirectionTime()
    {
        return MIN_DIRECTION_CHANGE_TIME + (float)random.NextDouble() * (MAX_DIRECTION_CHANGE_TIME - MIN_DIRECTION_CHANGE_TIME);
    }

    public bool GetIsAlive() => isAlive;
    public bool IsMovingRight() => movingRight;

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            Kill();
            HUDAndScoring.HUD.Instance.SendEvent(new HUDAndScoring.GameEvent
            {
                EventType = HUDAndScoring.GameEventType.EnemyKilledByFireball,
                EventPosition = Position,
                Data = this
            });
        }
    }

    public void Update(GameTime gameTime)
    {
        if (!isAlive) return;
        if (Position.Y > Camera.Instance.Position.Y + Camera.Instance.WindowSize.Y) { health = 1; TakeDamage(); }
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

        velocityY += GRAVITY;
        Position = new Point(Position.X, Position.Y + (int)velocityY);

        walkingRightSprite.Visible = movingRight;
        walkingLeftSprite.Visible = !movingRight;
    }

    private void ShootFireball()
    {
        var fireball = new Fireball(
            fireTexture.NewAnimatedSprite(161, 253, 24, 8, 2, 0.1f),
            fireTexture.NewAnimatedSprite(101, 253, 24, 8, 2, 0.1f),
            Position.X, Position.Y, movingRight);
        activeFireballs.Add(fireball);
        CollisionController.Instance.AddEnemy(fireball);
        SoundController.PlaySound(SoundType.BowserFires, 1.0f);
    }

    private void Move(GameTime gameTime)
    {
        var sec = gameTime.ElapsedGameTime.TotalSeconds;
        var dx = (int)(sec * VELOCITY);

        directionChangeTimer -= (float)sec;
        if (directionChangeTimer <= 0)
        {
            movingRight = !movingRight;
            directionChangeTimer = GetRandomDirectionTime();
        }

        if (movingRight)
            Position = new Point(Position.X + dx, Position.Y);
        else
            Position = new Point(Position.X - dx, Position.Y);
    }

    public void Kill()
    {
        isAlive = false;
        walkingRightSprite.Drop();
        walkingLeftSprite.Drop();
        CollisionController.Instance.RemoveEnemy(this);
    }

    public void ResetGravity() => velocityY = 0;

    public void UnCollide(Rectangle intersect, CollideDirection direction)
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

    // Camera handles drawing
    public void Draw(SpriteBatch _spriteBatch) { }

    public void OnCollidePlayer(Player player, CollideDirection direction) => collisionHandler.OnCollidePlayer(player, direction);
    public void OnCollideItem(IItems item, CollideDirection direction) => collisionHandler.OnCollideItem(item, direction);
    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) => collisionHandler.OnCollideEnemy(enemy, direction);
    public void OnCollideBlock(IBlock block, CollideDirection direction) => collisionHandler.OnCollideBlock(block, direction);
}