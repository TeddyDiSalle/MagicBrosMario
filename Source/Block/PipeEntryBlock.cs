using System;
using System.Collections.Generic;
using System.Diagnostics;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// 
/// </summary>
/// <param name="sprite"></param>
/// <param name="half"></param>
/// <param name="facing"></param>
/// <param name="exit">pass in null to make this pipe an one way end</param>
public class PipeEntryBlock(
    ISprite sprite,
    PipeEntryBlock.PipeDirection facing,
    PipeEntryBlock.PipeDirection half,
    Point? exit,
    PipeEntryBlock.PipeDirection? exitDirection
)
    : BlockBase<PipeEntryBlock>(sprite)
{
    public enum PipeDirection
    {
        Left,
        Right,
        Up,
        Down,
    }

    /// <summary>
    /// if the pipe is facing left, the pipe needs to be on the right of mario
    /// </summary>
    private static readonly Dictionary<PipeDirection, CollideDirection> MatchingDirections =
        new()
        {
            { PipeDirection.Left, CollideDirection.Right },
            { PipeDirection.Right, CollideDirection.Left },
            { PipeDirection.Up, CollideDirection.Down },
            { PipeDirection.Down, CollideDirection.Top },
        };

    public PipeDirection Facing { get; } = facing;

    /// <summary>
    /// up and down facing pipes can have left and right halves
    /// left and right facing pipes can have up and down halves
    /// </summary>
    public PipeDirection Half { get; } = half;

    public Point? Exit { get; } = exit;
    
    public PipeDirection? ExitDirection { get; } = exitDirection;

    /// <summary>
    /// check if mario is walking toward the pipe before calling this
    /// or else mario will get teleported between pipes
    /// </summary>
    /// <param name="collideDirection">collide direction from Mario.OnCollideBlock</param>
    /// <param name="mario">mario's hitbox</param>
    /// <returns>location to teleport mario too or null if no teleportation is needed</returns>
    public Point? CanEnter(CollideDirection collideDirection, Rectangle mario)
    {
        if (MatchingDirections[Facing] != collideDirection) return null;
        // skip check for exit only pipes
        if (Exit == null) return null;

        Point? result = null;
        switch (Facing)
        {
            case PipeDirection.Left:
            case PipeDirection.Right:
            {
                var pipeTop = CollisionBox.Top;
                if (Half == PipeDirection.Down)
                {
                    pipeTop -= CollisionBox.Height;
                }

                var pipeHeight = 2 * CollisionBox.Height;
                var maxOffset = pipeHeight - mario.Height;
                var marioOffset = mario.Top - pipeTop;
                

                if (0 <= marioOffset && marioOffset <= maxOffset)
                {
                    result = Exit;
                }

                break;
            }
            case PipeDirection.Up:
            case PipeDirection.Down:
            {
                var pipeLeft = CollisionBox.Left;
                if (Half == PipeDirection.Right)
                {
                    pipeLeft -= CollisionBox.Width;
                }

                var pipeWidth = 2 * CollisionBox.Width;
                var maxOffset = pipeWidth - mario.Width;
                var marioOffset = mario.Left - pipeLeft;

                if (0 <= marioOffset && marioOffset <= maxOffset)
                {
                    result = Exit;
                }

                break;
            }
            default:
                throw new Exception("impossible default branch");
        }

        return result;
    }

    public override void OnCollidePlayer(Player player, CollideDirection direction) { }

    public override void OnCollideItem(IItems item, CollideDirection direction) { }

    public override void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

    public override void OnCollideBlock(IBlock block, CollideDirection direction) { }
}