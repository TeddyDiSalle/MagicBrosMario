using MagicBrosMario.Source.Sound;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class SmallMarioSlideState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;
    private readonly Sprite.ISprite[] Sprites;
    private double LandingTimer = 0;

    public SmallMarioSlideState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        Sprites = [texture.NewSprite(156, 2, 16, 16),
            texture.NewSprite(173, 2, 16, 16)];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
        }
        CurrentSprite = Sprites[0];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 16 * scaleFactor);
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
        //nothing
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
        //nothing
    }
    public void PowerUp(Power power)
    {
        //Nothing
    }
    public Power GetCurrentMode()
    {
        return Power.None;
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
        if (Mario.Position.Y >= Mario.FlagPoleBottomY)
        {
            Mario.SetPositon(new Vector2(Mario.Position.X, Mario.FlagPoleBottomY));
            Mario.SetVelocity(Vector2.Zero);
            SwitchSprite(1); // landing/holding sprite
                             // wait a moment then transition to walk state
            LandingTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (LandingTimer >= 0.5)
            {
                Mario.ChangeState(new SmallMarioMoveState(Mario));
                Mario.EndPhase = Player.EndLevelPhase.Walking;
            }
        }
        else
        {
            SwitchSprite(0); // sliding sprite
            Mario.SetVelocity(new Vector2(0, 3));
        }

        CurrentSprite.Update(gameTime);
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        CurrentSprite.HFlipped = Mario.Flipped;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
