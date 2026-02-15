using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source;

public class PiranhaPlant : IEnemy
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
                pauseTimer = PAUSE_DURATION; // Pause at top
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
                pauseTimer = PAUSE_DURATION; // Pause at bottom
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
}