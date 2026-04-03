using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class Fireball : ICollidable
{
    private const int VELOCITY = 150;
    private const float LIFETIME = 3.0f;

    private Sprite.AnimatedSprite spriteRight;
    private Sprite.AnimatedSprite spriteLeft;
    private bool movingRight;
    private float lifetimeRemaining;
    private bool isActive = true;

    public Point Position => movingRight ? spriteRight.Position : spriteLeft.Position;

    public Rectangle CollisionBox
    {
        get
        {
            if (!isActive) return Rectangle.Empty;
            var currentSprite = movingRight ? spriteRight : spriteLeft;
            return new Rectangle(Position.X, Position.Y, currentSprite.Size.X, currentSprite.Size.Y);
        }
    }

    public Fireball(Sprite.AnimatedSprite fireballSpriteRight, Sprite.AnimatedSprite fireballSpriteLeft,
        int startX, int startY, bool movingRight)
    {
        spriteRight = fireballSpriteRight;
        spriteLeft = fireballSpriteLeft;
        this.movingRight = movingRight;
        lifetimeRemaining = LIFETIME;
        spriteRight.Scale = 3f;
        spriteLeft.Scale = 3f;
        spriteRight.Visible = false;
        spriteLeft.Visible = false;
        spriteRight.Position = new Point(startX, startY);
        spriteLeft.Position = new Point(startX, startY);
    }

    public void Update(GameTime gameTime)
    {
        if (!isActive) return;

        lifetimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (lifetimeRemaining <= 0)
        {
            Deactivate();
            return;
        }

        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        var dx = (int)(sec * VELOCITY);
        var newPos = movingRight
            ? new Point(spriteRight.Position.X + dx, spriteRight.Position.Y)
            : new Point(spriteLeft.Position.X - dx, spriteLeft.Position.Y);
        spriteRight.Position = newPos;
        spriteLeft.Position = newPos;

        spriteRight.Visible = movingRight;
        spriteLeft.Visible = !movingRight;
    }

    private void Deactivate()
    {
        isActive = false;
        spriteRight.Drop();
        spriteLeft.Drop();
    }

    public bool IsExpired() => !isActive || lifetimeRemaining <= 0;

    // Camera handles drawing
    public void Draw(SpriteBatch _spriteBatch) { }

    public void OnCollidePlayer(Player player, CollideDirection direction) => Deactivate();

    public void OnCollideItem(IItems item, CollideDirection direction) { }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

    public void OnCollideBlock(IBlock block, CollideDirection direction) => Deactivate();
}