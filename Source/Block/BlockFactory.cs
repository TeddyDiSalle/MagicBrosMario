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
    
}