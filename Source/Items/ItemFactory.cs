using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items;

public static class ItemFactory {
    private static readonly SharedTexture ITEM_SHARED_TEXTURE = new SharedTexture();
    public const string TEXTURE_NAME = "items";

    public static void BindTexture(Texture2D texture) {
        ITEM_SHARED_TEXTURE.BindTexture(texture);
    }

    public static IItems CreateCoin(int x, int y) {
        var item = new CollectableCoin(ITEM_SHARED_TEXTURE, x, y);
        return item;
    }
    public static IItems CreateMushroom(int x, int y) {
        var item = new Mushroom(ITEM_SHARED_TEXTURE, x, y, false);
        return item;
    }
    public static IItems CreateFireFlower(int x, int y) {
        var item = new Fireflower(ITEM_SHARED_TEXTURE, x, y);
        return item;
    }

    public static IItems CreateStar(int x, int y) {
        var item = new Star(ITEM_SHARED_TEXTURE, x, y);
        return item;
    }
    public static IItems CreateOneUp(int x, int y) {
        var item = new OneUp(ITEM_SHARED_TEXTURE, x, y, false);
        return item;
    }
    public static IItems CreateFlagPole(int x, int y) {
        int flagXOffset = -25;
        int flagYOffset = 20; 
        int flagPoleXOffset = 8; 
        int flagPoleYOffset = 15;
		FlagPole pole = new FlagPole(ITEM_SHARED_TEXTURE, x + flagPoleXOffset, y + flagPoleYOffset);
		var item = new Flag(ITEM_SHARED_TEXTURE, x + flagXOffset + flagPoleXOffset, y + flagYOffset + flagPoleYOffset, pole);
		return item;
    }

    public static IItems CreateAxe(int x, int y, int group) {
        var item = new Axe(ITEM_SHARED_TEXTURE, x, y, group);
        return item;
    }

    public static IItems CreateMovingPlatform(int x, int y, int size, bool goUp) {
        int direction = goUp ? -1 : 1;

        IItems item;
        switch (size)
        {   case 1:
                item = new MovingPlatform_Size1(ITEM_SHARED_TEXTURE, Camera.Instance.WindowSize.Y, x, y, direction);
                break;
            case 2:
                item = new MovingPlatform_Size2(ITEM_SHARED_TEXTURE, Camera.Instance.WindowSize.Y, x, y, direction);
                break;
            case 3:
                item = new MovingPlatform_Size3(ITEM_SHARED_TEXTURE, Camera.Instance.WindowSize.Y, x, y, direction);
                break;
            default:
                item = new MovingPlatform_Size1(ITEM_SHARED_TEXTURE, Camera.Instance.WindowSize.Y, x, y, direction);
                break;
        }
        return item;
    }

    public static IItems CreateAntiGravityCloud(int x, int y) {
        var item = new AntiGravityCloud(ITEM_SHARED_TEXTURE, x, y);
        return item;
    }

}