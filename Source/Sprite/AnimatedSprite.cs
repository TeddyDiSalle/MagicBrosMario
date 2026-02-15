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

    public Point Position
    {
        get;
        set
        {
            field = value;
            UpdateSize();
        }
    }

    public float Scale
    {
        get;
        set
        {
            field = value;
            UpdateSize();
        }
    } = 1;

    public Point Size { get; private set; }
    
    public Color Color { get; set; } = Color.White;

    private Rectangle destRect = new(0, 0, frameWidth, frameHeight);

    private int frame = 0;
    private float timer = 0;

    private void UpdateSize()
    {
        Size = new Point((int)(Scale * frameWidth), (int)(Scale * frameHeight));
        destRect = new Rectangle(Position.X, Position.Y, Size.X, Size.Y);
    }

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
        spriteBatch.Draw(texture.Texture, destRect, sourceRect, Color);
    }
}