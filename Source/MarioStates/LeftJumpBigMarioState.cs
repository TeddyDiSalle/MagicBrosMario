using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class LeftJumpBigMarioState : IPlayerState
{
    private readonly Player Mario;
    private Sprite.SharedTexture texture;
    private Sprite.Sprite sprite;
    private readonly double timeFrame;
    private readonly int scaleFactor;

    private bool StarMode = false;
    private float StarDuration = 10;
    private float StarTimeRemaining = 0;
    public LeftJumpBigMarioState(Player Mario, Sprite.SharedTexture texture, double timeFrame, int scaleFactor)
    {
        this.Mario = Mario;
        this.texture = texture;
        this.timeFrame = timeFrame;
        this.scaleFactor = scaleFactor;
        sprite = texture.NewSprite(128, 2, 16, 31);
        sprite.Scale = scaleFactor;
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
        Mario.ChangeState(new LeftCrouchBigMarioState(Mario, texture, timeFrame, scaleFactor));
    }
    public void Attack()
    {
        //Nothing
    }
    public void TakeDamage()
    {
        if (!StarMode)
        {
            Mario.ChangeState(new LeftJumpSmallMarioState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void PowerUp(Power power)
    {
        switch (power)
        {
            case Power.FireFlower:
              //  Mario.ChangeState(new LeftJumpFireMarioState(Mario, texture, timeFrame, scaleFactor));
                break;
            case Power.Mushroom:
                //Nothing
                break;
            case Power.Star:
                StarMode = true;
                StarTimeRemaining = 0;
                break;
        }
    }
    public void Idle()
    {
        //Nothing
    }
    public void Update(GameTime gameTime, Vector2 Velocity)
    {
        if (StarMode && StarTimeRemaining <= StarDuration)
        {
            float time = gameTime.ElapsedGameTime.Milliseconds;
            StarTimeRemaining += time / 1000.0f;
            sprite.Color = Mario.rainbow[(int)StarTimeRemaining % Mario.rainbow.Length];
        }
        else
        {
            StarMode = false;
            sprite.Color = Color.White;
        }
        if (Velocity.Y == 0)
        {
            Mario.ChangeState(new LeftBigMarioIdleState(Mario, texture, timeFrame, scaleFactor));
        }
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 Position)
    {
        sprite.Position = new Point((int)Position.X, (int)Position.Y);
        sprite.Draw(spriteBatch);
    }

}
