using MagicBrosMario.Source.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class FireMarioJumpState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;
    private readonly Sprite.ISprite[] Sprites;
    private const int maxJumpCalls = 7;
    private int JumpCalls = 0;
    private bool IsAttacking = false;
    public FireMarioJumpState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [
            texture.NewSprite(69, 164, 16, 32),
            texture.NewAnimatedSprite(69, 164, 16, 32, 4, timeFrame/4),
            texture.NewSprite(136, 269, 16, 32),
            texture.NewAnimatedSprite(136, 269, 16, 32, 4, timeFrame/4)
            ];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
        }
        CurrentSprite = Sprites[0];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 32 * scaleFactor);
        Mario.IsJumping = true;
        SoundEffectController.PlaySound(SoundTypes.JumpSuper, 1.0f);
        //Mario.IsGrounded = false;
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
        if (Mario.IsJumping && JumpCalls < maxJumpCalls)
        {
            Mario.MoveUp(gameTime, 0.3f);
            JumpCalls++;
        }
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new FireMarioCrouchState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Attack()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;
            Mario.CreateFireball();
            SoundEffectController.PlaySound(SoundTypes.Fireball, 1.0f);
        }
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.ChangeState(new SmallMarioJumpState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                //Nothing
                break;
            case Power.Mushroom:
                Mario.ChangeState(new BigMarioJumpState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
        }
    }
    public Power GetCurrentPower()
    {
        return Power.FireFlower;
    }
    public void Idle()
    {
        //Nothing
    }
    public void StateChangePrep()
    {
        CurrentSprite.Visible = false;

        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Drop();
        }
    }
    private void SwitchSprite(int index)
    {
        CurrentSprite.Visible = false;
        CurrentSprite = Sprites[index];
        CurrentSprite.Visible = true;
    }
    public void Update(GameTime gameTime)
    {
        if (Mario.Invincible)
        {
            Mario.StarTimeRemaining += gameTime.ElapsedGameTime.TotalSeconds;
            SwitchSprite((IsAttacking) ? 3 : 1);
        }
        else
        {
            SwitchSprite((IsAttacking) ? 2 : 0);
        }
        CurrentSprite.Update(gameTime);
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        CurrentSprite.Flipped = Mario.Flipped;
        if (Mario.IsGrounded)
        {
            Mario.IsJumping = false;
        }
        if (Mario.Velocity.Y == 0)
        {
            Mario.ChangeState(new FireMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
        IsAttacking = false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
