using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioH3.MoveableObjects;

namespace SuperMarioH3
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Move MoveLimiter;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set preferred window size (example values)
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        // Coins
        private List<Coin> coins;
        private Texture2D coinTexture;

        // Player properties
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private Vector2 playerVelocity;
        private Hitbox playerHitbox;

        // Movement constants (example values)
        private const float MoveSpeed = 150f;
        private const float jumpStrength = -400f;
        private const float Gravity = 900f;

        // Ground
        private Rectangle groundRect;
        private Texture2D groundTexture; // reuse instead of allocating every Draw

        // Input state
        private KeyboardState currentKey;
        private KeyboardState previousKey;

        // Plattformar
        private List<Platform> platforms;
        private Texture2D platformTexture;

        protected override void Initialize()
        {
            // Initialize player position and ground
            playerPosition = new Vector2(100, 850);
            playerVelocity = Vector2.Zero;

            // Ground top at y = 400, height set to cover bottom area
            groundRect = new Rectangle(0, 950, 1920, 130);

            // Initialize MoveLimiters
            MoveLimiter = new Move(
                _graphics.PreferredBackBufferWidth);

            // Initialize previousKey to current keyboard state to avoid false "just pressed" detections on first frame
            previousKey = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load textures
            playerTexture = Content.Load<Texture2D>("MarioOneLife");
            coinTexture = Content.Load<Texture2D>("Coin");

            // Create a single 1x1 ground texture and reuse it (avoid allocating each Draw)
            groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            groundTexture.SetData(new[] { Color.ForestGreen });

            coins = new List<Coin>
            {
                new Coin(coinTexture, new Vector2(200, 350), GraphicsDevice),
                new Coin(coinTexture, new Vector2(300, 350), GraphicsDevice),
                new Coin(coinTexture, new Vector2(400, 350), GraphicsDevice)
            };

            // Initialize player's hitbox
            playerHitbox = new Hitbox(GraphicsDevice, (int)playerPosition.X, (int)playerPosition.Y, playerTexture.Width, playerTexture.Height);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKey = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //TODO: Flytta all kod som hanterar spelarns uppdatering till dennes klass

            float walkSpeed = 180f;
            float runSpeed = 250f;
            float accel = 1500f;
            float decel = 3000f;
            float turnDecel = 5000f;

            // Horizontal movement
            float moveDirection = 0f;
            if (currentKey.IsKeyDown(Keys.Left) || currentKey.IsKeyDown(Keys.A))
                moveDirection -= 1f;
            if (currentKey.IsKeyDown(Keys.Right) || currentKey.IsKeyDown(Keys.D))
                moveDirection += 1f;

            float maxSpeed = walkSpeed;
            if (currentKey.IsKeyDown(Keys.B) || currentKey.IsKeyDown(Keys.LeftShift))
                maxSpeed = runSpeed;

            if (moveDirection != 0)
            {
                // if player changes direction, faster decel. 
                if (Math.Sign(playerVelocity.X) != Math.Sign(moveDirection) && playerVelocity.X != 0)
                {
                    // Decelerate quickly
                    playerVelocity.X += moveDirection * turnDecel * dt;
                }
                else
                {
                    // Normal acceleration
                    playerVelocity.X += moveDirection * accel * dt;
                }

                // Clamp to max speed
                if (playerVelocity.X > maxSpeed) playerVelocity.X = maxSpeed;
                if (playerVelocity.X < -maxSpeed) playerVelocity.X = -maxSpeed;
            }
            else
            {
                // No input, normal deccel
                if (playerVelocity.X > 0)
                {
                    playerVelocity.X -= decel * dt;
                    if (playerVelocity.X < 0) playerVelocity.X = 0;
                }
                else if (playerVelocity.X < 0)
                {
                    playerVelocity.X += decel * dt;
                    if (playerVelocity.X > 0) playerVelocity.X = 0;
                }
            }

            // Move player
            //playerVelocity.X = moveDirection * MoveSpeed;

            // Jumping: check if on ground and space was just pressed
            bool isOnGround = playerPosition.Y + (playerTexture?.Height ?? 16) >= groundRect.Y;
            if (isOnGround && currentKey.IsKeyDown(Keys.Space) && !previousKey.IsKeyDown(Keys.Space))
            {
                playerVelocity.Y = jumpStrength;
            }

            // Apply gravity
            playerVelocity.Y += Gravity * dt;

            // Update position
            playerPosition += playerVelocity * dt;

            // Keep player within screen bounds
            MoveLimiter.Clamp(ref playerPosition, new Vector2(playerTexture.Width, playerTexture.Height));

            // Simple collision with ground
            int playerWidth = playerTexture?.Width ?? 16;
            int playerHeight = playerTexture?.Height ?? 16;
            Rectangle playerRect = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, playerWidth, playerHeight);

            if (playerRect.Bottom >= groundRect.Top)
            {
                // Snap to ground
                playerPosition.Y = groundRect.Top - playerHeight;
                playerVelocity.Y = 0;
                playerRect.Y = (int)playerPosition.Y;
            }

            // Update player's hitbox to current position
            playerHitbox.Update(playerPosition);

            // Update coins in Update (not Draw) so game logic is separated from rendering
            foreach (var coin in coins)
            {
                coin.Update(playerHitbox);
            }

            previousKey = currentKey;
            base.Update(gameTime);
        }

        private bool IsOnGround()
        {
            return playerPosition.Y + (playerTexture?.Height ?? 16) >= groundRect.Y;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw ground using the single reused texture
            _spriteBatch.Draw(groundTexture, groundRect, Color.White);          

            // Draw player
            _spriteBatch.Draw(playerTexture, playerPosition, Color.White);

            // Draw coins (call their Draw method)
            foreach (var coin in coins)
            {
                if (!coin.Collected)
                    coin.Draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
} 
  // TODO: Add Terrain and Platforms for the player to jump on.
  // TODO: Implement animations for the player and coins.
  // TODO: Make the Game1 class partial and split code into multiple files for better organization.
