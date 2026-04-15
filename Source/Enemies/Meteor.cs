// Meteor.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;

namespace MagicBrosMario.Source;

public class Meteor : IEnemy, ICollidable
{
    private const float GRAVITY = 0.5f;
    private const float LIFETIME = 5.0f;

    private ISprite sprite;
    private float velocityY = 0f;
    private float lifetimeRemaining;
    private bool isActive = true;

    public Point Position
    {
        get => sprite.Position;
        private set => sprite.Position = value;
    }

    public Rectangle CollisionBox
    {
        get
        {
            if (!isActive) return Rectangle.Empty;
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public Meteor(ISprite meteorSprite, int x, int y)
    {
        sprite = meteorSprite;
        lifetimeRemaining = LIFETIME;
        Position = new Point(x, y);
        sprite.Visible = true;
    }

    public bool GetIsAlive() => isActive;

    public void Update(GameTime gameTime)
    {
        if (!isActive) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        lifetimeRemaining -= deltaTime;

        if (lifetimeRemaining <= 0)
        {
            Deactivate();
            return;
        }

        velocityY += GRAVITY;
        Position = new Point(Position.X, Position.Y + (int)velocityY);

        sprite.Visible = true;
    }

    public bool IsExpired() => !isActive;

    public void Kill() => Deactivate();

    private void Deactivate()
    {
        isActive = false;
        sprite.Visible = false;
        sprite.Drop();
        CollisionController.Instance.RemoveEnemy(this);
    }

    // Camera handles drawing
    public void Draw(SpriteBatch _spriteBatch) { }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        Deactivate();
    }

    public void OnCollideItem(IItems item, CollideDirection direction) { }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        Deactivate();
    }
}