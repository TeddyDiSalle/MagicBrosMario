using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Collision;

public interface ICollidable
{
    Rectangle CollisionBox { get; }
    void OnCollidePlayer(Player player, CollideDirection direction); 
    void OnCollideItem(IItems item, CollideDirection direction); 
    void OnCollideEnemy(IEnemy enemy, CollideDirection direction); 
    void OnCollideBlock(IBlock block, CollideDirection direction); 
}