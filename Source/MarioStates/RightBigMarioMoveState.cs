using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightBigMarioMoveState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;

    private readonly Sprite.ISprite[] Frames;
    private Sprite.ISprite CurrentSprite;

    private int Frame = 0;
    private int nextFrame = 1;
    private readonly double timeFrame;
    private double timer = 0;
    private readonly int scaleFactor;

    private readonly Sprite.Sprite[] Sprites;
    private int StarFrame = 0;
    private double StarTimer = 0;

    public RightBigMarioMoveState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Frames = [texture.NewSprite(97, 3, 16, 30), //Walking1
            texture.NewSprite(115, 2, 14, 31), //Walking2
            texture.NewSprite(131, 1, 16, 32), //Walking3
            texture.NewSprite(148, 1, 16, 32)];//Brake
        Sprites = [
        texture.NewSprite(97, 3, 16, 30), //Walking1 Invincible
        texture.NewSprite(97, 194, 16, 30),
        texture.NewSprite(97, 257, 16, 30),
        texture.NewSprite(97, 320, 16, 30),
        texture.NewSprite(115, 2, 14, 31), //Walking2 Invincible
        texture.NewSprite(115, 193, 14, 31),
        texture.NewSprite(115, 256, 14, 31),
        texture.NewSprite(115, 319, 14, 31),
        texture.NewSprite(131, 1, 16, 32), //Walking3 Invincible
        texture.NewSprite(131, 192, 16, 32),
        texture.NewSprite(131, 255, 16, 32),
        texture.NewSprite(131, 318, 16, 32),
        texture.NewSprite(148, 1, 16, 32), //Brake Invincible
        texture.NewSprite(148, 192, 16, 32),
        texture.NewSprite(148, 255, 16, 32),
        texture.NewSprite(148, 318, 16, 32)];
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].Scale = scaleFactor;
        }
        CurrentSprite = Frames[Frame];
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime, 1);
        Mario.ChangeState(new LeftBigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime, 1);
    }
    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new RightJumpBigMarioState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new RightCrouchBigMarioState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.ChangeState(new RightSmallMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                //Mario.ChangeState(new RightFireMarioMoveState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Mushroom:
                //Nothing
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
        }
    }
    public void Idle()
    {
        Mario.ChangeState(new RightBigMarioIdleState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Update(GameTime gameTime, Vector2 Velocity)
    {
        timer += gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > timeFrame && Velocity.X < 0)
        {
            Frame = 3;
            timer = 0;
            Mario.MoveRight(gameTime, 8);
        }
        else if (timer > timeFrame)
        {
            if(Frame == 3)
            {
                Frame = 0;
            }
            Frame += nextFrame;
            if(Frame == 0 || Frame == Frames.Length - 2)
            {
                nextFrame *= -1;
            }
            timer = 0;
        }
        if (Mario.Invincible && Mario.StarTimeRemaining <= Mario.StarDuration)
        {
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            Mario.StarTimeRemaining += time;
            StarTimer += time;
            if (StarTimer > timeFrame / 3)
            {
                StarFrame++;
                if (StarFrame == Sprites.Length)
                {
                    StarFrame = 0;
                }
                StarTimer = 0;
            }
        }
        else
        {
            CurrentSprite = Frames[Frame];
        }
        CurrentSprite.Effect = Mario.FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }

}
