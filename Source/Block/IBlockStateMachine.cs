using System;

namespace MagicBrosMario.Source.Block;

/// <summary>
/// interface for the state machine
/// </summary>
/// <typeparam name="TState">type of state that the state machine holds</typeparam>
/// <typeparam name="TEvent">type of event that the state machine responds to</typeparam>
public interface IBlockStateMachine<out TState, in TEvent>
    where TState : Enum
    where TEvent : Enum
{
    /// <summary>
    /// the current state of the state machine
    /// </summary>
    public TState State { get; }

    /// <summary>
    /// update function for the state machine when a block event occurs
    /// </summary>
    /// <param name="event">event</param>
    public void NextState(TEvent @event);
}