using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

namespace MagicBrosMario.Source;

public class Koopa : IEnemy, ICollidable
{
    private const int VELOCITY = 100;
    private const int SHELL_VELOCITY = 200;
    private const float RECOVERY_TIME = 3.0f;
    private const float SCALE = 2f;
    private const float GRAVITY = 0.35f;
    
    private Sprite.ISprite[] sprites;
    private float velocityY = 0f;

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

    private int CurrentSpriteIndex()
    {
        return state switch
        {
            KoopaState.WalkingAlive => movingRight ? WALKING_RIGHT : WALKING_LEFT,
            KoopaState.ShellIdle => SHELL_IDLE,
            KoopaState.ShellMoving => SHELL_MOVING,
            KoopaState.Stomped => STOMPED,
            KoopaState.Dead => SHELL_DEATH,
            _ => WALKING_RIGHT
        };
    }

    public bool IsShellMoving()
    {
        return state == KoopaState.ShellMoving;
    }

    public Point Position
    {
        get => sprites[0].Position;
        set 
        { 
            foreach (var sprite in sprites) sprite.Position = value;
        }
    }

    public Rectangle CollisionBox
    {
        get
        {
            var sprite = sprites[CurrentSpriteIndex()];
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public Koopa(SharedTexture EnemyTexture, int y, int leftBound)
    {
        int Y = y;
        sprites = [
            EnemyTexture.NewAnimatedSprite(296, 206, 18, 25, 2, 0.2f),
            EnemyTexture.NewAnimatedSprite(182, 206, 18, 25, 2, 0.2f),
            EnemyTexture.NewSprite(144, 216, 16, 14),
            EnemyTexture.NewSprite(144, 216, 16, 14),
            EnemyTexture.NewSprite(163, 215, 16, 15),
            EnemyTexture.NewSprite(334, 215, 16, 15),
        ];
        foreach (var sprite in sprites)
        {
            sprite.Scale = SCALE;
            sprite.Visible = false;
        }
        Position = new Point(leftBound, Y);
        this.state = KoopaState.WalkingAlive;
    }

    public bool GetIsAlive()
    {
        return isAlive;
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

        velocityY += GRAVITY;
        Position = new Point(Position.X, Position.Y + (int)velocityY);

        foreach (var sprite in sprites) sprite.Visible = false;
        sprites[CurrentSpriteIndex()].Visible = true;
    }

    private void Move(GameTime gameTime, int velocity)
    {
        var sec = gameTime.ElapsedGameTime.TotalSeconds;
        var dx = (int)(sec * velocity);

        if (movingRight)
        {
            Position = new Point(Position.X + dx, Position.Y);
        }
        else
        {
            Position = new Point(Position.X - dx, Position.Y);
        }
    }

    public void Kill()
    {
        if (state == KoopaState.WalkingAlive) { state = KoopaState.ShellIdle; shellTimer = 0f; }
        else if (state != KoopaState.Dead)
        {
            state = KoopaState.Dead;
            isAlive = false;
            foreach (var sprite in sprites)
            {
                sprite.Drop();
            }
            CollisionController.Instance.RemoveEnemy(this);
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
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        if (enemy is Bowser || (enemy is Koopa koopa && koopa.IsShellMoving()))
        {
            Kill();
            return;
        }
        if (direction == CollideDirection.Left || direction == CollideDirection.Right)
        {
            if (state == KoopaState.WalkingAlive)
            {
                UnCollide(Rectangle.Intersect(CollisionBox, enemy.CollisionBox), direction);
            }
        }
    }

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
            {
                if (state == KoopaState.WalkingAlive || state == KoopaState.ShellMoving)
                {
                    UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
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

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item is MarioFireball)
        {
            Kill();
        }
    }
}