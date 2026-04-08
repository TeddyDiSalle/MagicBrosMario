using MagicBrosMario.Source.Block;
using MagicBrosMario.Source.Collision;
using MagicBrosMario.Source.Items;
using MagicBrosMario.Source.Sound;
using MagicBrosMario.Source.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace MagicBrosMario.Source.MarioStates;
//Vincent Do
public class Player : ICollidable
{
    private IPlayerState PlayerState { get; set; }

    public Vector2 Position { get; private set; } = new Vector2(400, 240);
    public Vector2 Velocity { get; private set; }
    public int ScaleFactor { get; } = 2;
    private const float MovementSpeed = 5.0f, Gravity = 0.35f, MaxSpeed = 4.0f, fireballCooldown = 0.2f;
    public float TimeFrame { get; } = 0.15f;
    public bool IsGrounded { get; set; } = false;
    public bool IsCrouching { get; private set; } = false;
    public bool Flipped { get; set; } = false;
    public bool Invincible { get; set; } = false;
    private double StarDuration { get; set; } = 10;
    public double StarTimeRemaining { get; set; } = 0;
    private readonly List<MarioFireball> fireballs = [];
    private float FireballTimer = 0;
    public int Lives { get; set; } = 3;
    public bool IsAlive { get; set; } = true;
    private const double DamageCoolDown = 2.0;
    private double DamageTimer = 0;
    public Sprite.SharedTexture Texture { get; }
    public bool IsJumping { get; set; } = false;
    private readonly PlayerCollisionHandler PlayerCollision;
    public Rectangle CollisionBox {get; set; }

