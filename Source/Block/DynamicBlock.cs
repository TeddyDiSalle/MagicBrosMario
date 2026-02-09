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
///     new Rectangle(100, 100, 50, 50),    // textureRect
///     (block) => {                        // initializer
///         block.Scale = 2.0;              // set scale to 2
///         return sharedTexture;           // return the texture reference
///     }
/// );
/// 
/// // initialize the texture after the block has been created
/// var sharedTexture = Content.Load<Texture2d>("shared_texture);
/// 
/// // correct texture is loaded
/// block.LoadContent();
/// ]]>
/// </code>
/// </example>
/// </summary>
/// <param name="textureRect">the x, y, width and height of the block on the texture</param>
/// <param name="initializer"><see cref="DynamicBlockInitializer"/></param>
public class DynamicBlock(Rectangle textureRect, DynamicBlock.DynamicBlockInitializer initializer) : IBlock
{
    /// <summary>
    /// lambda function that get called when load content is called<br/>
    /// 
    /// this function allow for referencing global texture before it is created<br/>
    /// 
    /// set initial position, scale, texture and other with this function 
    /// </summary>
    public delegate Texture2D DynamicBlockInitializer(DynamicBlock block);

    public bool IsSolid { get; set; } = false;
    public bool IsVisible { get; set; } = true;

    public Vector2 Position
    {
        get;
        set // update destination rectangle when position change
        {
            field = value;
            destRect = new Rectangle(
                (int)value.X, (int)value.Y,
                (int)(Scale * texture.Width),
                (int)(Scale * texture.Height));
        }
    }

    public Vector2 Size => new(textureRect.Width, textureRect.Height);

    public float Scale { get; set; } = 1;

    // private fields
    private Texture2D texture;
    private Rectangle destRect;

    public void LoadContent()
    {
        Position = Vector2.Zero; // call the setter at least once so that destRect is initialized
        texture = initializer(this);
    }

    public void Draw(SpriteBatch spriteBatch, GameTime _)
    {
        if (!IsVisible) return;

        // we could add check for out of screen if needed 
        
        spriteBatch.Draw(texture, destRect, textureRect, Color.White);
    }
}