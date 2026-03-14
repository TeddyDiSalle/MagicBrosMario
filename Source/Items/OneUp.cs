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
        private float xSpeed = 120f;
        private float gravitySpeed = 250f;
        private int xDirection = 1;
        private int xLimit;
        private int yLimit;
        private bool isCollected = false;
        private bool hasRisen = false;
        private bool isOnBlock = false;
        private float riseAmount = 0f;
        private float riseTarget = 48f;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(position.X, position.Y, (int)(16 * sprite.Scale), (int)(16 * sprite.Scale));
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
            CollisionController.Instance.AddItem(this);

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
                    position.X += (int)(xDirection * xSpeed * time);

                    if (position.X <= 0)
                    {
                        position.X = 1;
                        xDirection = 1;
                    }
                    //else if (position.X + CollisionBox.Width >= xLimit)
                    //{
                    //    position.X = xLimit - CollisionBox.Width - 1;
                    //    xDirection = -1;
                    //}

                    if (position.Y > yLimit)
                    {
                        isCollected = true;
                        CollisionController.Instance.RemoveItem(this);
                    }

                    sprite.Position = position;
                }

                sprite.Update(gameTime);
                isOnBlock = false;
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
            CollisionController.Instance.RemoveItem(this);
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