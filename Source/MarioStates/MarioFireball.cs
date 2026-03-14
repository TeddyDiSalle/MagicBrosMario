using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;


namespace MagicBrosMario.Source;

public class MarioFireball: Items.IItems, Collision.ICollidable
{
    private const int VELOCITY = 4;
    private const float LIFETIME = 3.0f;

    private ISprite CurrentSprite;
    private readonly ISprite[] Sprites;
    private readonly bool movingRight;
    private float lifetimeRemaining;
    private int ScaleFactor;
    private double contactTimer = 0;
    private double contantCD = 0.01;

    private Vector2 position;
    private Vector2 velocity;
    private const float Gravity = 0.35f;

    public Rectangle CollisionBox { get; private set; }

    public MarioFireball(Sprite.AnimatedSprite fireball, Sprite.Sprite explosion, Vector2 position, bool movingRight, int ScaleFactor)
    {
        this.movingRight = movingRight;
        this.lifetimeRemaining = LIFETIME;
        explosion.Visible = false;
        Sprites = [fireball, explosion];
        CurrentSprite = Sprites[0];
        CurrentSprite.Visible = true;
        this.position = position;
        this.ScaleFactor = ScaleFactor;
        CurrentSprite.Position = new Point((int)position.X, (int)position.Y);
        CollisionBox = new Rectangle((int)position.X, (int)position.Y, 8 * ScaleFactor, 8 *ScaleFactor);
        CollisionController.Instance.AddItem(this);
        if (movingRight)
        {
            velocity = new Vector2(VELOCITY, 0);
        }
        else
        {
            velocity = new Vector2(-VELOCITY, 0);
        }

    }


    public void Contact()
    {
        SwitchSprite(1);
        lifetimeRemaining = lifetimeRemaining/100;
    }
    public bool IsExpired()
    {
        CollisionController.Instance.RemoveItem(this);
        if (lifetimeRemaining <= 0)
        {
            Sprites[0].Drop();
            Sprites[1].Drop();
        }
        return lifetimeRemaining <=0;
    }


    private void SwitchSprite(int index)
    {
        CurrentSprite.Visible = false;
        CurrentSprite = Sprites[index];
        CurrentSprite.Visible = true;
    }

    public bool getCollected()
    {
        return IsExpired();
    }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        //Nothing
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        //Nothing
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction)
    {
        Contact();
    }

    public void OnCollideBlock(IBlock block, CollideDirection direction)
    {
        if(contactTimer< contantCD) { return; }
        contactTimer = 0;
        UnCollide(Rectangle.Intersect(CollisionBox, block.CollisionBox), direction);
        if(direction != CollideDirection.Down) {
            Contact();
        }
     
            
    }
    public void UnCollide(Rectangle intersect, CollideDirection direction)
    {
        switch (direction)
        {
            case CollideDirection.Top:
                position += new Vector2(0, intersect.Height);
                velocity = new Vector2(0, 0);
                break;

            case CollideDirection.Down:
                float newY = position.Y - intersect.Height;
                newY = (float)Math.Floor(newY);
                position = new Vector2(position.X, newY);
                velocity = new Vector2(velocity.X, -5);
                break;

            case CollideDirection.Left:
                position += new Vector2(intersect.Width, 0);
                velocity = new Vector2(0, 0);
                break;

            case CollideDirection.Right:
                position -= new Vector2(intersect.Width, 0);
                velocity = new Vector2(0, 0);
                break;
        }
    }
    public void Update(GameTime gameTime)
    {
     
        float time = (float)gameTime.ElapsedGameTime.TotalSeconds;
        lifetimeRemaining -= time;
        float distanceMoved = (float)(time * 1 * VELOCITY);
        contactTimer += time;
        velocity += new Vector2(0, Gravity);
        position += velocity;

        CurrentSprite.Position = new Point((int)position.X, (int)position.Y);
        CollisionBox = new Rectangle((int)position.X, (int)position.Y, 8 * ScaleFactor, 8 * ScaleFactor);
        CurrentSprite.Update(gameTime);
    }
    public void Draw(SpriteBatch _spriteBatch)
    {
        CurrentSprite.Draw(_spriteBatch);
    }
}