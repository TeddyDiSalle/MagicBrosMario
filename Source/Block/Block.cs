using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// block implementation for non-animated and non-updatable blocks
///
/// <example>
/// creating a simple block
/// <code lang="csharp">
/// <![CDATA[
/// var sharedTexture = null;
/// 
/// new Block(
///     // this creates a new sprite for the block
///     // this can also be repalce with animated sprite
///     sharedTexture.NewSprite(100, 100, 50, 50);
/// );
/// 
/// /* in LoadContent */
/// // bind texture to shared texture
/// var texture = Content.Load<Texture2d>("texture");
/// sharedTexture.BindTexture(texture);
///
/// /* in Update */
/// // this updates the animation(if there is one)
/// block.Update(gameTime);
///
/// /* in Draw */
/// block.Draw(spriteBatch);
/// ]]>
/// </code>
/// </example>
/// </summary>
/// <param name="sprite">sprite object from shared texture, both sprite and animated sprite works</param>
public class Block(Sprite.ISprite sprite) : IBlock
{
    public bool IsSolid { get; set; } = false;
    public bool IsVisible { get; set; } = true;

    public Point Position
    {
        get => sprite.Position;
        set => sprite.Position = value;
    }

    public Point Size => sprite.Size;

    public float Scale
    {
        get => sprite.Scale;
        set => sprite.Scale = value;
    }

    public void Update(GameTime gameTime)
    {
        if (sprite.IsAnimated)
        {
            sprite.Update(gameTime);
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;

        // we could add check for out of screen if needed 

        sprite.Draw(spriteBatch);
    }
}