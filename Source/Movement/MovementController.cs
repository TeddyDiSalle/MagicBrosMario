using System;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Movement;

public class MovementController() {

    public Vector2 Position { get; set; } = Vector2.Zero;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public bool OnGround { get; private set; } = false;
    
    // these values need to be tuned
    private const float DefaultGravity = 10f;
    private const float DefaultHorizontalAcceleration = 10f;
    private const float DefaultMaxSpeed = 10f;
    private const float DefaultJumpingStrength = 10f;

    private float Gravity { get; set; } = DefaultGravity;
    private float HorizontalAcceleration { get; set; } = DefaultHorizontalAcceleration;
    private float MaxSpeed { get; set; } = DefaultMaxSpeed;
    private float JumpingStrength { get; set; } = DefaultJumpingStrength;

    private MovingDirection moving = MovingDirection.None;

    public void Update(GameTime gameTime) {
        float time = (float)gameTime.ElapsedGameTime.Milliseconds * 1000;

        // only apply gravity if it is not on ground
        if (!OnGround) {
            Velocity += time * new Vector2(0, Gravity);
        }

        Velocity = moving switch {
            MovingDirection.None =>
                // slow down
                Velocity.X switch {
                    > 0 => new Vector2(float.Max(0, Velocity.X - time * HorizontalAcceleration), Velocity.Y),
                    < 0 => new Vector2(float.Min(0, Velocity.X + time * HorizontalAcceleration), Velocity.Y),
                    _ => Velocity
                },
            MovingDirection.Left => new Vector2(
                float.Max(-MaxSpeed, Velocity.X - time * HorizontalAcceleration),
                Velocity.Y),
            MovingDirection.Right => new Vector2(
                float.Min(MaxSpeed, Velocity.X + time * HorizontalAcceleration),
                Velocity.Y),
            _ => throw new ArgumentOutOfRangeException()
        };

        Position += time * Velocity;

        OnGround = false;
        moving = MovingDirection.None;
    }

    private enum MovingDirection {
        None,
        Left,
        Right
    }

    public void Jump() {
        Velocity = new Vector2(Velocity.X, JumpingStrength);
    }

    public void MoveLeft() {
        moving = MovingDirection.Left;
    }

    public void MoveRight() {
        moving = MovingDirection.Right;
    }

    public void HitGround() {
        Velocity = new Vector2(Velocity.X, 0);
        OnGround = true;
    }

    public void HitCeiling() {
        Velocity = new Vector2(Velocity.X, 0);
        OnGround = true;
    }

    public void HitLeftWall() {
        if (Velocity.X < 0)
            Velocity = new Vector2(0, Velocity.Y);
    }

    public void HitRightWall() {
        if (Velocity.X > 0)
            Velocity = new Vector2(0, Velocity.Y);
    }
}