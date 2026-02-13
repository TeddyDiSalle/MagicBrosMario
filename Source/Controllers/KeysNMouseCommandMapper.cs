// Made by Teddy DiSalle
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace MagicBrosMario.Source;
public class KeysNMouseCommandMapper
{
    private Dictionary<Keys, Action> bindings =
        new Dictionary<Keys, Action>();
    private Dictionary<Func<MouseInfo,bool>, Action> clicks =
        new Dictionary<Func<MouseInfo,bool>, Action>();
    
    public void Bind(Keys key, Action command){// keyboard binding
        bindings[key] = command;
    }

    public void Bind(Func<MouseInfo, bool> condition, Action command){//mouse binding
        clicks[condition] = command;
    }

    public void ProcessInput(KeyboardInfo keyboard, MouseInfo mouse){
        foreach (var binding in bindings){ // check keyboard
            if (keyboard.IsKeyDown(binding.Key)){
                binding.Value.Invoke();
            }
        }

        foreach (var click in clicks){ // check mouse
            if (click.Key.Invoke(mouse)){
                click.Value.Invoke();
            }
        }
    }
}
