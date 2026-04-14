# Sprint 4 Reflection – MagicBrosMario

## Sprint Summary
During Sprint 4, our team focused on finishing the first playable level and tying together the systems developed in previous sprints. We added or improved the title screen, HUD, item handling, question blocks, pipes, time limit behavior, sound, and the level-loading workflow. We also kept the debug room and mouse shortcuts so that testing specific features stayed fast.

## What Went Well
- We made strong progress on functional gameplay systems rather than isolated prototypes.
- The game now has a clearer play loop with a start screen, transitions, a first level, and a debug room.
- The project structure became more modular through separate folders for blocks, items, levels, enemies, sprites, sound, and Mario states.
- Pipe support and deferred pipe-link resolution helped make the level system more flexible.

## What Was Challenging
- Pipe travel and camera behavior introduced several edge cases.
- Integrating collisions, state transitions, and level loading created bugs that were harder to debug than isolated sprite or item systems.
- End-of-level behavior after the flagpole is not fully complete yet.
- Some pipe interactions can still place Mario incorrectly or result in an unintended death.

## Team Process Reflection
Our team benefited from keeping a debug room and quick room-switching controls in the build. That made it much easier to test one feature at a time without replaying the whole level repeatedly.

We also benefited from dividing work by subsystem, but integration remained the hardest part of the sprint. Systems often worked independently before they were combined, and many issues only appeared once multiple systems interacted at runtime.

## What We Would Improve Next Sprint
- Finish and polish end-of-level behavior after the flagpole.
- Continue refining pipe entry/exit logic and camera transitions.
- Add more dedicated helper classes where large files are becoming hard to read.
- Spend more time on integration testing earlier in the sprint, not just near the deadline.
- Continue improving documentation and code review records as features stabilize.

## Overall Assessment
This sprint was productive because the project moved from subsystem-heavy development toward a more complete playable experience. While some bugs remain, especially around pipes and end-of-level flow, the core game is significantly closer to a complete SMB level than it was in earlier sprints.
