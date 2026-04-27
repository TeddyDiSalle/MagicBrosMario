using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class BigMarioCrouchState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;

    public BigMarioCrouchState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        Sprites = [
            texture.NewSprite(136, 94, 16, 32),
            texture.NewAnimatedSprite(136, 94, 16, 32, 4, timeFrame/4)
        ];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
            Sprites[i].Depth = 0.6f;
        }
        CurrentSprite = Sprites[0];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 32 * scaleFactor);
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
            Mario.ChangeState(new SmallMarioIdleState(Mario));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
                Mario.ChangeState(new FireMarioCrouchState(Mario));
                break;
            case Power.Mushroom:
                //Nothing
                break;
            case Power.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Power.Cloud:
                Mario.ChangeState(new CloudMarioCrouchState(Mario));
                break;
        }
    }
    public Power GetCurrentMode()
    {
        return Power.Mushroom;
    }
    public void Idle()
    {
        //Nothing
    }
    public void StateChangePrep()
    {
        CurrentSprite.Visible = false;
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Drop();
        }
    }
    public void SetVisibility(bool visible)
    {
        CurrentSprite.Visible = visible;
    }
    private void SwitchSprite(int index)
    {
        CurrentSprite.Visible = false;
        CurrentSprite = Sprites[index];
        CurrentSprite.Visible = true;
    }
    public void Update(GameTime gameTime)
    {
        if (Mario.Invincible)
        {
            SwitchSprite(1);
            Mario.StarTimeRemaining += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            SwitchSprite(0);
        }
        CurrentSprite.Update(gameTime);
        CurrentSprite.HFlipped = Mario.Flipped;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        if (!Mario.IsCrouching)
        {
            Mario.ChangeState(new BigMarioIdleState(Mario));
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }
}
