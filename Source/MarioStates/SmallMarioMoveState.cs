using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class SmallMarioMoveState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;

    private Sprite.ISprite CurrentSprite;

    private int Frame = 0;

    private readonly float timeFrame;
    private double timer = 0;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;
    private bool Braking;

    public SmallMarioMoveState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        Sprites = [
            texture.NewAnimatedSprite(2, 21, 16, 16, 4, timeFrame),
            texture.NewAnimatedSprite(2, 40, 16, 16, 16, timeFrame/4),
            texture.NewSprite(69, 21, 16, 16),
            texture.NewAnimatedSprite(69, 21, 16, 16, 4, timeFrame/4)
        ];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
        }
        CurrentSprite = Sprites[0];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 16 * scaleFactor);
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
        Mario.ChangeState(new SmallMarioJumpState(Mario));
    }
    public void Crouch(GameTime gameTime)
    {
        //Nothing
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.KillMario();
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new FireMarioMoveState(Mario));
                break;
            case Power.Mushroom:
                Mario.ChangeState(new BigMarioMoveState(Mario));
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Power.Cloud:
                Mario.ChangeState(new CloudMarioMoveState(Mario));
                break;
        }
    }
    public Power GetCurrentPower()
    {
        return Power.None;
    }
    public void Idle()
    {
        Mario.ChangeState(new SmallMarioIdleState(Mario));
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

        if (Braking && (!Mario.Flipped && Mario.Velocity.X >= 0 || Mario.Flipped && Mario.Velocity.X <= 0))
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
        else if(Frame == 3)
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

        UpdateMovementAnimations(gameTime);
        UpdateStarAnimations(time);
        SwitchSprite(Frame);
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        CurrentSprite.Update(gameTime);
        CurrentSprite.HFlipped = Mario.Flipped;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
