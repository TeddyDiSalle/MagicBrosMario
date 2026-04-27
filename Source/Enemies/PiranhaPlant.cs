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
public class PiranhaPlant : IEnemy, ICollidable
{
    public bool AlwaysActive => false;
    private const float RISE_SPEED = 100f;
    private const float PAUSE_DURATION = 2f;
    private const int RISE_HEIGHT = 48;
    private const float SCALE = 2f;

    private Sprite.AnimatedSprite aliveSprite;
    private readonly int hiddenY;
    private readonly int visibleY;

    private enum PiranhaState { Rising, Lowering, Dead }

    private PiranhaState state;
    private float pauseTimer = 0f;
    private bool isAlive;

    public Point Position
    {
        get { return aliveSprite.Position; }
        private set { aliveSprite.Position = value; }
    }

    public bool GetIsAlive() => isAlive;

    public Rectangle CollisionBox
    {
        get
        {
            if (!isAlive) return Rectangle.Empty;
            int exposedHeight = hiddenY - Position.Y;
            if (exposedHeight <= 0) return Rectangle.Empty;
            return new Rectangle(Position.X + 4, Position.Y, aliveSprite.Size.X - 8, exposedHeight);
        }
    }

    public PiranhaPlant(SharedTexture EnemyTexture, int pipeY, int pipeX)
    {
        hiddenY = pipeY;
        visibleY = pipeY - RISE_HEIGHT;
        aliveSprite = EnemyTexture.NewAnimatedSprite(125, 180, 16, 23, 2, 0.2f);
        aliveSprite.Depth = 0.6f;
        aliveSprite.Scale = SCALE;
        Position = new Point(pipeX+15, hiddenY);
        isAlive = true;
        state = PiranhaState.Rising;
        pauseTimer = PAUSE_DURATION;
    }

    public void Update(GameTime gameTime)
    {
        if (!isAlive) return;

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (pauseTimer > 0)
        {
            pauseTimer -= deltaTime;
            return;
        }

        if (state == PiranhaState.Rising)
        {
            int newY = Position.Y - (int)(RISE_SPEED * deltaTime);
            if (newY <= visibleY)
            {
                Position = new Point(Position.X, visibleY);
                state = PiranhaState.Lowering;
                pauseTimer = PAUSE_DURATION;
            }
            else
                Position = new Point(Position.X, newY);
        }
        else if (state == PiranhaState.Lowering)
        {
            int newY = Position.Y + (int)(RISE_SPEED * deltaTime);
            if (newY >= hiddenY)
            {
                Position = new Point(Position.X, hiddenY);
                state = PiranhaState.Rising;
                pauseTimer = PAUSE_DURATION;
            }
            else
                Position = new Point(Position.X, newY);
        }

        aliveSprite.Visible = isAlive && Position.Y < hiddenY;
    }

    public void Kill()
    {
        isAlive = false;
        state = PiranhaState.Dead;
        aliveSprite.Drop();
        CollisionController.Instance.RemoveEnemy(this);
    }

    // Camera handles drawing
    public void Draw(SpriteBatch _spriteBatch) { }

    public void OnCollidePlayer(Player player, CollideDirection direction)
    {
        if (player.GetCurrentPower().Equals(Enums.Star))
        {
            Kill();
            HUD.Instance.SendEvent(new GameEvent
            {
                EventType = GameEventType.EnemyStomped,
                EventPosition = Position,
                Data = this
            });
            return;
        }
    }

    public void OnCollideItem(IItems item, CollideDirection direction)
    {
        if (item is MarioFireball)
        {
            Kill();
            HUD.Instance.SendEvent(new GameEvent
            {
                EventType = GameEventType.EnemyKilledByFireball,
                EventPosition = Position,
                Data = this
            });
        }
    }

    public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

    public void OnCollideBlock(IBlock block, CollideDirection direction) { }
}