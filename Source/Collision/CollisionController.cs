using System;
using System.Collections.Generic;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Collision;

public class CollisionController {
    private ICollidable player;
    private readonly List<ICollidable> items = [];
    private readonly List<ICollidable> enemies = [];
    private readonly List<ICollidable> blocks = [];

    public CollisionController Instance { get; } = new CollisionController();

    public void BindPlayer<TCollidablePlayer>(TCollidablePlayer collidablePlayer)
        where TCollidablePlayer : Player, ICollidable {
        player = collidablePlayer;
    }

    public void AddItem<TCollidableItem>(TCollidableItem collidableItem) where TCollidableItem : IItems, ICollidable {
        items.Add(collidableItem);
    }

    public void AddBlock<TCollidableBlock>(TCollidableBlock collidableBlock)
        where TCollidableBlock : IBlock, ICollidable {
        blocks.Add(collidableBlock);
    }

    public void AddEnemy<TCollidableEnemy>(TCollidableEnemy collidableEnemy)
        where TCollidableEnemy : IEnemy, ICollidable {
        enemies.Add(collidableEnemy);
    }

    public void Update(GameTime gameTime) {
        {
            // player
            CheckCollisions(player, blocks, (a, b, aDir, bDir) => {
                a.OnCollideBlock(b as IBlock, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });

            CheckCollisions(player, items, (a, b, aDir, bDir) => {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });

            CheckCollisions(player, enemies, (a, b, aDir, bDir) => {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollidePlayer(a as Player, bDir);
            });
        }

        foreach (var block in blocks) {
            // block

            CheckCollisions(block, items, (a, b, aDir, bDir) => {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollideBlock(a as IBlock, bDir);
            });

            CheckCollisions(block, enemies, (a, b, aDir, bDir) => {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideBlock(a as IBlock, bDir);
            });
        }

        foreach (var item in items) {
            CheckCollisions(item, items, (a, b, aDir, bDir) => {
                a.OnCollideItem(b as IItems, aDir);
                b.OnCollideItem(a as IItems, bDir);
            });

            CheckCollisions(item, enemies, (a, b, aDir, bDir) => {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideItem(a as IItems, bDir);
            });
        }
        
        foreach (var enemy in enemies) {
            CheckCollisions(enemy, enemies, (a, b, aDir, bDir) => {
                a.OnCollideEnemy(b as IEnemy, aDir);
                b.OnCollideEnemy(a as IEnemy, bDir);
            });
        }
    }

    private static void CheckCollisions(
        ICollidable a, IEnumerable<ICollidable> others,
        Action<ICollidable, ICollidable, CollideDirection, CollideDirection> onCollide
    ) {
        foreach (var b in others) {
            if (a == b) continue;
            var collide = IsColliding(a, b);
            if (collide.HasValue) {
                var (dirA, dirB) = collide.Value;
                onCollide(a, b, dirA, dirB);
            }
        }
    }

    private static (CollideDirection, CollideDirection)? IsColliding(ICollidable a, ICollidable b) {
        var dx = (b.CollisionBox.X + b.CollisionBox.Width / 2) - (a.CollisionBox.X + a.CollisionBox.Width / 2);
        var dy = (b.CollisionBox.Y + b.CollisionBox.Height / 2) - (a.CollisionBox.Y + a.CollisionBox.Height / 2);

        var limitX = (a.CollisionBox.Width + b.CollisionBox.Width) / 2;
        var limitY = (a.CollisionBox.Height + b.CollisionBox.Height) / 2;

        // return if no overlap
        if (Math.Abs(dx) > limitX) return null;
        if (Math.Abs(dy) > limitY) return null;

        // if normalized dx is greater than normalized dy
        if (Math.Abs(dx) / limitX > Math.Abs(dy) / limitY) {
            if (dx > 0) {
                return (CollideDirection.Right, CollideDirection.Left);
            } else
                return (CollideDirection.Left, CollideDirection.Right);
        } else {
            if (dy > 0) {
                return (CollideDirection.Down, CollideDirection.Top);
            } else {
                return (CollideDirection.Top, CollideDirection.Down);
            }
        }
    }
}