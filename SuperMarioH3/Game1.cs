using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioH3.MoveableObjects;
using System.Collections.Generic;

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
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

        // Coins
        private List<Coin> coins;
        private Texture2D coinTexture;

        // Player properties
        private Texture2D playerTexture;
        private Vector2 playerPosition;
        private Vector2 playerVelocity;

        // Movement constants (example values)
        private const float MoveSpeed = 150f;
        private const float jumpStrength = -300f;
        private const float Gravity = 900f;

        // Ground
        private Rectangle groundRect;
        private Texture2D groundTexture; // reuse instead of allocating every Draw

        // Input state
        private KeyboardState currentKey;
        private KeyboardState previousKey;

        // Hitbox property for the player
        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 16, 16);
            }
        }
        protected override void Initialize()
        {
            // Initialize player position and ground
            playerPosition = new Vector2(100, 400);
            playerVelocity = Vector2.Zero;

            // Ground top at y = 400, height set to cover bottom area
            groundRect = new Rectangle(0, 400, 800, 200);

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
                new Coin(coinTexture, new Vector2(200, 350)),
                new Coin(coinTexture, new Vector2(300, 350)),
                new Coin(coinTexture, new Vector2(400, 350))
            };
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            currentKey = Keyboard.GetState();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Horizontal movement
            float moveDirection = 0f;
            if (currentKey.IsKeyDown(Keys.Left) || currentKey.IsKeyDown(Keys.A))
                moveDirection -= 1f;
            if (currentKey.IsKeyDown(Keys.Right) || currentKey.IsKeyDown(Keys.D))
                moveDirection += 1f;

            // Move player
            playerVelocity.X = moveDirection * MoveSpeed;

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

            // Update coins in Update (not Draw) so game logic is separated from rendering
            foreach (var coin in coins)
            {
                coin.Update(playerRect);
            }
            // Hitbox for player
            Rectangle playerHitbox = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 16, 16); // Example size
            // Update coins with hitbox
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
