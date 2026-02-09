using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

public class StaticBlock(Rectangle textureRect, StaticBlock.StaticBlockInitializer initializer) : IBlock
{
    /// <summary>
    /// lambda function that get called when load content is called.
    /// this function allow for referencing global texture before it is created
    ///
    /// set initial position, scale, texture and other with this function 
    /// </summary>
    public delegate Texture2D StaticBlockInitializer(StaticBlock block);

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