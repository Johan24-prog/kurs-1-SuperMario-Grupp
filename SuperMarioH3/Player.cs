using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioH3.MoveableObjects
{
    public class Player
    {
        // Fields
        private Texture2D texture;
        private Hitbox hitbox;
        private Move moveLimiter;

        // Keyboard states for input handling
        private KeyboardState previousKey;
        private KeyboardState currentKey;

        //Input handling for gamepad
        private GamePadState currentGamePad;
        private GamePadState previousGamePad;

        // Movement constants
        private const float Gravity = 900f;
        private const float JumpStrength = -400f;
        private const float WalkSpeed = 180f;
        private const float RunSpeed = 250f;
        private const float Accel = 1500f;
        private const float Decel = 3000f;
        private const float TurnDecel = 5000f;

        private Rectangle groundRect; // Ground rectangle for collision
        public VectorPosition Motion { get; private set; } // Player's position and velocity

        // Constructor
        public Player(Texture2D texture, Vector2 startPos, Move moveLimiter, Rectangle groundRect, GraphicsDevice graphics)
        {
            this.texture = texture; // Set player texture
            this.moveLimiter = moveLimiter; // Set movement limiter
            this.groundRect = groundRect; // Set ground rectangle

            // Initialize position and hitbox
            Motion = new VectorPosition(startPos.X, startPos.Y);
            hitbox = new Hitbox(graphics, (int)startPos.X, (int)startPos.Y, texture.Width, texture.Height);

            // Initialize keyboard state
            previousKey = Keyboard.GetState();
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            // (DT) Delta Time = “The amount of time that has passed since the last frame (update).”
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // Delta time calculation
            currentKey = Keyboard.GetState();
            currentGamePad = GamePad.GetState(PlayerIndex.One);

            // Handle movement and jumping
            HandleMovement(dt);
            HandleJumping(dt);
            Motion.ApplyGravity(Gravity, dt);
            Motion.Update(dt);

            // Keep player in screen bounds
            Motion.ClampToScreen(moveLimiter, new Vector2(texture.Width, texture.Height));

            // Ground collision
            Rectangle playerRect = new Rectangle((int)Motion.Position.X, (int)Motion.Position.Y, texture.Width, texture.Height);
            if (playerRect.Bottom >= groundRect.Top)
            {
                Motion.Position = new Vector2(Motion.Position.X, groundRect.Top - texture.Height);
                Motion.Velocity = new Vector2(Motion.Velocity.X, 0);
            }

            // Update hitbox position
            hitbox.Update(Motion.Position);

            //Update input states
            previousKey = currentKey;
            previousGamePad = currentGamePad;
        }

        // Handle horizontal movement
        private void HandleMovement(float dt)
        {
            float moveDirection = 0f;
            if (currentKey.IsKeyDown(Keys.Left) || currentKey.IsKeyDown(Keys.A) ||
                currentGamePad.IsButtonDown(Buttons.DPadLeft)) // Move left
                moveDirection -= 1f;
            if (currentKey.IsKeyDown(Keys.Right) || currentKey.IsKeyDown(Keys.D) ||
                currentGamePad.IsButtonDown(Buttons.DPadRight)) // Move right
                moveDirection += 1f;

            //Run
            float maxSpeed =
                currentKey.IsKeyDown(Keys.B) || currentKey.IsKeyDown(Keys.LeftShift) ||
                currentGamePad.IsButtonDown(Buttons.X)
                ? RunSpeed : WalkSpeed;

            // Apply acceleration and deceleration
            if (moveDirection != 0)
            {
                if (Math.Sign(Motion.Velocity.X) != Math.Sign(moveDirection) && Motion.Velocity.X != 0)
                    Motion.Velocity = new Vector2(Motion.Velocity.X + moveDirection * TurnDecel * dt, Motion.Velocity.Y);
                else
                    Motion.Velocity = new Vector2(Motion.Velocity.X + moveDirection * Accel * dt, Motion.Velocity.Y);

                Motion.Velocity = new Vector2(MathHelper.Clamp(Motion.Velocity.X, -maxSpeed, maxSpeed), Motion.Velocity.Y);
            }

            // Deceleration when no input
            else
            {
                if (Motion.Velocity.X > 0)
                    Motion.Velocity = new Vector2(Math.Max(Motion.Velocity.X - Decel * dt, 0), Motion.Velocity.Y);
                else if (Motion.Velocity.X < 0)
                    Motion.Velocity = new Vector2(Math.Min(Motion.Velocity.X + Decel * dt, 0), Motion.Velocity.Y);
            }
        }

        // Handle jumping logic
        private void HandleJumping(float dt)
        {
            bool isOnGround = Motion.Position.Y + texture.Height >= groundRect.Top; // Check if player is on the ground
            if (isOnGround)
            {
                if (currentKey.IsKeyDown(Keys.Space) && !previousKey.IsKeyDown(Keys.Space) ||
                    currentGamePad.IsButtonDown(Buttons.A) && !previousGamePad.IsButtonDown(Buttons.A))
                {
                    Motion.Velocity = new Vector2(Motion.Velocity.X, JumpStrength); // Apply jump velocity
                }
            }
        }

        // Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Motion.Position, Color.White); // Draw player texture
        }

        public Hitbox GetHitbox() => hitbox; // Get player's hitbox

        public void HandlePlatforms(List<Platform> platforms, float dt)
        {
            bool onPlatform = false;

            Vector2 position = Motion.Position;
            Vector2 velocity = Motion.Velocity;

            foreach (var plat in platforms)
            {
                if (plat.CheckCollision(hitbox.Bounds, ref position, ref velocity))
                    onPlatform = true;
            }

            Motion.Position = position;
            Motion.Velocity = velocity;

            bool isOnGround = Motion.Position.Y + texture.Height >= groundRect.Top;
            if (!onPlatform && !isOnGround)
            {
                Motion.ApplyGravity(Gravity, dt);
            }

        }

    }
}

