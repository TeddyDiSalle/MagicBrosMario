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
        private AnimatedSprite sprite;
        private Vector2 position;
        private float speed = 150f;
        private float gravitySpeed = 800f;
        private float yVelocity = 0f;
        private float jumpForce = -300f;
        private int xDirection = 1;
        private int xLimit;
        private bool isCollected = false;
        private bool hasRisen = false;
        private bool isOnBlock = false;
        private float riseAmount = 0f;
        private float riseTarget = 48f;


        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, (int)(14 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public Star(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
        {
            sprite = texture.NewAnimatedSprite(5, 94, 14, 16, 4, 0.08f);
            xLimit = screenWidth;

            if (positionX <= 0)
            {
                positionX = 1;
            }
            if (positionY <= 0)
            {
                positionY = 1;
            }

            position = new Vector2(positionX, positionY);
            sprite.Scale = 3f;
            CollisionController.Instance.AddItem(this);
        }

        public void Update(GameTime gameTime)
        {
            if (!isCollected)
            {
                float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!hasRisen)
                {
                    float riseStep = 50f * time;

                    position.Y -= riseStep;
                    riseAmount += riseStep;

                    if (riseAmount >= riseTarget)
                    {
                        hasRisen = true;
                    }
                }
                else
                {
                    yVelocity += gravitySpeed * time;
                    position.Y += yVelocity * time;
                    position.X += xDirection * speed * time;

                    if (position.X <= 0)
                    {
                        position.X = 1;
                        xDirection = 1;
                    }
                    else if (position.X + CollisionBox.Width >= xLimit)
                    {
                        position.X = xLimit - CollisionBox.Width - 1;
                        xDirection = -1;
                    }

                    sprite.Position = position.ToPoint();
                }
                sprite.Update(gameTime);
            }
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
            if (direction == CollideDirection.Down)
            {
                // this is when it is on ground
                isOnBlock = true;
                position.Y = block.CollisionBox.Top - CollisionBox.Height;
                yVelocity = jumpForce;
            }
            else if (direction == CollideDirection.Top)
            {
                position.Y = block.CollisionBox.Bottom + 1;
                yVelocity = 0;
            }
            else if (direction == CollideDirection.Left)
            {
                xDirection = 1;
                position.X = block.CollisionBox.Right + 1;
            }
            else if (direction == CollideDirection.Right)
            {
                xDirection = -1;
                position.X = block.CollisionBox.Left - CollisionBox.Width - 1;
            }
        }

        public bool getCollected()
        {
            return isCollected;
        }
    }
}