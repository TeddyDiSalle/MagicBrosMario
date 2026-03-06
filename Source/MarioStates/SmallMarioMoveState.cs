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

    public SmallMarioMoveState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [
            texture.NewAnimatedSprite(2, 21, 16, 16, 4, timeFrame),
            texture.NewAnimatedSprite(2, 40, 16, 16, 16, timeFrame/4),
            texture.NewSprite(69, 21, 16, 16),
            texture.NewAnimatedSprite(69, 21, 16, 16, 4, timeFrame/4)
        ];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
        CurrentSprite = Sprites[0];
        Rectangle currentHitBox = Mario.collision.CollisionBox;
        Mario.collision.CollisionBox = new Rectangle(currentHitBox.X, currentHitBox.Y, 16, 16);
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime, 1);
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime, 1);
    }
    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new SmallMarioJumpState(Mario, texture, timeFrame, scaleFactor));
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
            Mario.ChangeState(new MarioDeadState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new FireMarioMoveState(Mario, texture, timeFrame, scaleFactor));
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
        return Power.None;
    }
    public void Idle()
    {
        Mario.ChangeState(new SmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
    }
    private void IsBraking(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        bool BrakingRight = !Flipped && Velocity.X < 0;
        bool BrakingLeft = Flipped && Velocity.X > 0;
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

        if (Braking && (!Flipped && Velocity.X >= 0 || Flipped && Velocity.X <= 0))
        {
            Braking = false;
        }
    }

    private void UpdateMovementAnimations(GameTime gameTime, Vector2 Velocity, bool Flipped)
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
        IsBraking(gameTime, Velocity, Flipped);
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
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        timer += time;

        UpdateMovementAnimations(gameTime, Velocity, Flipped);
        UpdateStarAnimations(time);

        CurrentSprite = Sprites[Frame];
        CurrentSprite.Update(gameTime);
        CurrentSprite.Flipped = Flipped;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }

}
