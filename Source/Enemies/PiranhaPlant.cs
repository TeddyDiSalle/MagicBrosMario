using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

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
    public Boolean isAlive;

    public Point Position
    {
        get { return aliveSprite.Position; }
        private set { aliveSprite.Position = value; }
    }
    public bool GetIsAlive() => isAlive;

    public Rectangle CollisionBox
    {
        get
        {
            if (!isAlive) return Rectangle.Empty;

            int exposedHeight = hiddenY - Position.Y;

            if (exposedHeight <= 0) return Rectangle.Empty;

            return new Rectangle(
                Position.X + 4,
                Position.Y, 
                aliveSprite.Size.X - 8, 
                exposedHeight
            );
        }
    }

    public PiranhaPlant(SharedTexture EnemyTexture, int pipeX, int pipeY)
    {
        this.hiddenY = pipeY;
        this.visibleY = pipeY - RISE_HEIGHT;

        this.aliveSprite = EnemyTexture.NewAnimatedSprite(125, 180, 16, 23, 2, 0.2f);

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

        if (pauseTimer > 0)
        {
            pauseTimer -= deltaTime;
            aliveSprite.Update(gameTime);
            return;
        }

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

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item != null)//if its mario fireball
        {
            //Kill();
        }
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
    }
}