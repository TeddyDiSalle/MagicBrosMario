using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class BigMarioJumpState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;


    public BigMarioJumpState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [texture.NewSprite(69, 59, 16, 32),
            texture.NewAnimatedSprite(69, 59, 16, 32, 4, timeFrame/4)];
        CurrentSprite = Sprites[0];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
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
        //Nothing
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
            Mario.ChangeState(new SmallMarioJumpState(Mario, texture, timeFrame, scaleFactor));
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
                //Nothing
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
        }
    }
    public Power GetCurrentPower()
    {
        return Power.Mushroom;
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
        CurrentSprite.Update(gameTime);
        CurrentSprite.Flipped = Mario.Flipped;

        if (Mario.Velocity.Y == 0)
        {
            Mario.ChangeState(new BigMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }
}
