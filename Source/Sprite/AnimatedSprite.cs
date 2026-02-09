using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Sprite;

/// <summary>
/// animated sprite class that uses a <see cref="SharedTexture"/><br/>
///
/// this class should not be initiated outside SharedTexture
/// </summary>
/// <param name="texture">shared texture</param>
/// <param name="offsetX">x offset on texture</param>
/// <param name="offsetY">y offset on texture</param>
/// <param name="frameWidth">width of a single frame</param>
/// <param name="frameHeight">height of a single frame</param>
/// <param name="frameCount">number of frame</param>
/// <param name="secPerFrame">time each frame is on the screen</param>
public class AnimatedSprite(
    SharedTexture texture,
    int offsetX,
    int offsetY,
    int frameWidth,
    int frameHeight,
    int frameCount,
    float secPerFrame
) : ISprite
{
    public bool IsAnimated => true;

    public int X { get; set; }

    public int Y { get; set; }

    public Rectangle DestRect { get; private set; }

    public float Scale
    {
        get;
        set
        {
            // update field and destination rectangle
            field = value;
            DestRect = new Rectangle(X, Y, (int)(value * frameWidth), (int)(value * frameHeight));
        }
    } = 1;

    private int frame = 0;
    private float timer = 0;

    public void Update(GameTime gameTime)
    {
        timer += gameTime.ElapsedGameTime.Milliseconds;
        var msPerFrame = secPerFrame * 1000f;

        if (!(timer > msPerFrame)) return;

        // update current frame
        var framePassed = (int)(timer / msPerFrame);
        frame = (frame + framePassed) % frameCount;
        timer -= framePassed * msPerFrame;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var sourceRect = new Rectangle(offsetX + frameWidth * frame, offsetY, frameWidth, frameHeight);
        spriteBatch.Draw(texture.Texture, DestRect, sourceRect, Color.White);
    }
}