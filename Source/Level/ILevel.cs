using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MagicBrosMario.Source;
public interface ILevel
{
    String Name { get; }
    int MarioStartPosX { get; }
    int MarioStartPosY { get; }
    void Update(GameTime gt);
    void Draw(SpriteBatch sb);
    public void Initialize(Microsoft.Xna.Framework.Content.ContentManager contentManager, Texture2D bTexture, Texture2D eTexture, Texture2D iTexture);
    public void Clear();
}