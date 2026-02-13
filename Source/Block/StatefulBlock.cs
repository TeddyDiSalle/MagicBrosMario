using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TSm">type of state machine</typeparam>
/// <typeparam name="TS">type of block state</typeparam>
/// <typeparam name="TE">type of event that block respond to</typeparam>
public class StatefulBlock<TSm, TS, TE> : BlockBase<StatefulBlock<TSm, TS, TE>>
    where TSm : IBlockStateMachine<TS, TE>, new()
    where TS : Enum
    where TE : Enum
{
    private readonly TSm stateMachine;
    private TS previousState;
    private Dictionary<TS, Sprite.ISprite> stateSpriteMap;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sprite">sprite object from shared texture, both sprite and animated sprite works</param>
    /// <param name="stateSpriteMap">mapping between state and sprite</param>
    /// <typeparam name="TSm">type of state machine</typeparam>
    /// <typeparam name="TS">type of block state</typeparam>
    /// <typeparam name="TE">type of event that block respond to</typeparam>
    public StatefulBlock(Sprite.ISprite sprite, Dictionary<TS, Sprite.ISprite> stateSpriteMap) : base(sprite)
    {
        stateMachine = new TSm();
        previousState = stateMachine.State;
        this.stateSpriteMap = stateSpriteMap;
    }

    // there will be more methods here for creating events and signaling the state machine

    public override void Update(GameTime gameTime)
    {
        if (previousState != stateMachine.State)
        {
            previousState = stateMachine.State;
            Sprite = stateSpriteMap[stateMachine.State];
        }

        Sprite.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Sprite.Draw(spriteBatch);
    }
}