using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
namespace MagicBrosMario.Source;
public class KeyCommandMapper
{
    private Dictionary<Keys, Action> bindings =
        new Dictionary<Keys, Action>();

    public void Bind(Keys key, Action command)
    {
        bindings[key] = command;
    }

    public void ProcessInput(KeyboardInfo keyboard)
    {
        foreach (var binding in bindings)
        {
            if (keyboard.IsKeyDown(binding.Key))
            {
                binding.Value.Invoke();
            }
        }
    }
}
