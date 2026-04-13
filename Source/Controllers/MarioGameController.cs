// Made by Teddy DiSalle
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.GameStates;
using System.Diagnostics;
using MagicBrosMario.Source.Level;


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
        SetSprint3Binds();
    }
    public struct Sprint2Controller {
        public MouseInfo mouse;
        public  KeyboardInfo keyb;
        public int halfX;
        public int halfY;
    }
    // All the binds specified for sprint 2
    public void SetSprint3Binds()
    {
        //Keyboard inputs
        inputMap.Bind(Keys.W, gt => MagicBrosMario.INSTANCE.Mario.Jump(gt));
        inputMap.Bind(Keys.Up, gt => MagicBrosMario.INSTANCE.Mario.Jump(gt));
        inputMap.Bind(Keys.Space, gt => MagicBrosMario.INSTANCE.Mario.Jump(gt));
        inputMap.Bind(Keys.A, gt => MagicBrosMario.INSTANCE.Mario.Left(gt));
        inputMap.Bind(Keys.Left, gt => MagicBrosMario.INSTANCE.Mario.Left(gt));
        inputMap.Bind(Keys.S, gt => MagicBrosMario.INSTANCE.Mario.Crouch(gt));
        inputMap.Bind(Keys.Down, gt => MagicBrosMario.INSTANCE.Mario.Crouch(gt));
        inputMap.Bind(Keys.D, gt => MagicBrosMario.INSTANCE.Mario.Right(gt));
        inputMap.Bind(Keys.Right, gt => MagicBrosMario.INSTANCE.Mario.Right(gt));
        inputMap.Bind(Keys.Z, gt => MagicBrosMario.INSTANCE.Mario.Attack());
        inputMap.Bind(Keys.N, gt =>  MagicBrosMario.INSTANCE.Mario.Attack());
        inputMap.Bind(Keys.E, gt =>  MagicBrosMario.INSTANCE.Mario.TakeDamage());
        inputMap.Bind(Keys.Q, gt => game.Exit()); 
        inputMap.Bind(Keys.R, gt => MagicBrosMario.INSTANCE.CurrentState =(new TitleScreenState(MagicBrosMario.INSTANCE))); // reset game

        // mouse inputs
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Right), () => game.Exit());
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X < gameData.halfX, () => 
                MagicBrosMario.INSTANCE.CurrentState = new TransitionState(MagicBrosMario.INSTANCE, new DebugRoom()));//If you click the left side of the screen, call DebugRomm()
        
        inputMap.Bind(m => m.IsButtonDown(MouseButton.Left) && m.Position.X >= gameData.halfX, () => 
                MagicBrosMario.INSTANCE.CurrentState = new TitleScreenState(MagicBrosMario.INSTANCE)); //If you click the right side of the screen, call Level1()
    }
    public void Update(GameTime gameTime)
    {
        KeyboardInfo keyb = gameData.keyb;
        MouseInfo mouse = gameData.mouse;
        keyb.Update();
        mouse.Update();
        
        Player player = MagicBrosMario.INSTANCE.Mario;
        
        inputMap.ProcessInput(gameTime, keyb, mouse);// check all the inputs of the mouse and keyboard and run their corresponding function

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

}