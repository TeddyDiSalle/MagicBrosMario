# Sprint4 Code Review

## Readability Review by Vincent Do
Source\Block\PipeEntryBlock.cs
[PipeEntryBlock.cs](../Source\Block\PipeEntryBlock.cs) by Chuang

- Date: 04.13.2026
- Time: 4 minutes

### Comments

>The logic is a bit confusing and not exactly straight forward but overall what it does is simple.

---

## Maintainability Review by Vincent Do

[TitleScreenState.cs](..\Source\GameStates\TitleScreenState.cs) by Brian

- Date: 04.13.2026
- Time: 1 minutes

### Comments

>Very simple. Could be more maintainable if you can send a type of level that the title screen should go to when you press enter instead of hard coding it to level 1 but in this case there's no other level so its fine.
___
## Readability Review by Brian Perusek

[Class.cs](../Source/Level/DebugRoom.cs) by Teddy

- Date: 04.13.2026
- Time: 2 minutes

### Comments

>Very readable. Pretty simple class for the most part. the way you drew the map out lined up perfectly with the actual debug room so it was very easy to understand

---

## Maintainability Review by Brian Perusek

[Class.cs](../Source/Enemies/Fireball.cs) by Roshan

- Date: 04.13.2026
- Time: 3 minutes

### Comments

>Pretty nice class. The naming conventions are a little confusing to understand, so the maintainability is kind of iffy, but once you get a hang of it its not too bad.

---

## Readability Review by Teddy DiSalle

[Class.cs](../Source/Class.cs) by Author

- Date: 04.13.2026
- Time: 3 minutes
### Comments

> 

---

## Maintainability Review by Teddy DiSalle

[Class.cs](../Source/Class.cs) by Author

- Date: 04.13.2026
- Time: 3 minutes

### Comments

>

___

## Readability Review by Chuang-Yun Huang

[Class.cs](../Source/Class.cs) by Author

- Date: 04.13.2026
- Time: 3 minutes

### Comments

>

---

## Maintainability Review by Chuang-Yun Huang

[Class.cs](../Source/Class.cs) by Author

- Date: 04.13.2026
- Time: 3 minutes

### Comments

> 

---

## Readability Review by Roshan Ramamurthy

[MarioFireball.cs](..\Source\MarioStates\MarioFireball.cs) by Vincent

- Date: 04.13.2026
- Time: 3 minutes

### Comments

> The class is pretty easy to follow — the fireball lifecycle from spawn to explosion to cleanup is laid out in a logical order. The collision handling with UnCollide is straightforward and the bounce behavior on CollideDirection.Down makes sense. Sprite switching between the fireball and explosion is clean and simple. One issue I see is some naming like contantCD which looks like a typo and getCollected which is misleading since it just checks if the fireball expired. 

---

## Maintainability Review by Roshan Ramamurthy

[HUD.cs](..\Source\GameEventMangerAndHUD\HUD.cs) by Vincent

- Date: 04.13.2026
- Time: 3

### Comments

> The event system with SendEvent is a nice way to keep scoring logic in one place so other classes don't need to worry about points. Only concern is the class handles sound, scoring, time, and drawing all in one place so changing one thing could accidentally break something else. The class basically controls the game.