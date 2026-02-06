using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source;

public interface ISprite
{
    
    public void Update(GameTime gametime);
    public void Draw(SpriteBatch _spriteBatch);
}
