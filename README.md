# MagicBrosMario

MagicBrosMario is a Super Mario Bros. built in C# with MonoGame. The project includes a playable first level, a debug/testing room, a title screen, HUD, enemies, items, question blocks, level loading from CSV/XML data, music and sound effects, and pipe-based transitions.

## Team
- Brian
- Chuang
- Vincent
- Roshan
- Teddy DiSalle

## Current Sprint 4 Highlights
- Playable first level and debug room
- Title screen and transition state
- HUD with time limit
- Question blocks that hold items and coins
- Floating coins
- Pipes and level transitions
- Sound effects and background music
- Mouse shortcuts for quick room switching during testing

## Controls
### Keyboard
- `W`, `Up Arrow`, or `Space` - Jump
- `A` or `Left Arrow` - Move left
- `D` or `Right Arrow` - Move right
- `S` or `Down Arrow` - Crouch / enter some pipes
- `Z` or `N` - Attack / fireball when powered up
- `E` - Take damage (debug/testing)
- `R` - Reset to the title screen
- `Q` - Quit the game

### Mouse
- **Left click on left half of the screen** - Load the `DebugRoom`
- **Left click on right half of the screen** - Load `Level1`
- **Right click** - Quit the game

## How to Run
### Requirements
- .NET SDK compatible with the project file
- MonoGame DesktopGL dependencies

### Build and run
1. Open `MagicBrosMario.sln` in Visual Studio **or** run from the command line.
2. Restore tools and packages if needed.
3. Build the project.
4. Run the DesktopGL target.

Example command line flow:

```bash
 dotnet restore
 dotnet build MagicBrosMario.sln
 dotnet run --project MagicBrosMario.csproj
```

## Project Layout
```text
MagicBrosMario/
├── Content/                  # Sprite sheets, fonts, sounds, and level data
├── Source/
│   ├── Block/                # Blocks, pipe entry blocks, question blocks
│   ├── Collision/            # Collision interfaces and controller
│   ├── Controllers/          # Keyboard/mouse input and command mapping
│   ├── Enemies/              # Goomba, Koopa, Bowser, Piranha Plant, etc.
│   ├── GameStates/           # Title, transition, and playing states
│   ├── Items/                # Coins, mushrooms, star, platforms, flagpole
│   ├── Level/                # Level loading, block/item/enemy managers
│   ├── MarioStates/          # Mario power states, movement, collision handling
│   ├── Sound/                # Music and sound effect management
│   └── Sprite/               # Shared textures, animated sprites, camera
├── CodeReviews/              # Sprint review notes
├── 3 - Sprint3 Deliverables/
├── 4 - Sprint4 Deliverables/
└── README.md
```

## Architecture Overview
### Shared textures and sprites
The game uses a shared texture approach so only one `Texture2D` is created per sprite sheet. Individual sprites and animations reference a shared texture wrapper rather than each loading their own texture.

### Game states
The game currently uses separate states for:
- Title screen
- Transition screen
- Playing state

This helps keep setup, input flow, and drawing behavior separated.

### Levels
Levels are loaded from CSV and XML-backed data. The current system supports:
- Block layout
- Enemy placement
- Item placement
- Pipe links and deferred pipe endpoint resolution
- A dedicated debug room for testing features quickly

### Mario and collision
Mario behavior is organized through state classes for small, big, and fire forms. Collision handling is separated into a player collision handler so movement/state logic and collision response are not all mixed into one class.

## Implemented Features
- Title screen
- Transition state between scenes
- Debug room for testing
- Playable first level
- Player state system
- Enemies with collision
- Blocks and question blocks
- Items and power-ups
- Floating coin behavior
- HUD and time limit
- Pipe travel system
- Sound effects and music
- Reset and quick room switching for testing

## Known Issues
- The game does not always fully end after Mario reaches the flagpole.
- Pipe behavior is still being refined, and some pipe interactions can still kill Mario unexpectedly.
- Some pipe/camera transitions may still need adjustment in special cases.

## Testing Notes
The project intentionally keeps several debug-friendly controls in place:
- Quick reset with `R`
- Force damage with `E`
- Mouse-based room switching between the debug room and Level 1

These controls are useful during development and sprint demos.

## Sprint 4 Documentation Notes
For Sprint 4, the team focused on completing the first level, maintaining a debug/testing room, improving state transitions, and documenting code reviews and sprint reflection work.

## Credits
This project was created by the Magic Bros team.
