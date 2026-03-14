using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;

namespace MagicBrosMario.Source.Items
{
<<<<<<< HEAD
    public class Star : IItems
    {
        private AnimatedSprite sprite;
        private Vector2 position;
        private float speed = 100f;
        private float gravitySpeed = 220f;
        private int xDirection = 1;
        private int yDirection = -1;
        private int xLimit;
        private float yBottom;
        private float yTop;
=======
	public class Star : IItems
	{
		private AnimatedSprite sprite;
		private Vector2 position;
		private float speed = 100f;
        private float gravitySpeed = 220f;
        private int xDirection = 1;
		private int yDirection = -1;
		private int xLimit;
		private float yBottom;
		private float yTop;
>>>>>>> main
        private bool isCollected = false;
        private bool hasRisen = false;
        private bool isOnBlock = false;
        private float riseAmount = 0f;
        private float riseTarget = 16f;


        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle(sprite.Position.X, sprite.Position.Y, (int)(14 * sprite.Scale), (int)(16 * sprite.Scale));
            }
        }

        public Star(SharedTexture texture, int screenWidth, int screenHeight, int positionX, int positionY)
        {
            sprite = new AnimatedSprite(texture, 5, 94, 14, 16, 4, 0.03f);
            yBottom = positionY + 30f;
            yTop = positionY - 30f;
            xLimit = screenWidth;

            if (positionX <= 0)
            {
                positionX = 1;
            }
            if (positionY <= 0)
            {
                positionY = 1;
            }
<<<<<<< HEAD
=======

            position = new Vector2(positionX, positionY);
			sprite.Position = position.ToPoint();
			sprite.Scale = 3f;
>>>>>>> main

            position = new Vector2(positionX, positionY);
            sprite.Position = position.ToPoint();
            sprite.Scale = 3f;

<<<<<<< HEAD
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
                    position.X += (int)(xDirection * speed * time);
                    position.Y += (int)(yDirection * speed * time);

                    if (position.X <= 0)
                    {
                        position.X = 0;
                        xDirection = 1;
                    }
                    else if (position.X + sprite.Size.X >= xLimit)
                    {
                        position.X = xLimit - sprite.Size.X;
                        xDirection = -1;
                    }

                    if (!isOnBlock)
=======
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
                position.X += (int)(xDirection * speed * time);
                position.Y += (int)(yDirection * speed * time);

                if (position.X <= 0)
                {
                    position.X = 0;
                    xDirection = 1;
                }
                else if (position.X + sprite.Size.X >= xLimit)
                {
                    position.X = xLimit - sprite.Size.X;
                    xDirection = -1;
                }

                if (isOnBlock)
>>>>>>> main
                    {
                        if (position.Y <= yTop)
                        {
                            position.Y = yTop;
                            yDirection = 1;
                        }
                        else if (position.Y >= yBottom)
                        {
                            position.Y = yBottom;
                            yDirection = -1;
                        }
                    }
<<<<<<< HEAD
                    else
=======
                else
>>>>>>> main
                    {
                        position.Y += (int)(gravitySpeed * time);
                    }


<<<<<<< HEAD
                    sprite.Position = position.ToPoint();
                }
                sprite.Update(gameTime);
            }
        }
=======
                sprite.Position = position.ToPoint();
            }
            sprite.Update(gameTime);
         }
    }
>>>>>>> main

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
