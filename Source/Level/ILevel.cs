using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source;
public interface ILevel
{
    void Update(GameTime gt);
    void Draw(SpriteBatch sb);

}