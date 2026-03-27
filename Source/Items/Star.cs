using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
    public class Star : IItems
    {
        private const float X_SPEED = 150f;
        private const float GRAVITY_SPEED = 800f;
        private const float JUMP_FORCE = -350f; 
        private const float RISE_SPEED = 40f;

        private AnimatedSprite sprite;
        private float yVelocity = 0f;
        private int xDirection = 1;
        private bool isCollected = false;
        private bool hasRisen = false;
        private float riseAmount = 0f;
        private float riseTarget = 32f;

        public Point position { get => sprite.Position; private set => sprite.Position = value; }


        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(14 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public Star(SharedTexture texture, int positionX, int positionY)
        {
            sprite = texture.NewAnimatedSprite(5, 94, 14, 16, 4, 0.08f); 

            if (positionX <= 0)
            {
                positionX = 1;
            }
            if (positionY <= 0)
            {
                positionY = 1;
            }

            position = new Point(positionX, positionY);
            sprite.Scale = 2f;
            CollisionController.Instance.AddItem(this);
        }

        public void Update(GameTime gameTime)
        {
            if (isCollected) return;

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!hasRisen)
            {
                float riseStep = RISE_SPEED * time;
                position = new Point(position.X, position.Y - (int)riseStep);
                riseAmount += riseStep;

                if (riseAmount >= riseTarget)
                {
                    hasRisen = true;
                }
            }
            else
            {
                move(gameTime);
            }

            sprite.Update(gameTime);
        }

        private void move(GameTime gameTime)
        {
            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            yVelocity += GRAVITY_SPEED * time;

            int nextX = position.X + (int)(xDirection * X_SPEED * time);
            int nextY = position.Y + (int)(yVelocity * time);

            position = new Point(nextX, nextY);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCollected)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public void OnCollidePlayer(Player player, CollideDirection direction)
        {
            
            isCollected = true;
            CollisionController.Instance.RemoveItem(this);
            sprite.Drop();
            if (isCollected) return;

        }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction)
        {
            if (!hasRisen) return;

            if (direction == CollideDirection.Down)
            {
                position = new Point(position.X, block.CollisionBox.Top - CollisionBox.Height - 1);
                yVelocity = JUMP_FORCE;
            }
            else if (direction == CollideDirection.Top)
            {
                position = new Point(position.X, block.CollisionBox.Bottom + 1);
                yVelocity = 0;
            }
            else if (direction == CollideDirection.Left)
            {
                xDirection = 1;
                position = new Point(block.CollisionBox.Right + 1, position.Y);
            }
            else if (direction == CollideDirection.Right)
            {
                xDirection = -1;
                position = new Point(block.CollisionBox.Left - CollisionBox.Width - 1, position.Y);
            }
        }

        public bool getCollected()
        {
            return isCollected;
        }
    }
}