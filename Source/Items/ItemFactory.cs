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
        var item = new Coin(ITEM_SHARED_TEXTURE, x, y);
        return item;
    }

    public static IItems CreateFireFlower(int x, int y) {
        var item = new Fireflower(ITEM_SHARED_TEXTURE, 100, 100, x, y); // TODO get rid of bounds in parameters
        return item;
    }

    public static IItems CreateStar(int x, int y) {
        var item = new Star(ITEM_SHARED_TEXTURE, 100, 100, x, y); // TODO get rid of bounds in parameters
        return item;
    }
    public static IItems CreateOneUp(int x, int y) {
        var item = new OneUp(ITEM_SHARED_TEXTURE, 100, 100, x, y); // TODO get rid of bounds in parameters
        return item;
    }

}