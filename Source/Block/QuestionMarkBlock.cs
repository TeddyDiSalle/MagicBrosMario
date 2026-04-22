using System;
using System.Diagnostics;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source.Block;

public class QuestionMarkBlock : BlockBase<QuestionMarkBlock>
{
    public enum InnerItem
    {
        Coin,
        Star,
        OneUp,
        Mushroom,
        PoisonMushroom,
    }

    public Rectangle CollisionBox => new(sprite.Position.X, sprite.Position.Y, sprite.Size.X, sprite.Size.Y);

    private bool empty = false;
    private AnimatedSprite sprite;
    private readonly Sprite.Sprite emptySprite;
    private readonly InnerItem innerItem;

    public QuestionMarkBlock(AnimatedSprite sprite, Sprite.Sprite emptySprite, InnerItem innerItem) : base(sprite)
    {
        this.sprite = sprite;
        this.emptySprite = emptySprite;
        this.innerItem = innerItem;
        sprite.Visible = true;
        emptySprite.Visible = false;
    }

    public override void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (empty) return;

        // if mario does not collide from below
        if (direction != CollideDirection.Down) return;

        // drop the sprite and allow gc to clean it up
        sprite.Drop();
        sprite = null;

        // spawn coins and change sprite
        Sprite = emptySprite;
        Sprite.Visible = true;
        empty = true;

        IItems item = null;
        switch (innerItem)
        {
            case InnerItem.Coin:
				item = new Coin(MagicBrosMario.INSTANCE.ItemTexture, Position.X + 8, Position.Y);
				HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.CoinCollected, EventPosition = Position});
				break;
            case InnerItem.Star:
                item = new Star(MagicBrosMario.INSTANCE.ItemTexture, Position.X + 1, Position.Y - 5);
                HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.PowerupAppears, EventPosition = Position });
                break;
            case InnerItem.OneUp:
                item = new OneUp(MagicBrosMario.INSTANCE.ItemTexture, Position.X, Position.Y - 5);
                HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.PowerupAppears, EventPosition = Position });
                break;
			case InnerItem.Mushroom:
				if (player.GetCurrentPower() == Power.None)
				{
					item = new Mushroom(MagicBrosMario.INSTANCE.ItemTexture, Position.X, Position.Y - 5);
				}
				else
				{
					item = new Fireflower(MagicBrosMario.INSTANCE.ItemTexture, Position.X, Position.Y - 3);
				}
				HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.PowerupAppears, EventPosition = Position });
				break;
			case InnerItem.PoisonMushroom:
					item = new PoisonMushroom(MagicBrosMario.INSTANCE.ItemTexture, Position.X, Position.Y - 5);
				HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.PowerupAppears, EventPosition = Position });
				break;
			default:
                throw new Exception("impossible default branch");
        }

        MagicBrosMario.INSTANCE.level.AddItem(item);
    }

    public override void OnCollideItem(IItems item, CollideDirection direction)
    {
        // never gonna pick coin up, never gonna put coin down
    }

    public override void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        // enemies don't have wallet, they can't pick up coins
    }

    public override void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        // this shouldn't even be called
    }
}