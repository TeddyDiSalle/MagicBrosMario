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

[Class.cs](Source/GameStates/PlayingState.cs) by Brian

- Date: 04.13.2026
- Time: 3 minutes
### Comments

> The state class keeps level update flow centralized and readable. It is a good place for high-level orchestration, but it should avoid absorbing gameplay logic. As more features are added, it may need stricter boundaries between level setup, camera logic, and player lifecycle management. Maybe make a Parent class so the repeated logic can be changed in one spot since some things don't have to change between IGameStates

---

## Maintainability Review by Teddy DiSalle

[Class.cs](Source/Enemies/Goomba.cs) by Roshan

- Date: 04.13.2026
- Time:  4 minutes

### Comments

>Enemy behavior is reasonably isolated from level loading. Collision and update behavior are straightforward. Shared enemy patterns could eventually be moved into a base behavior helper if more enemies are added. If the team wanted to add enemy activation based on camera position, the current structure could support it because enemies are already managed through the level update cycle.

___

## Readability Review by Chuang-Yun Huang

[Class.cs](Source/MarioStates/PlayerCollisionHandler.cs) by Vincent

- Date: 04.13.2026
- Time: 3 minutes

### Comments

>Collision response is centralized, which helps avoid scattering collision logic across multiple classes. The file is becoming large and would benefit from extracting pipe travel or item/enemy collision helpers. A future change such as adding more environment-specific collision types may make this file harder to maintain unless helpers are introduced. If the game added multiple pipe destination types or scripted exits, the current implementation could support it, but it would be easier with a dedicated pipe travel service/helper.

---

## Maintainability Review by Chuang-Yun Huang

[PlayingState.cs](..\Source\GameStates\PlayingState.cs) by Brian

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

> The class is pretty easy to follow � the fireball lifecycle from spawn to explosion to cleanup is laid out in a logical order. The collision handling with UnCollide is straightforward and the bounce behavior on CollideDirection.Down makes sense. Sprite switching between the fireball and explosion is clean and simple. One issue I see is some naming like contantCD which looks like a typo and getCollected which is misleading since it just checks if the fireball expired. 

---

## Maintainability Review by Roshan Ramamurthy

[HUD.cs](..\Source\GameEventMangerAndHUD\HUD.cs) by Vincent

- Date: 04.13.2026
- Time: 3

### Comments

> The event system with SendEvent is a nice way to keep scoring logic in one place so other classes don't need to worry about points. Only concern is the class handles sound, scoring, time, and drawing all in one place so changing one thing could accidentally break something else. The class basically controls the game.