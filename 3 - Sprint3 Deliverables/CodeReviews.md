# Sprint2 Code Review

## Readability Review by Vincent Do

[Bowser.cs](../Source/Enemies/Bowser.cs) by Roshan
- Date: 02.23.2026
- Time: 3 minutes

### Comments

> Comments: Everything is readable and easy to understand.

---

## Maintainability Review by Vincent Do

[BlockFactory.cs](../Source/Block/BlockFactory.cs) by Chuang
- Date: 02.23.2026
- Time: 4 minutes

### Comments

> Methods to create each block are identical except for xy offset. Methods are as simple as it can get and you can easily add more blocks using the same format since all of the blocks are static.
    
---

## Readability Review by Brian Perusek

[SharedTexture.cs](../Source/Sprite/SharedTexture.cs) by Chuang
- Date: 02.23.2026
- Time: 2 minutes

### Comments

> Everything is readable and easy to understand. All variables were given names that made it super easy to tell what they were and their purpose.

---

## Maintainability Review by Brian Perusek

[Player.cs](../Source/MarioStates/Player.cs) by Vincent
- Date: 02.23.2026
- Time: 3 minutes

### Comments
>  The code quality seems really good. Everything seems pretty simple and I don't think there can be much done to improve it.  A change we can make would be to add a state where Mario can crouch while being small. This would work fairly easy with the player class, as the code is basically all there and the sprite would just need to be added and the condition to change to that sprite.

---

## Readability Review by Teddy DiSalle

[Mushroom.cs](../Source/Items/Mushroom.cs) by Brian
- Date: 02.23.2026
- Time: 4 minutes

### Comments

> Pretty short class, most is easy to understand. The cordinates of the Sprite object are unlabeled so you would have to have an understanding of Sprite's parameters to understand those magic numbers.

---

## Maintainability Review by Teddy DiSalle

[FireMarioMoveState.cs](../Source/MarioStates/FireMarioMoveState.cs) by Vincent
- Date: 02.23.2026
- Time: 3 minutes

### Comments
> Very large class. The frame data for this class is in the form of magic numbers. A lot of these classes set the Frame varaible to 0 if it == 3 or sets it to 2 if it equals 0. It is very difficult to understand and will hard to maintain what these frames do. More magic numbers in the creation of the Sprite. Then no standardization of the speed mario goes or brakes. Every call in this class has 1 or 8 repeated over and over again. Could be centralized to one spot to control the change. This class is broken up into many functions with descriptive names This makes it easy to follow that.

---

## Readability Review by Chuang-Yun Huang

[KeysNMouseCommandMapper.cs](../Source/Controllers/KeysNMouseCommandMapper.cs) by Teddy
- Date: 02.23.2026
- Time: 3 minutes

### Comments

> The class is short with only two methods, but the implementation is smart as magic. This code turns all the if else switch logic into a dictionary that the game can just loop through. Because of this, the logic inside [MarioGameController.cs](../Source/Controllers/MarioGameController.cs) becomes extra clean. 10 out of 10

---

## Maintainability Review by Chuang-Yun Huang

[Star.cs](../Source/Items/Star.cs) by Vincent
- Date: 02.23.2026
- Time: 4 minutes

### Comments
> The class looks fine for most part. However it lacks some features that future us might need. Including getting the position and size of the star for collision detecting and instead of referncing the position of the sprite directly, it creates an internal position and updates the sprite every frame. Overall a solid implemetation.

---

## Readability Review by Roshan Ramamurthy

[MarioGameController.cs](../Source/Controllers/KeysNMouseCommandMapper.cs) by Teddy
- Date: 02.23.2026
- Time: 2 minutes

### Comments

> The class has a lot of information jumbled together, but once you take a minute to read it, it becomes very clear and easy to understand. Still has comments saying things need to be fixed even thought they were already fixed, so that may need to be updated.

---

## Maintainability Review by Roshan Ramamurthy

[Star.cs](../Source/Items/Mushroom.cs) by Brian
- Date: 02.23.2026
- Time: 5 minutes

### Comments
> The class generally looks fine, but the way the star moves is a little confusing and could be more clear. Additionally, the way the star moves needs to be fixed, it should look like its bouncing and not moving in a straight diagonal path. You should change the y velocity as it moves, similar to how it works for Mario when he jumps.
