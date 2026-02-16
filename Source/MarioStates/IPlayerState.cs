using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public interface IPlayerState
{
    void Left(GameTime gameTime);
    void Right(GameTime gameTime);
    void Jump(GameTime gameTime);
    void Crouch(GameTime gameTime);
    void Attack();
    void TakeDamage();
    void PowerUp(Power power);
    void Idle();
    void Update(GameTime gameTime, Vector2 Velocity, bool Flipped);
    void Draw(SpriteBatch spriteBatch, Vector2 Position);

}
