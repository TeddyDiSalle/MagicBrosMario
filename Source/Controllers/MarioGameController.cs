// Made by Teddy DiSalle
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        Player player = gameData.player; 
        //Keyboard inputs
        inputMap.Bind(Keys.D0,gt => game.Exit());
        inputMap.Bind(Keys.D1, gt => game.displayPowerUp(0));
        inputMap.Bind(Keys.D2, gt => game.displayPowerUp(1));
        inputMap.Bind(Keys.D3, gt => game.displayPowerUp(2));
        inputMap.Bind(Keys.D4, gt => game.displayPowerUp(3));
        inputMap.Bind(Keys.W, gt => player.Jump(gt));
        inputMap.Bind(Keys.Up, gt => player.Jump(gt));
        inputMap.Bind(Keys.A, gt => player.Left(gt));
        inputMap.Bind(Keys.Left, gt => player.Left(gt));
        inputMap.Bind(Keys.S, gt => player.Crouch(gt));
        inputMap.Bind(Keys.Down, gt => player.Crouch(gt));
        inputMap.Bind(Keys.D, gt => player.Right(gt));
        inputMap.Bind(Keys.Right, gt => player.Right(gt));
        inputMap.Bind(Keys.Z, gt => player.Attack());
        inputMap.Bind(Keys.N, gt =>  player.Attack());
        inputMap.Bind(Keys.E, gt =>  player.TakeDamage());
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
        KeyboardInfo keyb = gameData.keyb;
        MouseInfo mouse = gameData.mouse;
        keyb.Update();
        mouse.Update();
        
        inputMap.ProcessInput(gameTime, keyb, mouse);// check all the inputs of the mouse and keyboard and run their corresponding function

        if (!keyb.IsKeyDown(Keys.S) && !keyb.IsKeyDown(Keys.Down))
        {
            gameData.player.ReleaseCrouch();
        }

        bool moving =
            keyb.IsKeyDown(Keys.A) || keyb.IsKeyDown(Keys.Left) ||
            keyb.IsKeyDown(Keys.D) || keyb.IsKeyDown(Keys.Right) ||
            keyb.IsKeyDown(Keys.S) || keyb.IsKeyDown(Keys.Down) ||
            keyb.IsKeyDown(Keys.W) || keyb.IsKeyDown(Keys.Up);
        if(!moving)
        {
            gameData.player.Idle();
        }
    
    }

}