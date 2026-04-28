
namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public enum Enums
{
    None,
    Mushroom,
    FireFlower,
    Star,
    Cloud
}
public enum CrouchEnums { 
    regularCrouch,
    starCrouch 
}

public enum IdleEnums
{
    regularIdle,
    starIdle
}
public enum JumpEnums
{
    regularJump,
    starJump
}
public enum SlideEnums
{
    sliding,
    landed
}
public enum MoveEnums
{
    regularMove,
    starMove,
    regularBrake,
    starBrake
}

public enum Attack2Enum { normalAttack = 2, starAttack }