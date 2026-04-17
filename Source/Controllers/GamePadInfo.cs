using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MagicBrosMario.Source;

public class GamePadInfo : IController
{
    
    public GamePadState PreviousState {  get; private set; }

    public GamePadState CurrentState { get; private set; }

    public GamePadInfo()
    {
        PreviousState = new GamePadState();
        CurrentState = GamePad.GetState(PlayerIndex.One);
    }

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = GamePad.GetState(PlayerIndex.One);
    }

    public bool IsButtonDown(Buttons button)
    {
        return CurrentState.IsButtonDown(button);
    }

    public bool IsButtonUp(Buttons button)
    {
        return CurrentState.IsButtonUp(button);
    }

    public bool WasButtonJustPressed(Buttons button)
    {
        return CurrentState.IsButtonDown(button) && PreviousState.IsButtonUp(button);
    }

    public bool WasButtonJustReleased(Buttons button)
    {
        return CurrentState.IsButtonUp(button) && PreviousState.IsButtonDown(button);
    }
}
