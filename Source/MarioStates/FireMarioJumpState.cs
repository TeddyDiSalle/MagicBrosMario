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
    private bool IsAttacking = false;
    public FireMarioJumpState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
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
        SoundController.PlaySound(SoundType.JumpSuper, 1.0f);
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
        if (Mario.IsJumping && Mario.JumpCalls < Player.maxJumpCalls)
        {
            Mario.MoveUp(gameTime, 0.3f);
            Mario.JumpCalls++;
        }
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new FireMarioCrouchState(Mario));
    }
    public void Attack()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;
            Mario.CreateFireball();
        }
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.ChangeState(new SmallMarioJumpState(Mario));
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
                Mario.ChangeState(new BigMarioJumpState(Mario));
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Power.Cloud:
                Mario.ChangeState(new CloudMarioJumpState(Mario));
                break;
        }
    }
    public Power GetCurrentMode()
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
        CurrentSprite.HFlipped = Mario.Flipped;
        if (Mario.IsGrounded)
        {
            Mario.IsJumping = false;
        }
        IsAttacking = false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
