# MagicBrosMario
The Magic Bros create Mario in C#

Items: 
    Controls - Press 'i' to switch to the next item and 'u' to switch to the previous item

    How Classes Work - Each class works by taking in parameters of (texture, screenWidth, screenHeight, positionx, positionY). 
    Not all parameters are used by every class, but each item takes in the same ones as a way to make it more universal and easier to remember when calling the item.
    The non moving sprites are simple, just put on the screen with the given x and y coordinates.
    The moving sprites are a little more complicated, requires more variables such as speed and direction. In Update(), the sprite's position is calculated and it moves until it reaches the screen boundary, then moves the other way.
    The Star sprite is a little special, where it's y value increases until it reaches a certain spot,then it falls down to another given spot, all while going right/left until it reaches screen boundary.

    Problems - Theres only two issues that need to be worked on. The first is the coin class. In the game, once a question block it hit, the coin moves up a few pixels then dissappears. 
    I wasn't sure how we were gonna make it work in our game so I left that part out, but it's a very simple fix and wouldn't take long to implement.
    The other problem is a little trickier. The star class goes up and down diagonally, instead of a smooth curve when bouncing. 
    I wasn't really sure how to implement this, i'm sure it wouldn't take too long to fix though.