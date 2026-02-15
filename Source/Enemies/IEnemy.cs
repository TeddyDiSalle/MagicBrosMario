using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source;

public interface IEnemy
{
    // What does an enemy need to do?
    // 1. Move
    // 2. Interact with the player (damage, etc.)
    // 3. Interact with the environment (platforms, etc.)
    // 4. Be destroyed (when the player jumps on it, etc.)
    // 5. State transitions
    public void Update(GameTime gametime);
    public void Draw(SpriteBatch spriteBatch);
    public void Kill();
    
    public Point Position { get; }
}
