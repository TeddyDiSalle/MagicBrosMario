// Made by Teddy DiSalle
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.GameStates;
using System.Diagnostics;
using MagicBrosMario.Source.Level;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;


namespace MagicBrosMario.Source;

public static class MarioGameController{
    // Classes should not worry about game actions not if "w" is pressed or mouse is over here or over there.
    // This allows the game to work whether the player is using a keyboard or a potato
    // This class will call the other's classes move functions and damage functions and such
    private static KeysNMouseCommandMapper keysNMouseInputMap;
    private static GamePadNStickCommandMapper gamePadNStickInputMap;
    private static Sprint5Controller gameData;
    private static bool muted = false;

    public static void Initialize( ref Sprint5Controller data)
    {
        
        gameData = data;
        if(data.keyb != null){
            keysNMouseInputMap = new KeysNMouseCommandMapper();
        }
        SetSprint5Binds();
    }
    public struct Sprint5Controller {
        public MouseInfo mouse;
        public  KeyboardInfo keyb;
        public GamePadInfo gamepad;
        public int halfX;
        public int halfY;
    }
    // All the binds specified for sprint 3
    private static void SetSprint5Binds()
    {
        double pauseIDelay= 0.5;
        double pauseRInterval = 1.0;
        //Need to make it so it can be unpaused through here
		keysNMouseInputMap.Bind(Keys.P, gt => MagicBrosMario.INSTANCE.changePaused(), pauseIDelay, pauseRInterval);
		keysNMouseInputMap.Bind(Keys.Escape, gt => MagicBrosMario.INSTANCE.changePaused(), pauseIDelay, pauseRInterval);

        keysNMouseInputMap.Bind(Keys.Q, gt => MagicBrosMario.INSTANCE.Exit()); 
        
        Action<GameTime> goDebug = gt => MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new DebugRoom());
        Action<GameTime> goLevel1 = gt => MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level1());
        Action<GameTime> goLevel2 = gt => MagicBrosMario.INSTANCE.CurrentState = new TransitionState(new Level2());


        keysNMouseInputMap.Bind(Keys.R, gt => MagicBrosMario.INSTANCE.CurrentState =new TitleScreenState()); // reset game
        keysNMouseInputMap.Bind(Keys.X, goDebug);
        keysNMouseInputMap.Bind(Keys.Y, goLevel1);
        keysNMouseInputMap.Bind(Keys.Z, goLevel2);
        // mouse inputs
        //keysNMouseInputMap.Bind(m => m.IsButtonDown(MouseButton.Right), gt => MagicBrosMario.INSTANCE.Exit());
        //keysNMouseInputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X < gameData.halfX, goDebug);//If you click the left side of the screen, call DebugRomm()
        //keysNMouseInputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X >= gameData.halfX, goLevel1); //If you click the right side of the screen, call Level1()

        MarioBinds();

        SetGamePad();

    }

    private static void MarioBinds()
    {
        //Keyboard inputs
        keysNMouseInputMap.Bind(Keys.W, gt => MagicBrosMario.INSTANCE.Mario.Jump(gt));
        keysNMouseInputMap.Bind(Keys.Up, gt => MagicBrosMario.INSTANCE.Mario.Jump(gt));
        keysNMouseInputMap.Bind(Keys.Space, gt => MagicBrosMario.INSTANCE.Mario.Jump(gt));
        keysNMouseInputMap.Bind(Keys.A, gt => MagicBrosMario.INSTANCE.Mario.Left(gt));
        keysNMouseInputMap.Bind(Keys.Left, gt => MagicBrosMario.INSTANCE.Mario.Left(gt));
        keysNMouseInputMap.Bind(Keys.S, gt => MagicBrosMario.INSTANCE.Mario.Crouch(gt));
        keysNMouseInputMap.Bind(Keys.Down, gt => MagicBrosMario.INSTANCE.Mario.Crouch(gt));
        keysNMouseInputMap.Bind(Keys.D, gt => MagicBrosMario.INSTANCE.Mario.Right(gt));
        keysNMouseInputMap.Bind(Keys.Right, gt => MagicBrosMario.INSTANCE.Mario.Right(gt));
        keysNMouseInputMap.Bind(Keys.Z, gt => MagicBrosMario.INSTANCE.Mario.Attack());
        keysNMouseInputMap.Bind(Keys.N, gt =>  MagicBrosMario.INSTANCE.Mario.Attack());
        keysNMouseInputMap.Bind(Keys.E, gt =>  MagicBrosMario.INSTANCE.Mario.TakeDamage());
        keysNMouseInputMap.Bind(Keys.Q, gt => MagicBrosMario.INSTANCE.Exit()); 
        keysNMouseInputMap.Bind(Keys.R, gt => MagicBrosMario.INSTANCE.CurrentState =new TitleScreenState()); // reset MagicBrosMario.INSTANCE
    }

    private static void SetGamePad()
    {
        gamePadNStickInputMap = new GamePadNStickCommandMapper();
        gamePadNStickInputMap.SetFromKeyboardMapper(keysNMouseInputMap);
    }
    public static void Update(GameTime gameTime)
    {
        if(!muted){
            MNKUpdate(gameTime);
            GPUpdate(gameTime);
        }
    }

    private static void MNKUpdate(GameTime gt)
    {
        KeyboardInfo keyb = gameData.keyb;
            MouseInfo mouse = gameData.mouse;
            keyb.Update();
            mouse.Update();
            
            Player player = MagicBrosMario.INSTANCE.Mario;
            
            keysNMouseInputMap.ProcessInput(gt, keyb, mouse);// check all the inputs of the mouse and keyboard and run their corresponding function

            if (!keyb.IsKeyDown(Keys.S) && !keyb.IsKeyDown(Keys.Down))
            {
                player.ReleaseCrouch();
            }

            bool moving =
                keyb.IsKeyDown(Keys.A) || keyb.IsKeyDown(Keys.Left) ||
                keyb.IsKeyDown(Keys.D) || keyb.IsKeyDown(Keys.Right) ||
                keyb.IsKeyDown(Keys.S) || keyb.IsKeyDown(Keys.Down) ||
                keyb.IsKeyDown(Keys.W) || keyb.IsKeyDown(Keys.Up);
            if(!moving)
            {
                player.Idle();
            }
    }

    private static void GPUpdate(GameTime gt)
    {
        GamePadInfo gamepad = gameData.gamepad;
        gamepad.Update();

        Player player = MagicBrosMario.INSTANCE.Mario;

        gamePadNStickInputMap.ProcessInput(gt, gamepad);

        if (!gamepad.IsButtonDown(Buttons.LeftThumbstickDown) && !gamepad.IsButtonDown(Buttons.DPadDown))
        {
            player.ReleaseCrouch();
        }

        bool moving =
            gamepad.IsButtonDown(Buttons.LeftThumbstickLeft) || gamepad.IsButtonDown(Buttons.DPadLeft) ||
            gamepad.IsButtonDown(Buttons.LeftThumbstickRight) || gamepad.IsButtonDown(Buttons.DPadRight) ||
            gamepad.IsButtonDown(Buttons.LeftThumbstickDown) || gamepad.IsButtonDown(Buttons.DPadDown) ||
            gamepad.IsButtonDown(Buttons.LeftThumbstickUp) || gamepad.IsButtonDown(Buttons.DPadUp);
        if(!moving)
        {
            player.Idle();
        }
    }

    public static bool IsMuted()
    {
        return muted;
    }
    public static void Mute()
    {
        muted = true;
    }
    public static void UnMute()
    {
        muted = false;
    }

    public static bool IsKeyDown(Keys k)
    {
        Buttons butt = (Buttons)k;
        if(k == Keys.Down || k == Keys.S){
            butt = Buttons.LeftThumbstickDown;
        }
        return gameData.keyb.IsKeyDown(k) || gameData.gamepad.IsButtonDown(butt);
    }

    public static bool IsMarioDown()
    {
        return  gameData.keyb.IsKeyDown(Keys.S) || gameData.keyb.IsKeyDown(Keys.Down) || gameData.gamepad.IsButtonDown(Buttons.LeftThumbstickDown);
    }

}