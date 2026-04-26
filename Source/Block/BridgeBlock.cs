using System.Collections.Generic;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Block;

public class BridgeBlock(ISprite sprite, int group, int order) : BlockBase<BrickBlock>(sprite)
{
    // in millis
    private const int BridgeBreakTimeDiff = 100;
    private static readonly Dictionary<int, int> GroupCounter = new();

    public static void CollapseBridge(int group)
    {
        GroupCounter[group] = 1;
    }
    
    public static void ResetBridgeGroup(int group)
    {
        GroupCounter[group] = 0;
    }

    private int bridgeBreakProgress = 0;

    public override void Update(GameTime gameTime)
    {
        GroupCounter.TryAdd(group, 0);

        if (GroupCounter[group] != order)
            return;

        bridgeBreakProgress += gameTime.ElapsedGameTime.Milliseconds;

        if (bridgeBreakProgress < BridgeBreakTimeDiff)
            return;
        
        GroupCounter[group] += 1;
        
        Sprite.Drop();
        Sprite = null;
        CollisionController.Instance.RemoveBlock(this);
    }


    public override void OnCollidePlayer(Player player, CollideDirection direction) { }
    public override void OnCollideItem(IItems item, CollideDirection direction) { }
    public override void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
    public override void OnCollideBlock(IBlock block, CollideDirection direction) { }
}