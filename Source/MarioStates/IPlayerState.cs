using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;

public interface IPlayerState
{
    void Left();
    void Right();
    void Jump();
    void Crouch();
    void Attack();
    void TakeDamage();
    void PowerUp(Power power);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);

}
