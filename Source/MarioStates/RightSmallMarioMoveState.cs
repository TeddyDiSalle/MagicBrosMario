using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightSmallMarioMoveState : IPlayerState
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

    private bool StarMode = false;
    private float StarDuration = 10;
    private float StarTimeRemaining = 0;

    public RightSmallMarioMoveState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Frames = [texture.NewSprite(292, 45, 12, 15), texture.NewSprite(307, 44, 11, 16), texture.NewSprite(321, 44, 15, 16), texture.NewSprite(339, 44, 13, 16)];
        for (int i = 0; i < Frames.Length; i++)
        {
            Frames[i].Scale = scaleFactor;
        }
        currentSprite = Frames[Frame];
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime, 1);
        //Mario.ChangeState(new LeftSmallMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime, 1);
    }
    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new RightJumpSmallMarioState(Mario, texture, timeFrame, scaleFactor));
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
        if (!StarMode)
        {
            Mario.ChangeState(new DeadMarioState(Mario, texture, timeFrame, scaleFactor));
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
                Mario.ChangeState(new BigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Star:
                StarMode = true;
                StarTimeRemaining = 0;
                break;
        }
    }
    public void Idle()
    {
        Mario.ChangeState(new RightSmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        if (StarMode && StarTimeRemaining <= StarDuration)
        {
            float time = gameTime.ElapsedGameTime.Milliseconds;
            StarTimeRemaining += time / 1000.0f;
            currentSprite.Color = Mario.rainbow[(int)StarTimeRemaining % Mario.rainbow.Length];
        }
        else
        {
            StarMode = false;
            currentSprite.Color = Color.White;
        }

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
