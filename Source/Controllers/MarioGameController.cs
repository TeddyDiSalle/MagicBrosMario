using Microsoft.Xna.Framework.Input;

namespace MagicBrosMario.Source;

//For sprint2
public class MarioGameController{
    //Classes should only worry about game actions not if "w" is pressed
    // This class will call the other's cllasses move functions and such

    
    /* Player Controls */
    void moveDirection(){
        // wasd and arrows
    }

    void attack()
    {
        // mario shoots a fireball with z or n
    }

    void takeDamage()
    {
        // mario is damaged with e
    }
    /* Block/obstacle controls */
    void rotateBlock()
    {
        // t is previous, y is next
    }

    /* Item Controls */
    void rotateItem(){
        // press u for previous, 
        // press i for next
    }

    /* Enemy/NPC (other chartacter) controls */
    void rotateEnemy(){
        //press o for previous
        //press p for next
    }

    /* Other controls */
    void quit()
    {
        // q
    }

    void reset()
    {
        // r
    }
}