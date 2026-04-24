//Made by Teddy
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MagicBrosMario.Source;
public static class KeysAndButtonsConverter{
    public static Keys? ToKey(Buttons b){
        switch(b){
            case Buttons.A:
                return Keys.Space;
            case Buttons.B:
                return Keys.B;
            case Buttons.X:
                return Keys.X;
            case Buttons.Y:
                return Keys.Y;
            case Buttons.RightShoulder:
                return Keys.Z;
            case Buttons.Back:
                return Keys.Q;
            case Buttons.Start:
                return Keys.Escape;
            case Buttons.DPadDown:
                return Keys.Down;
            case Buttons.LeftThumbstickDown:
                return Keys.Down;
            case Buttons.DPadUp:
                return Keys.Up;
            case Buttons.LeftThumbstickUp:
                return Keys.Up;
            case Buttons.DPadLeft:
                return Keys.Left;
            case Buttons.LeftThumbstickLeft:
                return Keys.Left;
            case Buttons.DPadRight:
                return Keys.Right;
            case Buttons.LeftThumbstickRight:
                return Keys.Right;
            default:
                System.Console.WriteLine("No key mapping for button " + b);
                return null;
        }
    }

    public static Buttons? ToButton(Keys k){
        switch(k){
            case Keys.Space:
                return Buttons.A;
            case Keys.B:
                return Buttons.B;
            case Keys.D0:
                return Buttons.X;
            case Keys.D1:
                return Buttons.Y;
            case Keys.Z:
                return Buttons.RightShoulder;
            case Keys.N:
                return Buttons.RightShoulder;
            case Keys.Q:
                return Buttons.None;
            case Keys.Escape:
                return Buttons.None;
            case Keys.Down:
                return Buttons.DPadDown;
            case Keys.Up:
                return Buttons.DPadUp;
            case Keys.Left:
                return Buttons.DPadLeft;
            case Keys.Right:
                return Buttons.DPadRight;
            case Keys.S:
                return Buttons.LeftThumbstickDown;
            case Keys.W:
                return Buttons.LeftThumbstickUp;
            case Keys.A:
                return Buttons.LeftThumbstickLeft;
            case Keys.D:
                return Buttons.LeftThumbstickRight;
            case Keys.P:
                return Buttons.Back;
            case Keys.R:
                return Buttons.Start;
            default:
                System.Console.WriteLine("No key mapping for button " + k);
                return null;
        }
    }

}