    public Player(Sprite.SharedTexture texture)
    {
        this.Texture = texture;
        CollisionBox = new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
        PlayerState = new SmallMarioIdleState(this, texture, TimeFrame, ScaleFactor);
        PlayerCollision = new PlayerCollisionHandler(this);
    }
    public void CreateFireball()
    {
        if(FireballTimer < fireballCooldown) { return; }
        SoundController.PlaySound(SoundType.Fireball, 1.0f);
        FireballTimer = 0;
        AnimatedSprite movingFireball = Texture.NewAnimatedSprite(207, 168, 8, 8, 4, TimeFrame);
        Sprite.Sprite explosion = Texture.NewSprite(239, 168, 8, 8);

        movingFireball.Scale = ScaleFactor;
        explosion.Scale = ScaleFactor;
        movingFireball.HFlipped = Flipped;
        explosion.HFlipped = Flipped;
        MarioFireball fireball = new(movingFireball, explosion, Position, !Flipped, ScaleFactor);
        fireballs.Add(fireball);
    }
    public void Left(GameTime gameTime)
    {
        Flipped = true;
        PlayerState.Left(gameTime);
    }
    public void Right(GameTime gameTime)
    {
        Flipped = false;
        PlayerState.Right(gameTime);
    }
    public void Jump(GameTime gameTime)
    {
        PlayerState.Jump(gameTime);
    }
    public void Crouch(GameTime gameTime)
    {
        IsCrouching = true;
        PlayerState.Crouch(gameTime);
    }
    public void SetPositon(Vector2 pos)
    {
        Position = pos;
        CollisionBox = new Rectangle(
            (int)Math.Ceiling(pos.X),
            (int)Math.Ceiling(pos.Y),
            CollisionBox.Width, CollisionBox.Height);
    }
    public void SetVelocity(Vector2 vel)
    {
        Velocity = vel;
    }
    public void ReleaseCrouch()
    {
        IsCrouching = false;
    }
    public void Attack()
    {
        if(fireballs.Count < 2)
            PlayerState.Attack();
    }
    public void TakeDamage()
    {
        if (DamageTimer >= DamageCoolDown)
        {
            DamageTimer = 0;
            PlayerState.TakeDamage();
        }
    }
    public void KickInvinsibility()
    {
        DamageTimer = DamageCoolDown - 0.2;
    }
    public void KillMario()
    {
        if(PlayerState is not MarioDeadState)
        {
            ChangeState(new MarioDeadState(this, Texture, TimeFrame, ScaleFactor));
        }
        Lives--;
        //Death Sound
    }
    public void PowerUp(Power power)
    {
        PlayerState.PowerUp(power);
    }
    public Power GetCurrentPower()
    {
        return PlayerState.GetCurrentPower();
    }
    public void ChangeState(IPlayerState state)
    {
        PlayerState.StateChangePrep();
        PlayerState = state;
    }
    public void MoveLeft(GameTime gameTime, float factor = 1.0f)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed * factor;
        Velocity -= new Vector2(distanceMoved, 0);
        if(-Velocity.X > MaxSpeed)
        {
            Velocity = new Vector2(-MaxSpeed, Velocity.Y);
        }
    }
    public void MoveRight(GameTime gameTime, float factor = 1.0f)
    {
        float distanceMoved = (float)gameTime.ElapsedGameTime.TotalSeconds * MovementSpeed * factor;
        Velocity += new Vector2(distanceMoved, 0);
        if (Velocity.X > MaxSpeed)
        {
            Velocity = new Vector2(MaxSpeed, Velocity.Y);
        }
    }
    public void MoveUp(GameTime gameTime, float factor = 1.0f)
    {
        float distanceMoved = (float)(gameTime.ElapsedGameTime.TotalSeconds * 45 * MovementSpeed * factor);
        Velocity -= new Vector2(0, distanceMoved);
    }
    public void Idle()
    {
        PlayerState.Idle();
        if (Velocity.X < 0)
        {
            Velocity += new Vector2(0.1f, 0);
            if(Velocity.X > 0)
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }
        else if(Velocity.X > 0)
        {
            Velocity -= new Vector2(0.1f, 0);
            if (Velocity.X < 0)
            {
                Velocity = new Vector2(0, Velocity.Y);
            }
        }
    }
    //Collision Handling Methods
    public void OnCollidePlayer(Player player, Collision.CollideDirection direction)
    {
        PlayerCollision.OnCollidePlayer(player, direction);
    }
    public void OnCollideItem(IItems item, Collision.CollideDirection direction)
    {
        PlayerCollision.OnCollideItem(item, direction);
    }
    public void OnCollideEnemy(IEnemy enemy, Collision.CollideDirection direction)
    {
        PlayerCollision.OnCollideEnemy(enemy, direction);
    }
    public void OnCollideBlock(IBlock block, Collision.CollideDirection direction)
    {
        PlayerCollision.OnCollideBlock(block, direction);
    }
    //Update and Draw
    public void Update(GameTime gameTime)
    {
        bool wasGrounded = IsGrounded;
        IsGrounded = false;
        if (DamageTimer < DamageCoolDown)
        {
            DamageTimer += gameTime.ElapsedGameTime.TotalSeconds;
        }
        if (!wasGrounded) { 
            Velocity += new Vector2(0, Gravity);
        }
        Position += Velocity;
        if(StarTimeRemaining >= StarDuration)
        {
            StarTimeRemaining = 0;
            Invincible = false;
        }
        if (IsCrouching)
        {
            Idle();
        }
        if(FireballTimer < fireballCooldown)
        {
            FireballTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        for (int i = fireballs.Count - 1; i >= 0; i--)
        {
            fireballs[i].Update(gameTime);
            if (fireballs[i].IsExpired())
            {
                fireballs.RemoveAt(i);
            }
        }
        if (Position.X < Camera.Instance.Position.X)
        {
            SetPositon(new Vector2(Camera.Instance.Position.X, Position.Y));
            SetVelocity(new Vector2(0, Velocity.Y));
        }
        if (Position.Y > Camera.Instance.Position.Y + Camera.Instance.WindowSize.Y)
        {
            KillMario();
        }
        CollisionBox = new Rectangle(
            (int)Math.Ceiling(Position.X),
            (int)Math.Ceiling(Position.Y),
            CollisionBox.Width, CollisionBox.Height);
        PlayerState.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        PlayerState.Draw(spriteBatch);
        foreach(MarioFireball fireball in fireballs)
        {
            fireball.Draw(spriteBatch);
        }
    }
}
