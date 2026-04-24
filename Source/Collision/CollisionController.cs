using System;
using System.Collections.Generic;
using System.Linq;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;


namespace MagicBrosMario.Source.Collision;

// ReSharper disable ConvertIfStatementToReturnStatement
public class CollisionController
{
    private ICollidable player;
    private readonly HashSet<ICollidable> items = [];
    private readonly HashSet<ICollidable> enemies = [];
    private readonly HashSet<ICollidable> blocks = [];

    public static CollisionController Instance { get; } = new();

    public void BindPlayer<TCollidablePlayer>(TCollidablePlayer collidablePlayer)
        where TCollidablePlayer : Player, ICollidable
    {
        player = collidablePlayer;
    }

    public void AddItem<TCollidableItem>(TCollidableItem collidableItem) where TCollidableItem : IItems, ICollidable
    {
        items.Add(collidableItem);
    }

    public void AddBlock<TCollidableBlock>(TCollidableBlock collidableBlock)
        where TCollidableBlock : IBlock, ICollidable
    {
        blocks.Add(collidableBlock);
    }

    public void AddEnemy<TCollidableEnemy>(TCollidableEnemy collidableEnemy)
        where TCollidableEnemy : IEnemy, ICollidable
    {
        enemies.Add(collidableEnemy);
    }

    public void RemoveItem<TCollidableItem>(TCollidableItem collidableItem)
        where TCollidableItem : IItems, ICollidable
    {
        items.Remove(collidableItem);
    }

    public void RemoveBlock<TCollidableBlock>(TCollidableBlock collidableBlock)
        where TCollidableBlock : IBlock, ICollidable
    {
        blocks.Remove(collidableBlock);
    }

    public void RemoveEnemy<TCollidableEnemy>(TCollidableEnemy collidableEnemy)
        where TCollidableEnemy : IEnemy, ICollidable
    {
        enemies.Remove(collidableEnemy);
    }

    public void RemovePlayer()
    {
        player = null;
    }

    public void RemoveAll()
    {
        items.Clear();
        enemies.Clear();
        blocks.Clear();
    }

    public void Update(GameTime gameTime)
    {
        {
            // player
            CheckCollisions(player, blocks, (a, b, aDir, bDir) =>
            {
                a.OnCollideBlock(b as IBlock, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });

            CheckCollisions(player, items, (a, b, aDir, bDir) =>
            {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });

            CheckCollisions(player, enemies, (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });
        }

        HashSet<ICollidable> seen = [];

        foreach (var block in blocks)
        {
            CheckCollisions(block, items, (a, b, aDir, bDir) =>
            {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollideBlock(a as IBlock, bDir);
            });

            CheckCollisions(block, enemies, (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideBlock(a as IBlock, bDir);
            });
        }

        seen.Clear();

        foreach (var item in items)
        {
            CheckCollisions(item, items.Except(seen), (a, b, aDir, bDir) =>
            {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollideItem(a as IItems, bDir);
            });
            seen.Add(item);

            CheckCollisions(item, enemies, (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideItem(a as IItems, bDir);
            });
        }

        seen.Clear();

        foreach (var enemy in enemies)
        {
            CheckCollisions(enemy, enemies.Except(seen), (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideEnemy(a as IEnemy, bDir);
            });
            seen.Add(enemy);
        }
    }

    private static void CheckCollisions(
        ICollidable a, IEnumerable<ICollidable> others,
        Action<ICollidable, ICollidable, CollideDirection, CollideDirection> onCollide
    )
    {
        foreach (var b in others)
        {
            if (a == b) continue;
            var collide = IsColliding(a, b);

            if (!collide.HasValue) continue;

            var (dirA, dirB) = collide.Value;
            onCollide(a, b, dirA, dirB);
        }
    }

    private static (CollideDirection, CollideDirection)? IsColliding(ICollidable a, ICollidable b)
    {
        var dx = (b.CollisionBox.Width / 2f + b.CollisionBox.X) - (a.CollisionBox.Width / 2f + a.CollisionBox.X);
        var dy = (b.CollisionBox.Height / 2f + b.CollisionBox.Y) - (a.CollisionBox.Height / 2f + a.CollisionBox.Y);

        var limitX = ((float)a.CollisionBox.Width + b.CollisionBox.Width) / 2;
        var limitY = ((float)a.CollisionBox.Height + b.CollisionBox.Height) / 2;

        // return if no overlap
        if (Math.Abs(dx) >= limitX) return null;
        if (Math.Abs(dy) >= limitY) return null;

        // if normalized dx is greater than normalized dy
        if (Math.Abs(dx) / limitX > Math.Abs(dy) / limitY)
        {
            if (dx > 0)
            {
                return (CollideDirection.Right, CollideDirection.Left);
            } else
                return (CollideDirection.Left, CollideDirection.Right);
        } else
        {
            if (dy > 0)
            {
                return (CollideDirection.Down, CollideDirection.Top);
            } else
            {
                return (CollideDirection.Top, CollideDirection.Down);
            }
        }
    }
}