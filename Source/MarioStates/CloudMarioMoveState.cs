using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class CloudMarioMoveState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;

    private Sprite.ISprite CurrentSprite;
    private int spriteIndex = (int)MoveEnums.regularMove;
    private readonly float timeFrame;
    private double timer = 0;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;
    private bool Braking;
    public CloudMarioMoveState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        Sprites = [
            texture.NewAnimatedSprite(2, 374, 16, 32, 4, timeFrame),
            texture.NewAnimatedSprite(2, 409, 16, 32, 16, timeFrame/4),
            texture.NewSprite(69, 374, 16, 32),
            texture.NewAnimatedSprite(69, 374, 16, 32, 4, timeFrame/4)
        ];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
            Sprites[i].Depth = 0.6f;
        }
        CurrentSprite = Sprites[(int)MoveEnums.regularMove];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16*scaleFactor, 32*scaleFactor);
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
    }
    public void Jump(GameTime gameTime)
    {
        if (!Mario.IsGrounded && !Mario.WasGrounded) { return; }
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new CloudMarioJumpState(Mario));
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new CloudMarioCrouchState(Mario));
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.ChangeState(new SmallMarioMoveState(Mario));
        }
    }
    public void PowerUp(Enums power)
    {
        switch (power)
        {
            case Enums.FireFlower:
                Mario.ChangeState(new FireMarioMoveState(Mario));
                break;
            case Enums.Mushroom:
                //Nothing
                break;
            case Enums.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Enums.Cloud:
                //Nothing
                break;
        }
    }

    public Enums GetCurrentMode()
    {
        return Enums.Cloud;
    }
    public void Idle()
    {
        Mario.ChangeState(new CloudMarioIdleState(Mario));
    }
    public void StateChangePrep()
    {
        CurrentSprite.Visible = false;
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Drop();
        }
    }
    public void SetVisibility(bool visible)
    {
        CurrentSprite.Visible = visible;
    }
    private void SwitchSprite(int index)
    {
        CurrentSprite.Visible = false;
        CurrentSprite = Sprites[index];
        CurrentSprite.Visible = true;
    }
    private void IsBraking(GameTime gameTime)
    {
        bool BrakingRight = !Mario.Flipped && Mario.Velocity.X < 0;
        bool BrakingLeft = Mario.Flipped && Mario.Velocity.X > 0;
        if (BrakingRight || BrakingLeft)
        {
            spriteIndex = (int)MoveEnums.regularBrake;
            timer = 0;
            Braking = true;
            if (BrakingRight)
            {
                Mario.MoveRight(gameTime, 8);
            }
            else
            {
                Mario.MoveLeft(gameTime, 8);
            }
        }

        if(Braking && (!Mario.Flipped && Mario.Velocity.X >= 0 || Mario.Flipped && Mario.Velocity.X <= 0))
        {
            Braking = false;
        }
    }

    private void UpdateMovementAnimations(GameTime gameTime)
    {
        if (timer <= timeFrame) { return; }

        if (spriteIndex == (int)MoveEnums.regularBrake)
        {
            spriteIndex = (int)MoveEnums.regularMove;
        }
        else if (spriteIndex == (int)MoveEnums.starBrake)
        {
            spriteIndex = (int)MoveEnums.starMove;
        }
        IsBraking(gameTime);
        timer = 0;
    }

    private void UpdateStarAnimations(double time)
    {
        if (!Mario.Invincible) { return; }
        Mario.StarTimeRemaining += time;
        if (Braking)
        {
            spriteIndex = (int)MoveEnums.starBrake;
        }
        else
        {
            spriteIndex = (int)MoveEnums.starMove;
        }
    }
    public void Update(GameTime gameTime)
    {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        timer += time;
        
        UpdateMovementAnimations(gameTime);
        UpdateStarAnimations(time);
        SwitchSprite(spriteIndex);

        CurrentSprite.Update(gameTime);
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        CurrentSprite.HFlipped = Mario.Flipped;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }
}
