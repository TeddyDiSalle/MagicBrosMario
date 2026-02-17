using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source;

public class Koopa : IEnemy
{
    private const int VELOCITY = 100;
    private const int SHELL_VELOCITY = 200;
    private const float RECOVERY_TIME = 3.0f;
    
    private readonly int leftBound;
    private readonly int rightBound;
    private Sprite.ISprite[] sprites;

    private const int WALKING_RIGHT = 0;
    private const int WALKING_LEFT = 1;
    private const int SHELL_IDLE = 2;
    private const int SHELL_MOVING = 3;
    private const int STOMPED = 4;
    private const int SHELL_DEATH = 5;

    private enum KoopaState
    {
        WalkingAlive,
        ShellIdle,
        ShellMoving,
        Stomped,
        Dead
    }

    private KoopaState state;
    private Boolean movingRight = true;
    private float shellTimer = 0f;

    private Sprite.ISprite CurrentSprite()
    {
        switch (state)
        {
            case KoopaState.WalkingAlive:
                return movingRight ? sprites[WALKING_RIGHT] : sprites[WALKING_LEFT];
            case KoopaState.ShellIdle:
                return sprites[SHELL_IDLE];
            case KoopaState.ShellMoving:
                return sprites[SHELL_MOVING];
            case KoopaState.Stomped:
                return sprites[STOMPED];
            case KoopaState.Dead:
                return sprites[SHELL_DEATH];
            default:
                return sprites[WALKING_RIGHT];
        }
    }

    public Point Position
    {
        get { return CurrentSprite().Position; }
        private set 
        { 
            foreach (var sprite in sprites)
            {
                sprite.Position = value;
            }
        }
    }

    public Koopa(
        Sprite.AnimatedSprite walkingRightSprite,
        Sprite.AnimatedSprite walkingLeftSprite,
        Sprite.Sprite shellIdleSprite,
        Sprite.Sprite shellMovingSprite,
        Sprite.Sprite stompedSprite,
        Sprite.Sprite shellDeathSprite,
        int Y, 
        int leftBound, 
        int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        
        sprites = [
            walkingRightSprite,
            walkingLeftSprite,
            shellIdleSprite,
            shellMovingSprite,
            stompedSprite,
            shellDeathSprite
        ];

        Position = new Point(leftBound, Y);
        this.state = KoopaState.WalkingAlive;
    }

    public void Update(GameTime gametime)
    {
        if (state == KoopaState.ShellIdle)
        {
            shellTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            
            if (shellTimer >= RECOVERY_TIME)
            {
                state = KoopaState.Stomped;
            }
        }
        else if (state == KoopaState.Stomped)
        {
            shellTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            
            if (shellTimer >= RECOVERY_TIME + 0.5f)
            {
                state = KoopaState.WalkingAlive;
                shellTimer = 0f;
            }
        }

        if (state == KoopaState.WalkingAlive)
        {
            Move(gametime, VELOCITY);
        }
        else if (state == KoopaState.ShellMoving)
        {
            Move(gametime, SHELL_VELOCITY);
        }

        CurrentSprite().Update(gametime);
    }

    private void Move(GameTime gameTime, int velocity)
    {
        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        var dx = (int)(sec * velocity);

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

    public void Kill()
    {
        if (state == KoopaState.WalkingAlive)
        {
            state = KoopaState.ShellIdle;
            shellTimer = 0f;
        }
        else if (state == KoopaState.ShellIdle || state == KoopaState.ShellMoving || state == KoopaState.Stomped)
        {
            state = KoopaState.Dead;
        }
    }

    public void KickShell(bool kickRight)
    {
        if (state == KoopaState.ShellIdle)
        {
            state = KoopaState.ShellMoving;
            movingRight = kickRight;
            shellTimer = 0f;
        }
    }

    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite().Draw(_spriteBatch);
    }
}