using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;

public interface IPlayerState
{
    void Left(GameTime gameTime);
    void Right(GameTime gameTime);
    void Jump(GameTime gameTime);
    void Crouch(GameTime gameTime);
    void Attack();
    void TakeDamage();
    void PowerUp(Power power);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);

}
