# Sprint2 Code Review

## Readability Review by Vincent Do

[Axe.cs](..\Source\Items\Axe.cs) by Brian

- Date: 4.27.26
- Time: 1 minute

### Comments

> Very simple and readable class.

---

## Maintainability Review by Vincent Do

[BowserCollisionHandler.cs](..\Source\Enemies\BowserCollisionHandler.cs) by Roshan

- Date: 4.27.26
- Time: 1 minute

### Comments

> This class is very maintainable. It only focuses on responding to collisions involving bowser and the code is very simple.

---

## Readability Review by Brian Perusek

[CloudMarioIdleState.cs](../Source/MarioStates/CloudMarioIdleState.cs) by Vincent

- Date: 4/27
- Time: 2 minutes

### Comments

> Relatively simple class, pretty consistent with the other mario states

---

## Maintainability Review by Brian Perusek

[Level2.cs](../Source/Level/Level2.cs) by Teddy

- Date: 03.13.2026
- Time: 2 minutes

### Comments

> The code seems very maintainable. If you want to add more checkpoints, more blocks, more items, etc. then the class seems really easy to figure out how to implement it.

---

## Readability Review by Teddy DiSalle

[Class.cs](../Source/Sound/SoundController.cs) by Chuang-Yun

- Date: 4/27/2026
- Time: 1 minute

### Comments

> Took a little bit to understand since the update and list are dealing with the sounds' cooldown not the actual sound effects but that was the only bump in the road and everything else was very intuitive and understandable.

---

## Maintainability Review by Teddy DiSalle

[Class.cs](../Source/Items/AntiGravityCloud.cs) by Brian Perusek

- Date: 4/27/2026
- Time: 2 minutes

### Comments

>Pretty good, lots of abstraction so the class only has to deal with exactly what's different with it compared to other sprites, objects, and items. There are a few unanmed magic numbers particularly in the constructor as well as update. These can be identified with context, contructor knowledge, and nearby variable names. If just those were cleaned up, the whole thing could be easily changed by anyone or understood for debugging.  

---

## Readability Review by Chuang-Yun Huang

[Class.cs](../Source/Class.cs) by Author

- Date:
- Time:

### Comments

>

---

## Maintainability Review by Chuang-Yun Huang

[Class.cs](../Source/Class.cs) by Author

- Date:
- Time:

### Comments

>

---

## Readability Review by Roshan Ramamurthy

[AntiGravityCloud.cs](..\Source\Items\AntiGravityCloud.cs) by Brian

- Date: 4.27.26
- Time: 1 minute

### Comments

> Easy to follow, the constant names make it clear what the cloud does. The up and down floating logic is split nicely with hasRisen. The position property is lowercase which doesn't match the rest of the code, and the 40f should probably be a constant like the others.

---

## Maintainability Review by Roshan Ramamurthy

[GamePadNStickCommandMapper.cs](..\Source\Controllers\GamePadNStickCommandMapper.cs) by Teddy

- Date: 4.27.26
- Time: 1 minute

### Comments

> The RepeatBinding inner class keeps everything organized and SetFromKeyboardMapper is a nice way to avoid duplicating binds. There's some commented out keyboard code that should be deleted, and the while loop for repeat interval could have an infinite loop if the interval is 0.