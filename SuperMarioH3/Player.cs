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
        private Texture2D texture; // Player texture
        private Hitbox hitbox; // Player's hitbox
        private Hitbox playerOldHitbox; // Previous hitbox for collision detection
        private Move moveLimiter; // Movement limiter for screen bounds
        private Vector2 oldPlayerPosition; // Previous player position for collision detection

        private bool isOnPlatform = false; // check if player is on a platform

        // Keyboard states for input handling
        private KeyboardState previousKey;
        private KeyboardState currentKey;

        //Input handling for gamepad
        private GamePadState currentGamePad;
        private GamePadState previousGamePad;

        // Movement constants
        private const float Gravity = 1700f;
        private const float JumpStrength = -750f;
        private const float WalkSpeed = 300f;
        private const float RunSpeed =450f;
        private const float Accel = 1500f;
        private const float Decel = 3000f;
        private const float TurnDecel = 5000f;
        // TODO: Update these values for better feeling in-game

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
            playerOldHitbox = new Hitbox(graphics, (int)startPos.X, (int)startPos.Y, texture.Width, texture.Height);


            // Initialize keyboard state
            previousKey = Keyboard.GetState();
        }

        // Update method
        public void Update(GameTime gameTime)
        {
            // (DT) Delta Time = “The amount of time that has passed since the last frame (update).”
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds; // Delta time calculation
            // Save old position BEFORE movement update for collision detection
            oldPlayerPosition = Motion.Position;
            playerOldHitbox.Update(oldPlayerPosition);

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

            bool canJump = isOnGround || isOnPlatform;
            if (canJump)
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
            // Flag to check if player is on any platform
            isOnPlatform = false;

            // Current position and velocity
            Vector2 position = Motion.Position;
            Vector2 velocity = Motion.Velocity;

            // Current and old hitbox rectangles for collision detection
            Rectangle currentBox = hitbox.Bounds;
            Rectangle oldBox = playerOldHitbox.Bounds;

            // Check collision with each platform
            foreach (var plat in platforms)
            {
                if (plat.CheckCollision(currentBox, oldBox, ref position, ref velocity))
                {
                    isOnPlatform = true;
                    break;
                }
            }

            // Update position and velocity after platform collision checks
            Motion.Position = position;
            Motion.Velocity = velocity;

            // NOTE: gravity is handled once per frame in Update(), so do NOT apply it here.
        }



    }
}

