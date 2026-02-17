using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class BigMarioCrouchState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.Sprite CurrentSprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.Sprite[] Sprites;
    private int StarFrame = 0;
    private double StarTimer = 0;

    public BigMarioCrouchState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        Sprites = [texture.NewSprite(182, 1, 16, 32),
            texture.NewSprite(182, 192, 16, 32),
            texture.NewSprite(182, 255, 16, 32),
            texture.NewSprite(182, 318, 16, 32)];
        CurrentSprite = Sprites[0];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
    }
    public void Left(GameTime gameTime)
    {
        //Nothing
    }
    public void Right(GameTime gameTime)
    {
        //Nothing
    }
    public void Jump(GameTime gameTime)
    {
        //Nothing
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
            Mario.ChangeState(new SmallMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new FireMarioCrouchState(Mario, texture, timeFrame, scaleFactor));
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
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        if (Mario.Invincible)
        {
            double time = gameTime.ElapsedGameTime.TotalSeconds;
            Mario.StarTimeRemaining += time;
            StarTimer += time;
            if (StarTimer > timeFrame / 4)
            {
                StarFrame++;
                if (StarFrame == Sprites.Length)
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
        CurrentSprite.Flipped = Flipped;

        if (!Mario.IsCrouching)
        {
            Mario.ChangeState(new BigMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }

}
