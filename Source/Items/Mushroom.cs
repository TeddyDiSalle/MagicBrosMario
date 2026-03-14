// Made by Brian
using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics.Metrics;

namespace MagicBrosMario.Source.Items
{
    internal class Mushroom : IItems
    {
        private const float GRAVITY_SPEED = 250f;


        private Sprite.Sprite sprite;
        private Point position;
        private float xSpeed = 120f;
        private int xDirection = 1;
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

        public Mushroom(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
        {
            
            sprite = texture.NewSprite(184, 34, 16, 16);
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
                    float riseStep = GRAVITY_SPEED * time;

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
                        position.Y += (int)(GRAVITY_SPEED * time);
                    }
                    position.X += (int)(xDirection * xSpeed * time);
                    
                    sprite.Position = position;
                }

                sprite.Update(gameTime);
            } else
            {
                return;
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
            if (isCollected) return;
        }


        public void OnCollideItem(IItems item, CollideDirection direction) { }

        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public void OnCollideBlock(IBlock block, CollideDirection direction)
        {
            if (direction == CollideDirection.Down) 
            {
                isOnBlock = true;
                position.Y = block.CollisionBox.Top - (int)(16 * sprite.Scale);
            }
            else if (direction == CollideDirection.Left) 
            {
                xDirection = 1;
                position.X = block.CollisionBox.Right + 1;
            }
            else if (direction == CollideDirection.Right)
            {
                xDirection = -1; 
                position.X = block.CollisionBox.Left - (int)(16 * sprite.Scale) - 1;
            }

        }

        public bool getCollected()
        {
            return isCollected;
        }

    }
}
