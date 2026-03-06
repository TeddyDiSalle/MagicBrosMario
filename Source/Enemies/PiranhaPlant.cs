using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;

public class PiranhaPlant : IEnemy, ICollidable
{
    private const float RISE_SPEED = 100f;
    private const float PAUSE_DURATION = 0f;
    private const int RISE_HEIGHT = 48;

    private Sprite.AnimatedSprite aliveSprite;
    private readonly int hiddenY;
    private readonly int visibleY;

    private enum PiranhaState
    {
        Rising,
        Lowering,
        Dead
    }

    private PiranhaState state;
    private float pauseTimer = 0f;
    private Boolean isAlive;

    public Point Position
    {
        get { return aliveSprite.Position; }
        private set { aliveSprite.Position = value; }
    }

    // ICollidable implementation
    public Rectangle CollisionBox
    {
        get
        {
            if (!isAlive)
            {
                return Rectangle.Empty; // No collision when dead
            }

            return new Rectangle(
                aliveSprite.Position.X,
                aliveSprite.Position.Y,
                aliveSprite.Size.X,
                aliveSprite.Size.Y
            );
        }
    }

    public PiranhaPlant(Sprite.AnimatedSprite aliveSprite, int pipeX, int pipeY)
    {
        this.hiddenY = pipeY;
        this.visibleY = pipeY - RISE_HEIGHT;

        this.aliveSprite = aliveSprite;

        Position = new Point(pipeX, hiddenY);
        this.isAlive = true;
        this.state = PiranhaState.Rising;
        this.pauseTimer = PAUSE_DURATION; 
    }

    public void Update(GameTime gameTime)
    {
        if (!isAlive)
        {
            return;
        }

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Handle pausing at top or bottom
        if (pauseTimer > 0)
        {
            pauseTimer -= deltaTime;
            aliveSprite.Update(gameTime);
            return;
        }

        // Move up or down
        if (state == PiranhaState.Rising)
        {
            int newY = Position.Y - (int)(RISE_SPEED * deltaTime);
            if (newY <= visibleY)
            {
                Position = new Point(Position.X, visibleY);
                state = PiranhaState.Lowering;
                pauseTimer = PAUSE_DURATION;
            }
            else
            {
                Position = new Point(Position.X, newY);
            }
        }
        else if (state == PiranhaState.Lowering)
        {
            int newY = Position.Y + (int)(RISE_SPEED * deltaTime);
            if (newY >= hiddenY)
            {
                Position = new Point(Position.X, hiddenY);
                state = PiranhaState.Rising;
                pauseTimer = PAUSE_DURATION;
            }
            else
            {
                Position = new Point(Position.X, newY);
            }
        }

        aliveSprite.Update(gameTime);
    }

    public void Kill()
    {
        this.isAlive = false;
        state = PiranhaState.Dead;
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        if (isAlive)
        {
            aliveSprite.Draw(_spriteBatch);
        }
    }

    // ICollidable methods
    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        // Piranha Plant damages player on any collision
        // In classic Mario, you can't stomp Piranha Plants
        // Player takes damage regardless of direction
        // Handle player damage here if needed
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        // Piranha Plant can be killed by fireballs
        if (item != null) // Check if it's a fireball or star
        {
            // You could add logic here to check item type
            // For now, any item collision kills it
            Kill();
        }
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        // Piranha Plants don't interact with other enemies
        // They're stationary and don't affect each other
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        // Piranha Plants don't collide with blocks
        // They're inside pipes
    }
}