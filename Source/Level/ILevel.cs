using Microsoft.Xna.Framework;

namespace MagicBrosMario.Source;
public interface ILevel
{
    void Update(GameTime gt);
    void Draw(GameTime gt);

}