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

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set preferred window size (example values)
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }

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

        // Input state
        private KeyboardState currentKey;
        private KeyboardState previousKey;

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // Example: Initialize player position and ground
            playerPosition = new Vector2(100, 400);
            playerVelocity = Vector2.Zero;

            groundRect = new Rectangle(0, 400, 800, 800); // Ex. Ground at y=400

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            playerTexture = Content.Load<Texture2D>("MarioOneLife");
            // Example:
            // playerTexture = new Texture2D(GraphicsDevice, 16, 16); // Placeholder for player texture
            // Color[] data = new Color[16 * 16];
            // for (int i = 0; i < data.Length; ++i) data[i] = Color.Red; // Fill with red color (Can use picture instead)
            // playerTexture.SetData(data);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            currentKey = Keyboard.GetState();
            if (currentKey.IsKeyDown(Keys.Escape))
                Exit();

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Horizontal movement
            float moveDirection = 0f;
            if (currentKey.IsKeyDown(Keys.Left) || currentKey.IsKeyDown(Keys.A))
                moveDirection -= 1f;
            if (currentKey.IsKeyDown(Keys.Right) || currentKey.IsKeyDown(Keys.D))
                moveDirection += 1f;

            // Move player
            playerVelocity.X = moveDirection * MoveSpeed;

            // Jumping
            // Check if on ground
            bool isOnGround = playerPosition.Y + playerTexture.Height >= groundRect.Y;
            // Allow jump if on ground and space is pressed
            if (isOnGround && currentKey.IsKeyDown(Keys.Space) && !previousKey.IsKeyDown(Keys.Space))
            {
                playerVelocity.Y = jumpStrength;
            }

            // Apply gravity
            playerVelocity.Y += Gravity * dt;

            // Update position
            playerPosition += playerVelocity * dt;

            // Simple collision with ground
            Rectangle playerRect = new Rectangle((int)playerPosition.X, (int)playerPosition.Y, 16, 16);
            if (playerRect.Bottom >= groundRect.Top)
            {
                // Snap to ground
                playerPosition.Y = groundRect.Top - 16;
                playerVelocity.Y = 0;
            }

            
            previousKey = currentKey;
            base.Update(gameTime);
        }
        private bool IsOnGround()
        {
            return playerPosition.Y + playerTexture.Height >= groundRect.Y;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin();

            // Draw ground
            Texture2D groundTexture = new Texture2D(GraphicsDevice, 1, 1);
            groundTexture.SetData(new[] { Color.ForestGreen });
            _spriteBatch.Draw(groundTexture, groundRect, Color.White);

            // Draw player
            _spriteBatch.Draw(playerTexture, playerPosition, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
