using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Transactions;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class BigMarioMoveState : IPlayerState
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

    public BigMarioMoveState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
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
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
        CurrentSprite = Frames[Frame];
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime, 1);
        //Mario.ChangeState(new LeftBigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime, 1);
    }
    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new BigMarioJumpState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new BigMarioCrouchState(Mario, texture, timeFrame, scaleFactor));
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
                StarFrame = Frame * 4;
                break;
        }
    }
    public void Idle()
    {
        Mario.ChangeState(new BigMarioIdleState(Mario, texture, timeFrame, scaleFactor));
    }
    private bool IsBraking(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        bool braking = false;
        if (!Flipped & timer > timeFrame && Velocity.X < 0)
        {
            Frame = 3;
            timer = 0;
            Mario.MoveRight(gameTime, 8);
            braking = true;
        }
        else if (Flipped & timer > timeFrame && Velocity.X > 0)
        {
            Frame = 3;
            timer = 0;
            Mario.MoveLeft(gameTime, 8);
            braking = true;
        }
        return braking;
    }
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        timer += time;
        bool Braking = IsBraking(gameTime, Velocity, Flipped);
        if (timer > timeFrame)
        {
            if (Frame == 3)
            {
                Frame = 0;
                nextFrame = 1;
            }
            Frame += nextFrame;
            if (Frame == 0 || Frame == Frames.Length - 2)
            {
                nextFrame *= -1;
            }
            timer = 0;
        }
        if (Mario.Invincible)
        {
            Mario.StarTimeRemaining += time;
            StarTimer += time;
            
            if (StarTimer > timeFrame / 4)
            {
                StarFrame++;

                if(Braking)
                {
                    while(StarFrame + 4 < Sprites.Length)
                    {
                        StarFrame += 4;
                    }
                    if (StarFrame >= Sprites.Length)
                    {
                        StarFrame = 12;
                    }
                }
                else
                {
                    if (StarFrame >= Sprites.Length - 4)
                    {
                        StarFrame = 0;
                    }
                }
                    StarTimer = 0;
            }
            CurrentSprite = Sprites[StarFrame];
            if (Braking) { Debug.Write("Braking: " + StarFrame + " "); }
            else { Debug.Write("NOT Braking: " + StarFrame + " "); }
        }
        else
        {
            CurrentSprite = Frames[Frame];
        }
        
        CurrentSprite.Flipped = Flipped;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }

}
