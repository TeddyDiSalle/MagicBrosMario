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
    private Sprite.ISprite currentSprite;

    private int Frame = 0;
    private int nextFrame = -1;
    private readonly double timeFrame;
    private double timer = 0;
    private readonly int scaleFactor;

    public RightBigMarioMoveState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Frames = [texture.NewSprite(296, 3, 16, 30), //Walking1
            texture.NewSprite(315, 2, 14, 31), //Walking2
            texture.NewSprite(331, 1, 16, 32), //Walking3
            texture.NewSprite(350, 1, 16, 32)];//Brake
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].Scale = scaleFactor;
        }
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
        //Nothing
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        Mario.ChangeState(new RightSmallMarioMoveState(Mario, texture, timeFrame, scaleFactor));
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new RightFireMarioMoveState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Mushroom:
                //Nothing
                break;
            case Power.Star:
                //RainbowState?
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
            if (Frame == 3)
            {
                Frame = 0;
            }
            else if (Frame == 0 || Frame == 2)
            {
                nextFrame = -nextFrame;
                Frame += nextFrame;
            }
            else
            {
                Frame += nextFrame;
            }
            timer = 0;
        }
        currentSprite = Frames[Frame];
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        currentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        currentSprite.Draw(spriteBatch);
    }

}
