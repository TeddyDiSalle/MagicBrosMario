using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class FireMarioIdleState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;

    private bool IsAttacking = false;

    public FireMarioIdleState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;

        Sprites = [
            texture.NewSprite(2, 164, 16, 32),
            texture.NewAnimatedSprite(2, 164, 16, 32, 4, timeFrame/4),
            texture.NewSprite(136, 164, 16, 32),
            texture.NewAnimatedSprite(136, 164, 16, 32, 4, timeFrame / 4),
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
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
        Mario.ChangeState(new FireMarioMoveState(Mario));
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
        Mario.ChangeState(new FireMarioMoveState(Mario));
    }

    public void Jump(GameTime gameTime)
    {
        if (!Mario.IsGrounded && !Mario.WasGrounded) { return; }
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new FireMarioJumpState(Mario));
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
            Mario.ChangeState(new SmallMarioIdleState(Mario));
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
                Mario.ChangeState(new BigMarioIdleState(Mario));
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Power.Cloud:
                Mario.ChangeState(new CloudMarioIdleState(Mario));
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
        IsAttacking = false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }
}
