// Made by Brian
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MagicBrosMario.Source.Items
{
    internal class OneUp : IItems
    {

        private Sprite.Sprite sprite;
        private Point position;
        private float xSpeed = 100f;
        private float gravitySpeed = 220f;
<<<<<<< HEAD
        private int xDirection = 1;
=======
        private int direction = 1;
>>>>>>> main
        private int xLimit;
        private int yLimit;
        private bool isCollected = false;
        private bool hasRisen = false;
        private bool isOnBlock = false;
        private float riseAmount = 0f;
        private float riseTarget = 16f;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public OneUp(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
        {

            sprite = texture.NewSprite(214, 34, 16, 16);
            yLimit = screenHeight;
            xLimit = screenWidth;

            if (positionX <= 0)
            {
                positionX = 1;
            }
            if (positionY <= 0)
            {
                positionY = 1;
            }

            position = new Point(positionX, positionY);
            sprite.Scale = 3f;

        }
        public void Update(GameTime gameTime)
        {
            if (!isCollected)
            {
                float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!hasRisen)
                {
                    float riseStep = gravitySpeed * time;

                    position.Y -= (int)riseStep;
                    riseAmount += riseStep;

                    if (riseAmount >= riseTarget)
                    {
                        hasRisen = true;
                    }
                }
                else
                {
                    if (!isOnBlock)
                    {
                        position.Y += (int)(gravitySpeed * time);
                    }
<<<<<<< HEAD
                    position.X += (int)(xDirection * xSpeed * time);
                    if (position.X <= 0 || position.X + sprite.Size.X >= xLimit)
                    {
                        xDirection *= -1;
=======
                    position.X += (int)(direction * xSpeed * time);
                    if (position.X <= 0 || position.X + sprite.Size.X >= xLimit)
                    {
                        direction *= -1;
>>>>>>> main
                    }
                    sprite.Position = position;
                    if (!isOnBlock)
                    {

                    }
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
            if (isCollected) return;

            isCollected = true;
        }

        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction)
        {
            if (direction == CollideDirection.Down)
            {
                // this is when it is on ground
                isOnBlock = true;
            }
            if (direction == CollideDirection.Left || direction == CollideDirection.Right)
            {
                xDirection = (-1) * (xDirection);
            }
        }

        public bool getCollected()
        {
            return isCollected;
        }

    }
}
