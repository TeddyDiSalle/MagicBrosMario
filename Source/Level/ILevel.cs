using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MagicBrosMario.Source;
public interface ILevel
{
    void Update(GameTime gt);
    void Draw(SpriteBatch sb);
    public void Initialize(Texture2D bTexture, Texture2D eTexture, Texture2D iTexture);
    public List<IEnemy> GetEnemies();

}