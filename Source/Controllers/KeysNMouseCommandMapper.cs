// Made by Teddy DiSalle
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace MagicBrosMario.Source;
public class KeysNMouseCommandMapper
{
    //private Dictionary<Keys, Action<GameTime>> _held = new();
     private sealed class RepeatBinding
    {
        public Action<GameTime> Action = default!;
        public double InitialDelay;
        public double RepeatInterval;

        // state:
        public bool WasDown;
        public double TimeUntilNext;
    }

    private readonly Dictionary<Keys, RepeatBinding> _held = new();

    private Dictionary<Func<MouseInfo,bool>, Action> _clicks = new();
    
    //public void Bind(Keys key, Action<GameTime> command){// keyboard binding
    //    _held[key] = command;
    //}

    public void Bind(Keys key, Action<GameTime> action,
        double initialDelaySeconds = 0.1, double repeatIntervalSeconds = 0.01)
    {
        _held[key] = new RepeatBinding{
            Action = action,
            InitialDelay = initialDelaySeconds,
            RepeatInterval = repeatIntervalSeconds
        };
    }

    public void Bind(Func<MouseInfo, bool> condition, Action command){//mouse binding
        _clicks[condition] = command;
    }

    public void ProcessInput(GameTime time, KeyboardInfo keyboard, MouseInfo mouse){
        
        double dt = time.ElapsedGameTime.TotalSeconds;
        
        // check keyboard
        foreach (var (key, b) in _held){
            bool isDown = keyboard.IsKeyDown(key);

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

        // check mouse
        foreach (var click in _clicks){
            if (click.Key.Invoke(mouse)){
                click.Value.Invoke();
            }
        }
    }
}
