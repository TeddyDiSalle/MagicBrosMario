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
    public FireMarioMoveState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
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
        }
        CurrentSprite = Sprites[0];
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
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new FireMarioJumpState(Mario, texture, timeFrame, scaleFactor));
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
        }
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.ChangeState(new SmallMarioMoveState(Mario, texture, timeFrame, scaleFactor));
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
                Mario.ChangeState(new BigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
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
        Mario.ChangeState(new FireMarioIdleState(Mario, texture, timeFrame, scaleFactor));
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
            CurrentSprite = Sprites[Frame + 4];
            if(AttackTimer > timeFrame)
            {
                IsAttacking = false;
                AttackTimer = 0;
            }
        }
        else
        {
            CurrentSprite = Sprites[Frame];
        }
        CurrentSprite.Update(gameTime);
        CurrentSprite.Flipped = Mario.Flipped;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }
}
