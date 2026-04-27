using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.HUDAndScoring;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source;
//Roshan Ramamurthy
public class Goomba : IEnemy, ICollidable
{
    public bool AlwaysActive => false;
    private const int VELOCITY = 100;
    private const float SCALE = 2f;
    private const float GRAVITY = 0.35f;
    private Sprite.ISprite[] sprites;
    private bool movingRight = true;
    private bool isAlive = true;
    private float velocityY = 0f;
    private GoombaCollisionHandler collisionHandler;

    public Point Position
    {
        get => sprites[0].Position;
        set
        {
            foreach (var sprite in sprites)
                sprite.Position = value;
        }
    }

    public Rectangle CollisionBox
    {
        get
        {
            var sprite = isAlive ? sprites[0] : sprites[1];
            return new Rectangle(Position.X, Position.Y, sprite.Size.X, sprite.Size.Y);
        }
    }

    public Goomba(SharedTexture EnemyTexture, int y, int x)
    {
        sprites = [EnemyTexture.NewAnimatedSprite(296, 187, 16, 16, 2, 0.2f),
                    EnemyTexture.NewSprite(276, 187, 16, 16)];
        foreach (var sprite in sprites)
        {
            sprite.Scale = SCALE;
            sprite.Visible = false;
        }
        Position = new Point(x, y);
        collisionHandler = new GoombaCollisionHandler(this);
    }

    public bool GetIsAlive() => isAlive;

    public void Update(GameTime gametime)
    {
        if (isAlive)
            Walking(gametime);

        velocityY += GRAVITY;
        Position = new Point(Position.X, Position.Y + (int)velocityY);

        sprites[0].Visible = isAlive;
        sprites[1].Visible = !isAlive;
    }

    public void Walking(GameTime gameTime)
    {
        var sec = (double)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0;
        var dx = (int)(sec * VELOCITY);

        if (movingRight)
            Position = new Point(Position.X + dx, Position.Y);
        else
            Position = new Point(Position.X - dx, Position.Y);
    }

    public void Kill()
    {
        isAlive = false;
        foreach (var sprite in sprites)
            sprite.Drop();

        CollisionController.Instance.RemoveEnemy(this);
    }

    public void UnCollide(Rectangle intersect, CollideDirection direction)
    {
        if (direction == CollideDirection.Left)
        {
            Position = new Point(Position.X + intersect.Width, Position.Y);
            movingRight = true;
        }
        else if (direction == CollideDirection.Right)
        {
            Position = new Point(Position.X - intersect.Width, Position.Y);
            movingRight = false;
        }
    }

    public void ResetGravity() => velocityY = 0;

    // Camera handles drawing
    public void Draw(SpriteBatch _spriteBatch) { }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) =>
        collisionHandler.OnCollideEnemy(enemy, direction);

    public void OnCollideBlock(IBlock block, CollideDirection direction) =>
        collisionHandler.OnCollideBlock(block, direction);

    public void OnCollidePlayer(Player player, CollideDirection direction) =>
        collisionHandler.OnCollidePlayer(player, direction);

    public void OnCollideItem(IItems item, CollideDirection direction) =>
        collisionHandler.OnCollideItem(item, direction);
}