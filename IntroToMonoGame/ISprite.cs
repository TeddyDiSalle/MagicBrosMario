using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IntroToMonoGame;

public interface ISprite
{
    
    public void Update(GameTime gametime);
    public void Draw(SpriteBatch _spriteBatch);
}
