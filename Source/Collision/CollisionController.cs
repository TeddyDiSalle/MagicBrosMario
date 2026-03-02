using System;
using System.Collections.Generic;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Collision;

public class CollisionController {
    private ICollidable player;
    private readonly List<ICollidable> items;
    private readonly List<ICollidable> enemies;
    private readonly List<ICollidable> blocks;

    public CollisionController() {
        items = [];
        enemies = [];
        blocks = [];
    }

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
        // loop through each player, items, block and enemies
        // send OnPlayerCollide/OnEnemyCollide/OnBlockCollide
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
