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
/// var block1 = new Block(
///     // this creates a new sprite for the block
///     sharedTexture.NewSprite(100, 100, 50, 50);
/// );
///
/// var block2 = new Block(
///     // this creates a new animated sprite for the block
///     sharedTexture.NewAnimatedSprite(150, 150, 50, 50, 10, 0.5);
/// ).WithScale(2.0)             // this gives the block an initial scale
/// .WithPosition(100, 100);     // this gives the block an initial position
/// 
/// /* in LoadContent */
/// // bind texture to shared texture
/// var texture = Content.Load<Texture2d>("texture");
/// sharedTexture.BindTexture(texture);
/// 
/// // both block1 and block2 has valid texture after this point
///
/// /* in Update */
/// // this updates the animation
/// block1.Update(gameTime);
/// block2.Update(gameTime);
///
/// /* in Draw */
/// block1.Draw(spriteBatch);
/// block2.Draw(spriteBatch);
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

    /// <summary>
    /// this utility method allow giving block a initial position by chaining with constructor
    /// </summary>
    /// <param name="x">x position</param>
    /// <param name="y">y position</param>
    /// <returns></returns>
    public Block WithPosition(int x, int y)
    {
        Position = new Point(x, y);
        return this;
    }

    public Point Size => sprite.Size;

    public float Scale
    {
        get => sprite.Scale;
        set => sprite.Scale = value;
    }

    /// <summary>
    /// this utility method allow giving block a initial scale by chaining with constructor
    /// </summary>
    /// <param name="scale">scale</param>
    /// <returns></returns>
    public Block WithScale(float scale)
    {
        Scale = scale;
        return this;
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