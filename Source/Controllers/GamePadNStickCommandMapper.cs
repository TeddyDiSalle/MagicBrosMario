// Made by Teddy DiSalle
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace MagicBrosMario.Source;
public class GamePadNStickCommandMapper
{
    private sealed class RepeatBinding
    {
        public Action<GameTime> Action = default!;
        public double InitialDelay;
        public double RepeatInterval;

        // state:
        public bool WasDown;
        public double TimeUntilNext;
    }
    private readonly Dictionary<Buttons, RepeatBinding> _button = new();

    
    //public void Bind(Keys key, Action<GameTime> command){// keyboard binding
    //    _held[key] = command;
    //}

    public void Bind(Buttons key, Action<GameTime> action,
        double initialDelaySeconds = 0.01, double repeatIntervalSeconds = 0.01)
    {
        _button[key] = new RepeatBinding{
            Action = action,
            InitialDelay = initialDelaySeconds,
            RepeatInterval = repeatIntervalSeconds
        };
    }

    public void ProcessInput(GameTime time, GamePadInfo gamepad){
        
        double dt = time.ElapsedGameTime.TotalSeconds;
        
        // check keyboard
        foreach (var (key, b) in _button){
            bool isDown = gamepad.IsButtonDown(key);

            if (!isDown)
            {
                b.WasDown = false;
                b.TimeUntilNext = 0;
                continue;
            }

            // just pressed
            if (!b.WasDown)
            {
                b.WasDown = true;
                b.TimeUntilNext = b.InitialDelay;
                b.Action(time);           // immediate fire
                continue;
            }

            // held: countdown and repeat
            b.TimeUntilNext -= dt;
            if (b.TimeUntilNext <= 0)
            {
                while (b.TimeUntilNext <= 0)
                    b.TimeUntilNext += b.RepeatInterval;

                b.Action(time);
            }
        }

    }
}
