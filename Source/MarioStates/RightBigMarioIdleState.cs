using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class RightBigMarioIdleState : IPlayerState
{
    private readonly Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite CurrentSprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;

    private Sprite.Sprite[] Sprites;
    private int StarFrame = 0;
    private double StarTimer = 0;

    public RightBigMarioIdleState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        
        Sprites = [texture.NewSprite(80, 1, 16, 32),
        texture.NewSprite(80, 192, 16, 32),
        texture.NewSprite(80, 255, 16, 32),
        texture.NewSprite(80, 318, 16, 32)];
        CurrentSprite = Sprites[0];
        for (int i = 1; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime, 1);
        Mario.ChangeState(new LeftBigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        //Mario.ChangeState(new BigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime, 1);
        Mario.ChangeState(new RightBigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        //Mario.ChangeState(new BigMarioMoveState(Mario, texture, timeFrame, scaleFactor));
    }

    public void Jump(GameTime gameTime)
    {
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new RightJumpBigMarioState(Mario, texture, timeFrame, scaleFactor));
        //Mario.ChangeState(new BigMarioJumpState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new RightCrouchBigMarioState(Mario, texture, timeFrame, scaleFactor));
        //Mario.ChangeState(new BigMarioCrouchState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        if (!Mario.Invincible)
        {
            Mario.ChangeState(new RightSmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
            //Mario.ChangeState(new SmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
               // Mario.ChangeState(new FireMarioIdleState(Mario, texture, timeFrame, scaleFactor));
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
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity)
    {
        if (Mario.Invincible && Mario.StarTimeRemaining <= Mario.StarDuration)
        {
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            Mario.StarTimeRemaining += time;
            StarTimer += time;
            if(StarTimer > timeFrame / 4)
            {
                StarFrame++;
                if(StarFrame == Sprites.Length)
                {
                    StarFrame = 0;
                }
                StarTimer = 0;
            }
            CurrentSprite = Sprites[StarFrame];
        }
        else
        {
            CurrentSprite = Sprites[0];
        }
        CurrentSprite.Effect = Mario.FacingRight ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }

}
