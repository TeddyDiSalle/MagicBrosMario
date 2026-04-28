using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class SmallMarioIdleState : IPlayerState
{
    private readonly Player Mario;
    private readonly Sprite.SharedTexture texture;
    private Sprite.ISprite CurrentSprite;
    private readonly float timeFrame;
    private readonly int scaleFactor;

    private readonly Sprite.ISprite[] Sprites;

    public SmallMarioIdleState(Player Mario)
    {
        this.Mario = Mario;
        this.texture = Mario.Texture;
        this.timeFrame = Mario.TimeFrame;
        this.scaleFactor = Mario.ScaleFactor;
        Sprites = [texture.NewSprite(2, 2, 16, 16),
            texture.NewAnimatedSprite(2, 2, 16, 16, 4, timeFrame/4)];
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprites[i].Scale = scaleFactor;
            Sprites[i].Visible = false;
            Sprites[i].Depth = 0.6f;
        }
        CurrentSprite = Sprites[(int)IdleEnums.regularIdle];
        CurrentSprite.Visible = true;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);
        Mario.CollisionBox = new Rectangle(Mario.CollisionBox.X, Mario.CollisionBox.Y, 16 * scaleFactor, 16 * scaleFactor);
    }
    public void Left(GameTime gameTime)
    {
        Mario.MoveLeft(gameTime);
        Mario.ChangeState(new SmallMarioMoveState(Mario));
    }
    public void Right(GameTime gameTime)
    {
        Mario.MoveRight(gameTime);
        Mario.ChangeState(new SmallMarioMoveState(Mario));
    }

    public void Jump(GameTime gameTime)
    {
        if (!Mario.IsGrounded && !Mario.WasGrounded) { return; }
        Mario.MoveUp(gameTime);
        Mario.ChangeState(new SmallMarioJumpState(Mario));
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
            Mario.KillMario();
        }
    }
    public void PowerUp(Enums power)
    {
        switch (power)
        {
            case Enums.FireFlower:
                Mario.ChangeState(new FireMarioIdleState(Mario));
                break;
            case Enums.Mushroom:
                Mario.ChangeState(new BigMarioIdleState(Mario));
                break;
            case Enums.Star:
                Mario.Invincible = true;
                Mario.StarTimeRemaining = 0;
                break;
            case Enums.Cloud:
                Mario.ChangeState(new CloudMarioIdleState(Mario));
                break;
        }
    }
    public Enums GetCurrentMode()
    {
        return Enums.None;
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
            SwitchSprite((int)IdleEnums.starIdle);
            Mario.StarTimeRemaining += gameTime.ElapsedGameTime.TotalSeconds;
        }
        else
        {
            SwitchSprite((int)IdleEnums.regularIdle);
        }
        CurrentSprite.Update(gameTime);
        CurrentSprite.HFlipped = Mario.Flipped;
        CurrentSprite.Position = new Point((int)Mario.Position.X, (int)Mario.Position.Y);

    }
    public void Draw(SpriteBatch spriteBatch)
    {
        CurrentSprite.Draw(spriteBatch);
    }

}
