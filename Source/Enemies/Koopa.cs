using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;

public class Koopa : IEnemy, ICollidable
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
    public Boolean isAlive = true;

    private enum KoopaState { WalkingAlive, ShellIdle, ShellMoving, Stomped, Dead }

    private KoopaState state;
    private bool movingRight = true;
    private float shellTimer = 0f;


    private Sprite.ISprite CurrentSprite()
    {
        return state switch
        {
            KoopaState.WalkingAlive => movingRight ? sprites[WALKING_RIGHT] : sprites[WALKING_LEFT],
            KoopaState.ShellIdle => sprites[SHELL_IDLE],
            KoopaState.ShellMoving => sprites[SHELL_MOVING],
            KoopaState.Stomped => sprites[STOMPED],
            KoopaState.Dead => sprites[SHELL_DEATH],
            _ => sprites[WALKING_RIGHT]
        };
    }

    public Point Position
    {
        get => CurrentSprite().Position;
        private set 
        { 
            foreach (var sprite in sprites) sprite.Position = value;
        }
    }

    public Rectangle CollisionBox
    {
        get
        {
            var sprite = CurrentSprite();
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public Koopa(
        Sprite.AnimatedSprite walkingRightSprite,
        Sprite.AnimatedSprite walkingLeftSprite,
        Sprite.Sprite shellIdleSprite,
        Sprite.Sprite shellMovingSprite,
        Sprite.Sprite stompedSprite,
        Sprite.Sprite shellDeathSprite,
        int Y, int leftBound, int rightBound)
    {
        this.leftBound = leftBound;
        this.rightBound = rightBound;
        sprites = [walkingRightSprite, walkingLeftSprite, shellIdleSprite, shellMovingSprite, stompedSprite, shellDeathSprite];
        Position = new Point(leftBound, Y);
        this.state = KoopaState.WalkingAlive;
    }

    public void Update(GameTime gametime)
    {
        if (state == KoopaState.ShellIdle)
        {
            shellTimer += (float)gametime.ElapsedGameTime.TotalSeconds;
            if (shellTimer >= RECOVERY_TIME) state = KoopaState.Stomped;
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

        if (state == KoopaState.WalkingAlive) Move(gametime, VELOCITY);
        else if (state == KoopaState.ShellMoving) Move(gametime, SHELL_VELOCITY);

        CurrentSprite().Update(gametime);
    }

    private void Move(GameTime gameTime, int velocity)
    {
        var sec = gameTime.ElapsedGameTime.TotalSeconds;
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
        if (state == KoopaState.WalkingAlive) { state = KoopaState.ShellIdle; shellTimer = 0f; }
        else if (state != KoopaState.Dead) { state = KoopaState.Dead; isAlive = false; }
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

    public void Draw(SpriteBatch _spriteBatch) => CurrentSprite().Draw(_spriteBatch);

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        

        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (state == KoopaState.WalkingAlive)
            {
                int pushDistance = 15;
                if (direction == CollideDirection.Left)
                {
                    movingRight = true; // Hit on left, go right
                    Position = new Point(Position.X + pushDistance, Position.Y);
                }
                else
                {
                    movingRight = false; // Hit on right, go left
                    Position = new Point(Position.X - pushDistance, Position.Y);
                }
                Console.WriteLine("Koopa bumped enemy and changed direction.");
            }
            else if (state == KoopaState.ShellMoving)
            {
                enemy.Kill();
            }
        }
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            // Only turn if walking or a moving shell
            if (state == KoopaState.WalkingAlive || state == KoopaState.ShellMoving)
            {
                int pushDistance = 15;

                if (direction == CollideDirection.Left)
                {
                    movingRight = true;
                    Position = new Point(Position.X + pushDistance, Position.Y);
                }
                else if (direction == CollideDirection.Right)
                {
                    movingRight = false;
                    Position = new Point(Position.X - pushDistance, Position.Y);
                }
                
                
            }
        }
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (state == KoopaState.Dead) return;

        if (direction == CollideDirection.Top)
        {
            if (state == KoopaState.WalkingAlive) Kill();
            else if (state == KoopaState.ShellIdle || state == KoopaState.Stomped) KickShell(player.Position.X < Position.X);
            else if (state == KoopaState.ShellMoving) { state = KoopaState.ShellIdle; shellTimer = 0f; }
        }
        else
        {
            if (state == KoopaState.ShellIdle || state == KoopaState.Stomped) KickShell(direction == CollideDirection.Left);
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction) { }
}