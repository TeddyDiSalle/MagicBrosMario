using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class FireMarioJumpState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;

    private bool IsAttacking = false;
    public FireMarioJumpState(Player Mario, Sprite.SharedTexture texture, float timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [
            texture.NewSprite(69, 164, 16, 32),
            texture.NewAnimatedSprite(69, 164, 16, 32, 4, timeFrame/4),
            texture.NewSprite(136, 269, 16, 32),
            texture.NewAnimatedSprite(136, 269, 16, 32, 4, timeFrame/4)
            ];
        CurrentSprite = Sprites[0];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
        Rectangle currentHitBox = Mario.collision.CollisionBox;
        Mario.collision.CollisionBox = new Rectangle(currentHitBox.X, currentHitBox.Y, 16, 32);
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
        //Nothing
    }
    public void Crouch(GameTime gameTime)
    {
        Mario.ChangeState(new FireMarioCrouchState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Attack()
    {
        if (!IsAttacking)
        {
            IsAttacking = true;
            Mario.CreateFireball();
        }
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
                //Nothing
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
    public Power getCurrentPower()
    {
        return Power.FireFlower;
    }
    public void Idle()
    {
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        if (Mario.Invincible)
        {
            Mario.StarTimeRemaining += gameTime.ElapsedGameTime.TotalSeconds;
            CurrentSprite = (IsAttacking) ? Sprites[3] : Sprites[1];
        }
        else
        {
            CurrentSprite = (IsAttacking) ? Sprites[2] : Sprites[0];
        }
        CurrentSprite.Update(gameTime);
        CurrentSprite.Flipped = Flipped;

        if (Velocity.Y == 0)
        {
            Mario.ChangeState(new FireMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
        IsAttacking = false;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }

}
