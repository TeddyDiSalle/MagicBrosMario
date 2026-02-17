using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class BigMarioMoveState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;

    private Sprite.Sprite CurrentSprite;

    private int Frame = 0;
    private int nextFrame = 1;
    private readonly double timeFrame;
    private double timer = 0;
    private readonly int scaleFactor;

    private readonly Sprite.Sprite[] Sprites;
    private int StarFrame = 0;
    private double StarTimer = 0;

    private bool Braking;
    public BigMarioMoveState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
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
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
        }
        CurrentSprite = Sprites[Frame];
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
            Mario.ChangeState(new SmallMarioMoveState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new FireMarioMoveState(Mario, texture, timeFrame, scaleFactor));
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
    private void IsBraking(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        bool BrakingRight = !Flipped && Velocity.X < 0;
        bool BrakingLeft = Flipped && Velocity.X > 0;
        if (BrakingRight || BrakingLeft)
        {
            Frame = 3;
            timer = 0;
            Braking = true;
            if (BrakingRight)
            {
                Mario.MoveRight(gameTime, 8);
            }
            else
            {
                Mario.MoveLeft(gameTime, 8);
            }
        }

        if(Braking && (!Flipped && Velocity.X >= 0 || Flipped && Velocity.X <= 0))
        {
            Braking = false;
        }
    }

    private void UpdateMovementAnimations(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        if (timer <= timeFrame) { return; }

        if (Frame == 3)
        {
            Frame = 0;
            nextFrame = 1;
        }
        Frame += nextFrame;
        if (Frame == 0 || Frame == Sprites.Length/4 - 2)
        {
            nextFrame *= -1;
        }
        IsBraking(gameTime, Velocity, Flipped);
        timer = 0;
    }

    private void UpdateStarAnimations(double time)
    {
        if (!Mario.Invincible) { return; }
        Mario.StarTimeRemaining += time;
        StarTimer += time;

        if (StarTimer <= timeFrame / 4) { return; }
        
        StarFrame++;
        if (Braking)
        {
            while (StarFrame + 4 < Sprites.Length)
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
        
        CurrentSprite = Sprites[StarFrame];
    }
    public void Update(GameTime gameTime, Vector2 Velocity, bool Flipped)
    {
        double time = gameTime.ElapsedGameTime.TotalSeconds;
        timer += time;
        
        UpdateMovementAnimations(gameTime, Velocity, Flipped);
        UpdateStarAnimations(time);

        CurrentSprite = (Mario.Invincible) ? Sprites[StarFrame] : Sprites[Frame*4];
        CurrentSprite.Flipped = Flipped;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        CurrentSprite.Position = new Point((int)Position.X, (int)Position.Y);
        CurrentSprite.Draw(spriteBatch);
    }
}
