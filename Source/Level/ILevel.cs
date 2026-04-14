using MagicBrosMario.Source.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace MagicBrosMario.Source;
public interface ILevel
{
    string ToString(){ return Name; }
    string Name { get; }
    int TimeLimit {get;}
    int MarioStartPosX { get; }
    int MarioStartPosY { get; }
    void Update(GameTime gt);
    void Draw(SpriteBatch sb);
    void AddItem(IItems item);
    public void Initialize(Microsoft.Xna.Framework.Content.ContentManager contentManager, Texture2D bTexture, Texture2D eTexture, Texture2D iTexture);
    public void Clear();
}