using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class FireMarioJumpState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.Sprite CurrentSprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.Sprite[] Sprites;
    private int StarFrame = 0;
    private double StarTimer = 0;

    private bool IsAttacking = false;
    public FireMarioJumpState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [texture.NewSprite(165, 129, 16, 32),
            texture.NewSprite(165, 192, 16, 32),
            texture.NewSprite(165, 255, 16, 32),
            texture.NewSprite(165, 318, 16, 32),
            texture.NewSprite(420, 129, 16, 32),
            texture.NewSprite(420, 192, 16, 32),
            texture.NewSprite(420, 255, 16, 32),
            texture.NewSprite(420, 318, 16, 32)];
        CurrentSprite = Sprites[0];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
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
        IsAttacking = true;
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
    public void Idle()
    {
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        if (Mario.Invincible)
        {
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            Mario.StarTimeRemaining += time;
            StarTimer += time;
            if (StarTimer > timeFrame / 3)
            {
                StarFrame++;
                if (StarFrame == Sprites.Length-4)
                {
                    StarFrame = 0;
                }
                StarTimer = 0;
            }
            CurrentSprite = (IsAttacking) ? Sprites[StarFrame + 4] : Sprites[StarFrame];
        }
        else
        {
            CurrentSprite = (IsAttacking) ? Sprites[4] : Sprites[0];
        }
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
