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
    private MagicBrosMario game;
    private Sprint2Controller gameData;
    public MarioGameController(MagicBrosMario g, ref Sprint2Controller data)
    {
        game =g;
        gameData = data;
        Initialize();
    }

    public void Initialize()
    {
        inputMap = new KeysNMouseCommandMapper();
        SetSprint2Binds();
    }
    public struct Sprint2Controller {
        public Player player;
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
        inputMap.Bind(Keys.W, gt => gameData.player.Jump(gt));
        inputMap.Bind(Keys.Up, gt => gameData.player.Jump(gt));
        inputMap.Bind(Keys.A, gt => gameData.player.Left(gt));
        inputMap.Bind(Keys.Left, gt => gameData.player.Left(gt));
        inputMap.Bind(Keys.S, gt => gameData.player.Crouch(gt));
        inputMap.Bind(Keys.Down, gt => gameData.player.Crouch(gt));
        inputMap.Bind(Keys.D, gt => gameData.player.Right(gt));
        inputMap.Bind(Keys.Right, gt => gameData.player.Right(gt));
        inputMap.Bind(Keys.Z, gt => gameData.player.Attack());
        inputMap.Bind(Keys.N, gt =>  gameData.player.Attack());
        inputMap.Bind(Keys.E, gt =>  gameData.player.TakeDamage());
        inputMap.Bind(Keys.T, gt => game.incrementBlock()); // needs to be fixed so that it only runs once per key press
        inputMap.Bind(Keys.Y, gt => game.decrementBlock());// needs to be fixed so that it only runs once per key press
        inputMap.Bind(Keys.U, gt => game.incrementItem()); // needs to be fixed so that it only runs once per key press
        inputMap.Bind(Keys.I, gt => game.decrementItem()); // needs to be fixed so that it only runs once per key press
        inputMap.Bind(Keys.O, gt => game.incrementEnemy());// needs to be fixed so that it only runs once per key press
        inputMap.Bind(Keys.P, gt => game.decrementEnemy());// needs to be fixed so that it only runs once per key press
        inputMap.Bind(Keys.Q, gt => game.Exit()); 
        inputMap.Bind(Keys.R, gt => game.resetGame());

        // mouse inputs
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Right), () => game.Exit());
        inputMap.Bind(m => m.WasButtonJustPressed(MouseButton.Left) && m.Position.X < gameData.halfX && m.Position.Y < gameData.halfY, () => game.displayPowerUp(0));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X > gameData.halfX && m.Position.Y < gameData.halfY, () => game.displayPowerUp(1));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X < gameData.halfX && m.Position.Y > gameData.halfY, () => game.displayPowerUp(2));
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X > gameData.halfX && m.Position.Y > gameData.halfY, () => game.displayPowerUp(3));
    }
    public void Update(GameTime gameTime)
    {
        gameData.keyb.Update();
        gameData.mouse.Update();
        
        if(gameData.keyb.IsKeyUp(Keys.S) || gameData.keyb.IsKeyUp(Keys.Down)) // needs to go into input map
        {
            gameData.player.ReleaseCrouch();
        }
        if(gameData.keyb.CurrentState.GetPressedKeyCount() == 0 || (gameData.keyb.IsKeyDown(Keys.Z) || gameData.keyb.IsKeyDown(Keys.N))) // needs to be fixed so that it runs only after movement is not pressed
        {
            gameData.player.Idle();
        }
        inputMap.ProcessInput(gameTime, gameData.keyb, gameData.mouse);// check all the inputs of the mouse and keyboard and run their corresponding function
    
    }

}