# MagicBrosMario
The Magic Bros create Mario in C#

# Sprint2 Documentation


## Texture and Sprites
We implemented a system where only one Texture2D instance is created for each sprite sheet and only one animation algorithm is used across different part of the game.

### Texture
- The [SharedTexture](Source/Sprite/SharedTexture.cs) class is just a wrapper with two method for creating non-animated and animated sprite
- Since the sprites reference the SharedTexture, we can put every sprite creation inside constructor and bind the texture later in the LoadContent phase.

### Sprites
- The [sprite](Source/Sprite/Sprite.cs) class represents a non-ainmated sprite with public properties including position and scale
- The [sprite](Source/Sprite/AnimatedSprite.cs) class represents a ainmated sprite with public properties including position and scale, and increments the frame based on the gameTime.
- With these two classes, every items, blocks, enemies and player can hold one or more sprite internally to avoid having to implement the drawing logic

## Items

### Controls 
Press `i` to switch to the next item and `u` to switch to the previous item

### How Item Classes Work
- Each class works by taking in parameters of (texture, screenWidth, screenHeight, positionx, positionY). 
- Not all parameters are used by every class, but each item takes in the same ones as a way to make it more universal and easier to remember when calling the item.
- The non moving sprites are simple, just put on the screen with the given x and y coordinates.
- The moving sprites are a little more complicated, requires more variables such as speed and direction. In Update(), the sprite's position is calculated and it moves until it reaches the screen boundary, then moves the other way.
- The Star sprite is a little special, where it's y value increases until it reaches a certain spot,then it falls down to another given spot, all while going right/left until it reaches screen boundary.

### Problems
- Theres only two issues that need to be worked on. The first is the coin class. In the game, once a question block it hit, the coin moves up a few pixels then dissappears. 
- I wasn't sure how we were gonna make it work in our game so I left that part out, but it's a very simple fix and wouldn't take long to implement.
- The other problem is a little trickier. The star class goes up and down diagonally, instead of a smooth curve when bouncing. 
- I wasn't really sure how to implement this, i'm sure it wouldn't take too long to fix though.

## Blocks

### Controls 
Press `t` or `y` to switch between blocks

### Implementation Details
- There are 4 total classes for the blocks currently.
    - IBlock interface that describes the behavior for every class
    - BlockBase abstract class that have general methods(tracking position and size)
    - Block concret class for all the simple blocks(both animated and non-animated)
    - BlockFactory functional class with convenient methods to create different blocks
- Currently, the blocks are just a wrapper for the sprite(introduced above) with no additional functionality, more block features(like collision and events) will be implemented in sprint 3

### Problems
- The main problem is what method we should use to deserialize each level from a csv, json or even a blob file.
    - The csv approach seems to be the best since we can assign each block a number and some metadata like 0001,67,67 and editing the csv would be as easy as editing the correct row and column. However, this approaches requires us to write a lot of deserializing code to achieve the desire effect
    - Using json would be a lot easier, but the json format has too many freedom that makes the json a lot harder to read compare to csvs and we most likely would not need it
- Another minor problem is how are we going to integrate the mario and the blocks together to achieve collision, event triggering. We should be able to figure this out during sprint 3


---

# Sprint3 Documentation
