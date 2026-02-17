// Made by Teddy DiSalle
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using MagicBrosMario.Source.MarioStates;


namespace MagicBrosMario.Source;

public class MarioGameController{
    // Classes should not worry about game actions not if "w" is pressed or mouse is over here or over there.
    // This allows the game to work whether the player is using a keyboard or a potato
    // This class will call the other's classes move functions and damage functions and such
    private KeysNMouseCommandMapper inputMap;

    private KeyboardInfo keyboard;
    private MouseInfo mouse;

    private int x;
    private int y;
    private Player player;
    private MagicBrosMario game;
    public MarioGameController(MagicBrosMario g, ref Sprint2Controller data)
    {
        player = data.player;
        game =g;
        keyboard = data.keyb;
        mouse = data.mouse;
        x = data.halfX;
        y = data.halfY;
        Initialize();
    }

    public void Initialize()
    {
        inputMap = new KeysNMouseCommandMapper();
        SetSprint2Binds();
    }
    public struct Sprint2Controller {
        public MarioStates.Player player;
        public MouseInfo mouse;
        public  KeyboardInfo keyb;
        public int halfX;
        public int halfY;
    }
    // All the binds specified for sprint 2
    public void SetSprint2Binds()
    {
        //Keyboard inputs
        inputMap.Bind(Keys.D0,gt => game.Exit());
        inputMap.Bind(Keys.D1, gt => game.displayPowerUp(0));
        inputMap.Bind(Keys.D2, gt => game.displayPowerUp(1));
        inputMap.Bind(Keys.D3, gt => game.displayPowerUp(2));
        inputMap.Bind(Keys.D4, gt => game.displayPowerUp(3));
        inputMap.Bind(Keys.W, gt => player.Jump(gt));
        inputMap.Bind(Keys.Up, gt => player.Jump(gt));
        inputMap.Bind(Keys.A, gt => player.Jump(gt));
        inputMap.Bind(Keys.Left, gt => player.Jump(gt));
        inputMap.Bind(Keys.S, gt => player.Crouch(gt));
        inputMap.Bind(Keys.Down, gt => player.Crouch(gt));
        inputMap.Bind(Keys.D, gt => player.Right(gt));
        inputMap.Bind(Keys.Right, gt => player.Right(gt));
        inputMap.Bind(Keys.Z, gt => player.Attack());
        inputMap.Bind(Keys.N, gt =>  player.Attack());
        inputMap.Bind(Keys.E, gt =>  player.TakeDamage());
        inputMap.Bind(Keys.T, gt => game.incrementBlock());
        inputMap.Bind(Keys.Y, gt => game.decrementBlock());
        inputMap.Bind(Keys.U, gt => game.incrementItem()); 
        inputMap.Bind(Keys.I, gt => game.decrementItem()); 
        inputMap.Bind(Keys.O, gt => game.incrementEnemy());
        inputMap.Bind(Keys.P, gt => game.decrementEnemy());
        inputMap.Bind(Keys.Q, gt => game.Exit()); 
        inputMap.Bind(Keys.R, gt => game.resetGame());

        // mouse inputs
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Right), () => game.Exit());
        inputMap.Bind(m => m.WasButtonJustPressed(MouseButton.Left) && m.Position.X < x && m.Position.Y < y, () => game.displayPowerUp(0));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X > x && m.Position.Y < y, () => game.displayPowerUp(1));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X < x && m.Position.Y > y, () => game.displayPowerUp(2));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X > x && m.Position.Y > y, () => game.displayPowerUp(3));
    }
    public void Update(GameTime gameTime)
    {
        keyboard.Update();
        mouse.Update();
        
        inputMap.ProcessInput(gameTime, keyboard, mouse);// check all the inputs of the mouse and keyboard and run their corresponding function
    }

}