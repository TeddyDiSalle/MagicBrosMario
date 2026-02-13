using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightSmallMarioMoveState : IPlayerState
{
    private Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite sprite;
    private readonly Point[] XYOffset = [new Point(292, 45), new Point(307, 44), new Point(321, 44), new Point(339, 44)];
    private readonly Point[] WidthHeight = [new Point(12, 15), new Point(11, 16), new Point(15, 16), new Point(13, 16)];
    private int Frame = 0;
    private int nextFrame = -1;
    private double timeFrame = 0.15;
    private double timer = 0;

    public RightSmallMarioMoveState(Player Mario, Sprite.SharedTexture texture)
    {
        this.Mario = Mario;
        this.texture = texture;
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
        Mario.ChangeState(new LeftSmallMarioMoveState(Mario, texture));
        
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
    }
    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new RightJumpSmallMarioState(Mario, texture));
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
        Mario.ChangeState(new DeadMarioState(Mario, texture));
    }
    public void PowerUp(Power power)
    {
        //switch (power)
        //{
        //    case Power.FireFlower:
        //        Mario.ChangeState(new RightFireMarioMoveState(Mario, texture));
        //        break;
        //    case Power.Mushroom:
        //        Mario.ChangeState(new RightBigMarioMoveState(Mario, texture));
        //        break;
        //    case Power.Star:
        //        //RainbowState?
        //        break;
        //}
    }
    public void Idle()
    {
        Mario.ChangeState(new RightSmallMarioIdleState(Mario, texture));
    }
    public void Update(GameTime gameTime, Vector2 Velocity)
    {
        timer += gameTime.ElapsedGameTime.TotalSeconds;
        if (timer > timeFrame && Velocity.X < 0)
        {
            Frame = 3;
            timer = 0;
            Mario.BreakRight(gameTime);
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
        sprite = new Sprite.Sprite(texture, XYOffset[Frame].X, XYOffset[Frame].Y, WidthHeight[Frame].X, WidthHeight[Frame].Y);
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        sprite.Position = new Point((int)Position.X, (int)Position.Y);
        sprite.Draw(spriteBatch);
    }

}
