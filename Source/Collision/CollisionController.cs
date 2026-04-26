using System;
using System.Collections.Generic;
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

    private readonly List<ICollidable> _enemyList = [];
    private readonly List<ICollidable> _itemList = [];
    private readonly List<ICollidable> _blockList = [];

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
        _enemyList.Clear();
        _enemyList.AddRange(enemies);

        _itemList.Clear();
        _itemList.AddRange(items);

        _blockList.Clear();
        _blockList.AddRange(blocks);

        {
            // player
            CheckCollisions(player, _blockList, (a, b, aDir, bDir) =>
            {
                a.OnCollideBlock(b as IBlock, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });

            CheckCollisions(player, _itemList, (a, b, aDir, bDir) =>
            {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });

            CheckCollisions(player, _enemyList, (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });
        }

        foreach (var block in _blockList)
        {
            CheckCollisions(block, _itemList, (a, b, aDir, bDir) =>
            {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollideBlock(a as IBlock, bDir);
            });

            CheckCollisions(block, _enemyList, (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideBlock(a as IBlock, bDir);
            });
        }

        for (int i = 0; i < _itemList.Count; i++)
        {
            var item = _itemList[i];

            for (int j = i + 1; j < _itemList.Count; j++)
            {
                var collide = IsColliding(item, _itemList[j]);
                if (!collide.HasValue) continue;
                var (dirA, dirB) = collide.Value;
                item.OnCollideItem(_itemList[j] as IItems, dirA);
                _itemList[j].OnCollideItem(item as IItems, dirB);
            }

            CheckCollisions(item, _enemyList, (a, b, aDir, bDir) =>
            {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideItem(a as IItems, bDir);
            });
        }

        for (int i = 0; i < _enemyList.Count; i++)
        {
            for (int j = i + 1; j < _enemyList.Count; j++)
            {
                var collide = IsColliding(_enemyList[i], _enemyList[j]);
                if (!collide.HasValue) continue;
                var (dirA, dirB) = collide.Value;
                _enemyList[i].OnCollideEnemy(_enemyList[j] as IEnemy, dirA);
                _enemyList[j].OnCollideEnemy(_enemyList[i] as IEnemy, dirB);
            }
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
        var boxA = a.CollisionBox;
        var boxB = b.CollisionBox;

        var dx = (boxB.Width / 2f + boxB.X) - (boxA.Width / 2f + boxA.X);
        var dy = (boxB.Height / 2f + boxB.Y) - (boxA.Height / 2f + boxA.Y);

        var limitX = ((float)boxA.Width + boxB.Width) / 2;
        var limitY = ((float)boxA.Height + boxB.Height) / 2;

        // return if no overlap
        if (Math.Abs(dx) >= limitX) return null;
        if (Math.Abs(dy) >= limitY) return null;

        // if normalized dx is greater than normalized dy
        if (Math.Abs(dx) / limitX > Math.Abs(dy) / limitY)
        {
            if (dx > 0)
            {
                return (CollideDirection.Right, CollideDirection.Left);
            }
            else
                return (CollideDirection.Left, CollideDirection.Right);
        }
        else
        {
            if (dy > 0)
            {
                return (CollideDirection.Down, CollideDirection.Top);
            }
            else
            {
                return (CollideDirection.Top, CollideDirection.Down);
            }
        }
    }
}