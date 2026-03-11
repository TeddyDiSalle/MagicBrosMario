using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class SmallMarioJumpState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;

    private const int maxJumpCalls = 5;
    private int JumpCalls = 0;


    public SmallMarioJumpState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [texture.NewSprite(69, 2, 16, 16),
            texture.NewAnimatedSprite(69, 2, 16, 16, 4, timeFrame/4)];
        CurrentSprite = Sprites[0];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 16 * scaleFactor);
        Mario.IsJumping = true;
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
        if(Mario.IsJumping && JumpCalls < maxJumpCalls)
        {
            Mario.MoveUp(gameTime, 0.3f);
            JumpCalls++;
        }
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
                Mario.ChangeState(new FireMarioJumpState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Mushroom:
                Mario.ChangeState(new BigMarioJumpState(Mario, texture, timeFrame, scaleFactor));
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
        //Nothing
    }
    public void Update(GameTime gameTime)
    {
        if (Mario.Invincible)
        {
            CurrentSprite = Sprites[1];
            Mario.StarTimeRemaining += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            CurrentSprite = Sprites[0];
        }
        CurrentSprite.Flipped = Mario.Flipped;
        CurrentSprite.Update(gameTime);
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        if (Mario.Velocity.Y == 0)
        {
            Mario.ChangeState(new SmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }

    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
