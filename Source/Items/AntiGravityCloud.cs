using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.Items
{
    internal class AntiGravityCloud : IItems, ICollidable
    {
        private Sprite.ISprite sprite;

        private const float xSpeed = 100f;
        private const float riseSpeed = 25f; 

        private const float bobSpeed = 4f;
        private const float bobDist = 25f;

        private Vector2 floatPosition;
        private float centerPoint;
        private float totalTime = 0f;

        private bool isCollected = false;
        private bool hasRisen = false;
        private float riseAmount = 0f;
        private float riseTarget = 32f;

        public Rectangle CollisionBox
        {
            get
            {
                return new Rectangle((int)floatPosition.X + 2, (int)floatPosition.Y, sprite.Size.X, sprite.Size.Y);
            }
        }

        public Point position { get => sprite.Position; private set => sprite.Position = value; }

        public AntiGravityCloud(SharedTexture texture, int positionX, int positionY)
        {
            sprite = texture.NewSprite(95, 118, 16, 13);
            sprite.Scale = 2f;

            floatPosition = new Vector2(positionX, positionY);
            centerPoint = positionY;

            sprite.Position = floatPosition.ToPoint();
            CollisionController.Instance.AddItem(this);
        }

        public void Update(GameTime gameTime)
        {
            if (isCollected) return;

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!hasRisen)
            {
                float riseStep = 40f * dt;
                floatPosition.Y -= riseStep;
                riseAmount += riseStep;
                if (riseAmount >= riseTarget)
                {
                    hasRisen = true;
                    centerPoint = floatPosition.Y;
                }
            }
            else
            {
                totalTime += dt;

                floatPosition.X += xSpeed * dt;
                centerPoint -= riseSpeed * dt;

                float sineOffset = (float)Math.Sin(totalTime * bobSpeed) * bobDist;

                floatPosition.Y = centerPoint + sineOffset;
            }

            sprite.Position = floatPosition.ToPoint();
            sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCollected) sprite.Draw(spriteBatch);
        }

        public void OnCollidePlayer(Player player, CollideDirection direction)
        {
            isCollected = true;
            CollisionController.Instance.RemoveItem(this);
            sprite.Drop();
        }
        public void OnCollideBlock(IBlock block, CollideDirection direction) { }
        public void OnCollideItem(IItems item, CollideDirection direction) { }
        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }

        public bool getCollected() => isCollected;
    }
}