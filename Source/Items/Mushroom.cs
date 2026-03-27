using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.MarioStates;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MagicBrosMario.Source.Items
{
    internal class Mushroom : IItems
    {
        private const float GRAVITY_SPEED = 250f;
        private const float RISE_SPEED = 30f;
        private const float X_SPEED = 120f;

        private Sprite.Sprite sprite;
        private Vector2 floatPosition; 

        private int xDirection = 1;
        private bool isCollected = false;
        private bool hasRisen = false;
        private bool isOnBlock = false;
        private float riseAmount = 0f;
        private float riseTarget = 32f;

        public Rectangle CollisionBox
        {
            get
            {
                int width = (int)(sprite.Size.X - 2);
                int height = sprite.Size.Y;
                return new Rectangle(sprite.Position.X + 1, sprite.Position.Y, width, height);
            }
        }

        public Point position { get => sprite.Position; private set => sprite.Position = value; }

        public Mushroom(SharedTexture texture, int positionX, int positionY)
        {
            sprite = texture.NewSprite(184, 34, 16, 16);
            sprite.Scale = 2f;

            floatPosition = new Vector2(positionX, positionY);
            sprite.Position = floatPosition.ToPoint();

            CollisionController.Instance.AddItem(this);
        }

        public void Update(GameTime gameTime)
        {
            if (isCollected) return;

            float time = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!hasRisen)
            {
                Rise(time);
            }
            else
            {
                Move(time);
            }

            sprite.Position = floatPosition.ToPoint();

            isOnBlock = false;
        }

        private void Rise(float time)
        {
            float step = RISE_SPEED * time;
            floatPosition.Y -= step;
            riseAmount += step;

            if (riseAmount >= riseTarget)
            {
                hasRisen = true;
            }
        }

        private void Move(float dt)
        {
            if (!isOnBlock)
            {
                floatPosition.Y += (GRAVITY_SPEED * dt);
            }

            floatPosition.X += (xDirection * X_SPEED * dt);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!isCollected)
            {
                sprite.Draw(spriteBatch);
            }
        }

        public void OnCollideBlock(IBlock block, CollideDirection direction)
        {
            if (!hasRisen) return;

            if (direction == CollideDirection.Down)
            {
                isOnBlock = true;
                floatPosition.Y = block.CollisionBox.Top - sprite.Size.Y;
            }
            else if (direction == CollideDirection.Left)
            {
                xDirection = 1;
                floatPosition.X = block.CollisionBox.Right + 1;
            }
            else if (direction == CollideDirection.Right)
            {
                xDirection = -1;
                floatPosition.X = block.CollisionBox.Left - sprite.Size.X - 1;
            }
        }

        public void OnCollidePlayer(Player player, CollideDirection direction)
        {
            isCollected = true;
            CollisionController.Instance.RemoveItem(this);
            sprite.Drop();
        }

        public void OnCollideItem(IItems item, CollideDirection direction) { }
        public void OnCollideEnemy(IEnemy enemy, CollideDirection direction) { }
        public bool getCollected() => isCollected;
    }
}