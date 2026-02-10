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
/// new StaticBlock(
///     
/// );
/// 
/// /* in LoadContent */
/// // bind texture to shared texture
/// var texture = Content.Load<Texture2d>("texture");
/// sharedTexture.BindTexture(texture);
/// ]]>
/// </code>
/// </example>
/// </summary>
/// <param name="sprite">sprite object from shared texture</param>
public class StaticBlock(Sprite.Sprite sprite) : IBlock
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
        // static block don't need to be updated
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsVisible) return;

        // we could add check for out of screen if needed 

        sprite.Draw(spriteBatch);
    }
}