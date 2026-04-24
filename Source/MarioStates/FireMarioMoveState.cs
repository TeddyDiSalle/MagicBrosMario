using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class FireMarioMoveState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;

    private ISprite CurrentSprite;

    private int Frame = 0;
    private readonly float timeFrame;
    private double timer = 0;
    private readonly int scaleFactor;

    private readonly ISprite[] Sprites;

    private bool Braking;
    private bool IsAttacking = false;
    private double AttackTimer = 0;
    public FireMarioMoveState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        Sprites = [
        texture.NewAnimatedSprite(2, 199, 16, 32, 4, timeFrame), //Normal Walking
        texture.NewAnimatedSprite(2, 234, 16, 32, 16, timeFrame/4),//Rainbow Walking
        texture.NewSprite(69, 199, 16, 32), //Normal Braking
        texture.NewAnimatedSprite(69, 199, 16, 32, 4, timeFrame/4), //Rainbow Braking
        texture.NewAnimatedSprite(2, 269, 16, 32, 4, timeFrame), //Attack Walking
        texture.NewAnimatedSprite(2, 304, 16, 32, 16, timeFrame/4), //Rainbow Attack Walking
        texture.NewSprite(69, 269, 16, 32), //Attack Braking
        texture.NewAnimatedSprite(69, 269, 16, 32, 4, timeFrame/4) //Rainbow Attack Braking
        ];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
        }
        CurrentSprite = Sprites[Frame];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 32 * scaleFactor);
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
            Mario.ChangeState(new SmallMarioMoveState(Mario));
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
                Mario.ChangeState(new BigMarioMoveState(Mario));
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Power.Cloud:
                //Nothing
                break;
        }
    }
    public Power GetCurrentMode()
    {
        return Power.FireFlower;
    }
    public void Idle()
    {
        Mario.ChangeState(new FireMarioIdleState(Mario));
    }
    public void StateChangePrep()
    {
        CurrentSprite.Visible = false;
        for(int i = 0; i < Sprites.Length; i++)
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
            Frame = 2;
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

        if (Frame == 2)
        {
            Frame = 0;
        }
        else if (Frame == 3)
        {
            Frame = 1;
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
            Frame = 3;
        }
        else
        {
            Frame = 1;
        }    
    }
    public void Update(GameTime gameTime)
    {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        timer += time;
        AttackTimer += time;
        
        UpdateMovementAnimations(gameTime);
        UpdateStarAnimations(time);
        if (IsAttacking)
        {
            SwitchSprite(Frame + 4);
            if(AttackTimer > timeFrame)
            {
                IsAttacking = false;
                AttackTimer = 0;
            }
        }
        else
        {
            SwitchSprite(Frame);
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
