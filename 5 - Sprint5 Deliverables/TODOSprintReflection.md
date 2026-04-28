# Sprint 5 Reflection – MagicBrosMario

## Sprint Summary
During Sprint 5, our team focused on making level 2 and its new blocks, items, enemies, and power up. This includes minecraft textured blocks, bridges and axes for bowser boss fight, a new enemy called meteor mage, and a item and power up that reduces Mario's gravity. We also improved the behavior of the flagpole, pipe travleing animations, and refactoring.

## What Went Well
- The game now has a complete gameplay loop with a start screen, transitions, a first level, second level, and a debug room.

## What Was Challenging
- There is a problem with false collision triggers where hugging a wall of blocks and jump could result in a top collision or landing on the ground may trigger a side collision.
- The gamestate transitions caused many problems during this sprint and the code logic for it is scattered all over the place in MagicBrosMario.cs, HUD.cs, and the other game state classes.

## Team Process Reflection
Our team benefited from keeping a debug room and quick room-switching controls in the build. That made it much easier to test one feature at a time without replaying the whole level repeatedly.


## What We Would Improve Next Sprint
- Continue refining pipe entry/exit logic and camera transitions.
- Add more dedicated helper classes where large files are becoming hard to read.
- Continue improving documentation and code review records as features stabilize.

## Overall Assessment
Our performance this sprint was noticeably better than the previous sprint due to our increased proactiveness in completing our work early enough to work out any problems during implementation. Although there are a couple of bugs that we haven't figured out how to fix including the false collisions described above and Mario not appearing after a level transition until player input.