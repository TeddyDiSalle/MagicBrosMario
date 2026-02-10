using System;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Sprite;

/// <summary>
/// create a shared texture
/// <code lang="csharp">
/// <![CDATA[
/// var sharedTexture = new SharedTexture();
/// 
/// // create a new sprite from the shared texture
/// var sprite = sharedTexture.NewSprite(100, 100, 50, 50);
/// 
/// // create a new animated sprite from the shared texture
/// // with 10 frames and 2 frames per second
/// var animatedSprite = sharedTexture.NewAnimatedSprite(
///     100, 100, 50, 50, 10, 0.5
/// );
/// 
/// /* in LoadContent */
/// var texture = Content.Load<Texture2D>("texture_image");
/// 
/// // this binds the texture to both sprite, animated sprite
/// // and any sprite that is create with the same SharedTexture
/// sharedTexture.BindTexture(texture);
///
/// /* in Update */
/// // update is required for animated texture
/// animatedTexture.Update(gameTime);
///
/// // update is not required for non-animated texture
/// sprite.Update(gameTime);
/// 
/// /* in Draw */
/// sprite.Draw(spriteBatch);
/// animatedSprite.Draw(spriteBatch);
/// ]]></code>
/// </summary>
public class SharedTexture
{
    /// <summary>
    /// texture of this shared texture
    /// </summary>
    public Texture2D Texture { get; private set; }

    /// <summary>
    /// bind a texture to this shared texture<br/>
    ///
    /// every sprite created with this shared texture will be linked to <paramref name="texture"/>
    /// </summary>
    /// <param name="texture">texture to bind</param>
    /// <exception cref="Exception">thrown when texture is bound twice</exception>
    public void BindTexture(Texture2D texture)
    {
        if (Texture != null)
        {
            throw new Exception("overriding shared texture is not allowed");
        }

        Texture = texture;
    }

    /// <summary>
    /// create a new non-animated sprite from this shared texture
    /// </summary>
    /// <param name="offsetX">x offset on texture</param>
    /// <param name="offsetY">y offset on texture</param>
    /// <param name="width">width of sprite</param>
    /// <param name="height">height of sprite</param>
    /// <returns>a new non-animated sprite using this shared texture</returns>
    public Sprite NewSprite(int offsetX, int offsetY, int width, int height)
    {
        return new Sprite(this, offsetX, offsetY, width, height);
    }

    /// <summary>
    /// create a new animated sprite from this shared texture
    /// </summary>
    /// <param name="offsetX">x offset on texture</param>
    /// <param name="offsetY">y offset on texture</param>
    /// <param name="width">width of a single frame</param>
    /// <param name="height">height of a single frame</param>
    /// <param name="frameCount">number of frame</param>
    /// <param name="secPerFrame">time each frame is on the screen</param>
    /// <returns>a new animated sprite using this shared texture</returns>
    public AnimatedSprite NewAnimatedSprite(
        int offsetX, int offsetY,
        int width, int height,
        int frameCount, float secPerFrame)
    {
        return new AnimatedSprite(this, offsetX, offsetY, width, height, frameCount, secPerFrame);
    }
}