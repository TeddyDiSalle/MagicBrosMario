using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace MagicBrosMario.Source;

public class MarioGameController : IController{
    // Classes should not worry about game actions not if "w" is pressed or mouse is over here or over there.
    // This allows the game to work whether the player is using a keyboard or a potato
    // This class will call the other's classes move functions and damage functions and such
    private KeysNMouseCommandMapper inputMap;

    private ISprite[] sprites;
    private IController[] keyboardNMouse;
    private int[] cords;
    private MagicBrosMario game;
    public MarioGameController(MagicBrosMario g, ISprite[] Sprites, IController[] Controllers, int[] halfCords )
    {
        game =g;
        sprites = Sprites;
        keyboardNMouse = Controllers;
        cords = halfCords;
        Initialize();
    }

    public void Initialize()
    {
        
        inputMap = new KeysNMouseCommandMapper();
        SetSprint2Binds();
    }

    // All the binds specified for sprint 2
    public void SetSprint2Binds()
    {
        //Keyboard inputs
        inputMap.Bind(Keys.D0,() => game.Exit());
        inputMap.Bind(Keys.D1, () => game.SetCurrentSprite(sprites[0]));
        inputMap.Bind(Keys.D2, () => game.SetCurrentSprite(sprites[1]));
        inputMap.Bind(Keys.D3, () => game.SetCurrentSprite(sprites[2]));
        inputMap.Bind(Keys.D4, () => game.SetCurrentSprite(sprites[3]));
        inputMap.Bind(Keys.W, () => game.Exit()); //TO DO player.jump()
        inputMap.Bind(Keys.Up, () => game.Exit()); //TO DO player.jump()
        inputMap.Bind(Keys.A, () => game.Exit()); //TO DO player.left()
        inputMap.Bind(Keys.Left, () => game.Exit()); //TO DO player.left()
        inputMap.Bind(Keys.S, () => game.Exit()); //TO DO player.crouch()
        inputMap.Bind(Keys.Down, () => game.Exit()); //TO DO player.crouch()
        inputMap.Bind(Keys.D, () => game.Exit()); //TO DO player.right()
        inputMap.Bind(Keys.Right, () => game.Exit()); //TO DO player.right()
        inputMap.Bind(Keys.Z, () => game.Exit()); //TO DO player.attack()
        inputMap.Bind(Keys.N, () => game.Exit()); //TO DO player.attack()
        inputMap.Bind(Keys.E, () => game.Exit()); //TO DO player.TakeDamage()
        inputMap.Bind(Keys.T, () => game.Exit()); //TO DO cycle obstacle left
        inputMap.Bind(Keys.Y, () => game.Exit()); //TO DO cycle obstacle right
        inputMap.Bind(Keys.U, () => game.Exit()); //TO DO cycle item left/previous
        inputMap.Bind(Keys.I, () => game.Exit()); //TO DO cycle item right/next
        inputMap.Bind(Keys.O, () => game.Exit()); //TO DO cycle enemy left/previous
        inputMap.Bind(Keys.P, () => game.Exit()); //TO DO cycle enemy right/next
        inputMap.Bind(Keys.Q, () => game.Exit()); 
        inputMap.Bind(Keys.R, () => game.Exit()); //TO DO reset game

        // mouse inputs
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Right), () => game.Exit());
        inputMap.Bind(m => m.WasButtonJustPressed(MouseButton.Left) && m.Position.X < cords[0] && m.Position.Y < cords[1], () => game.SetCurrentSprite(sprites[0]));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X > cords[0] && m.Position.Y < cords[1], () => game.SetCurrentSprite(sprites[1]));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X < cords[0] && m.Position.Y > cords[1], () => game.SetCurrentSprite(sprites[2]));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X > cords[0] && m.Position.Y > cords[1], () => game.SetCurrentSprite(sprites[3]));
    }
    public void Update()
    {
        KeyboardInfo keyboardInput = (KeyboardInfo)keyboardNMouse[0];
        MouseInfo mouseInput = (MouseInfo)keyboardNMouse[1];
        keyboardInput.Update();
        mouseInput.Update();
        
        inputMap.ProcessInput(keyboardInput, mouseInput);// check all the inputs of the mouse and keyboard and run their corresponding function
    }
    
}