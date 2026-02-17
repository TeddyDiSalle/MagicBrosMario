using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

public class BlockFactory
{
    public static readonly SharedTexture BLOCK_SHARED_TEXTURE = new SharedTexture();
    public const string TEXTURE_NAME = "blocks";

    public static void BindTexture(Texture2D texture)
    {
        BLOCK_SHARED_TEXTURE.BindTexture(texture);
    }

    public static Block VoidBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(16, 16, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block SkyBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(32, 16, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block GroundBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(16, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block BlueGroundBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(32, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block Bricks()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(48, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block BlueBricks()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(64, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block BaseBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(80, 32, 16, 16);
        var block = new Block(sprite);
        return block;
    }

    public static Block QuestionMarkBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewAnimatedSprite(16, 48, 16, 16, 3, 0.2f);
        var block = new Block(sprite);
        return block;
    }

    public static Block EmptyQuestionMarkBlock()
    {
        var sprite = BLOCK_SHARED_TEXTURE.NewSprite(64, 48, 16, 16);
        var block = new Block(sprite);
        return block;
    }
}