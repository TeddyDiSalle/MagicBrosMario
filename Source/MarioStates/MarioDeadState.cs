using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class MarioDeadState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private readonly Sprite.Sprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;
    public MarioDeadState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        CurrentSprite = texture.NewSprite(136, 2, 16, 16);
        CurrentSprite.Scale = scaleFactor;
        CurrentSprite.Visible = true;
        Mario.SetVelocity(new Vector2(0, -6));
        Mario.IsAlive = false;
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 16 * scaleFactor);
        SoundController.StopMusic();
        SoundController.PlaySound(SoundType.MarioDie, 1.0f);
        HUD.Instance.SendEvent(new GameEvent { EventType = GameEventType.Death });
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
        //Nothing
    }
    public void PowerUp(Power power)
    {
        //Nothing
    }
    public Power GetCurrentPower()
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
        CurrentSprite.Drop();
    }
    public void SetVisibility(bool visible)
    {
        CurrentSprite.Visible = true;
    }

    public void Update(GameTime gameTime)
    {
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        if (CurrentSprite.Position.Y > Camera.Instance.Position.Y + Camera.Instance.WindowSize.Y)
        {
            Mario.SetVelocity(Vector2.Zero);
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
