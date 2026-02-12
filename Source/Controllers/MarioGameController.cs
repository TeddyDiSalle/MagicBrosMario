using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace MagicBrosMario.Source;

//For sprint2
public class MarioGameController : IController{
    //Classes should only worry about game actions not if "w" is pressed
    // This class will call the other's cllasses move functions and such
    ISprite[] sprites;
    IController[] keyboardNMouse;
    private int[] cords;
    private MagicBrosMario game;
    public MarioGameController(MagicBrosMario g, ISprite[] Sprites, IController[] Controllers, int[] halfCords )
    {
        game =g;
        sprites = Sprites;
        keyboardNMouse = Controllers;
        cords = halfCords;
    }
    public void Update()
    {
        KeyboardInfo keyboardInput = (KeyboardInfo)keyboardNMouse[0];
        MouseInfo mouseInput = (MouseInfo)keyboardNMouse[1];
        keyboardInput.Update();
        mouseInput.Update();
        Point position = mouseInput.Position;


        if (keyboardInput.IsKeyDown(Keys.D0) || mouseInput.IsButtonDown(MouseButton.Right))
        {
            game.Exit();
        }
        else if (keyboardInput.IsKeyDown(Keys.D1) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X < cords[0] && position.Y < cords[1]))
        {
            game.SetCurrentSprite(sprites[0]);
        }
        else if (keyboardInput.IsKeyDown(Keys.D2) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X > cords[0] && position.Y < cords[1]))
        {
            game.SetCurrentSprite(sprites[1]);
        }
        else if (keyboardInput.IsKeyDown(Keys.D3) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X < cords[0] && position.Y > cords[1]))
        {
            game.SetCurrentSprite(sprites[2]);

        }
        else if (keyboardInput.IsKeyDown(Keys.D4) || (mouseInput.IsButtonDown(MouseButton.Left) && position.X > cords[0] && position.Y > cords[1]))
        {
            game.SetCurrentSprite(sprites[3]);
        }
    }
    
    /* Player Controls */
    private void moveDirection(){
        // wasd and arrows
    }

    private void attack()
    {
        // mario shoots a fireball with z or n
    }

    private void takeDamage()
    {
        // mario is damaged with e
    }
    /* Block/obstacle controls */
    private void rotateBlock()
    {
        // t is previous, y is next
    }

    /* Item Controls */
    private void rotateItem(){
        // press u for previous, 
        // press i for next
    }

    /* Enemy/NPC (other chartacter) controls */
    private void rotateEnemy(){
        //press o for previous
        //press p for next
    }

    /* Other controls */
    private void quit()
    {
        // q
    }

    private void reset()
    {
        // r
    }
}