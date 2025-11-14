using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioH3.MoveableObjects;

namespace SuperMarioH3
{
    public class Game1 : Game
    {
        // Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Game objects
        private Move moveLimiter; // Screen movement limiter
        private Player player; // Player instance
        private List<Coin> coins; // List of coins
        private Camera2D camera; // Camera instance

        // Textures
        private Texture2D coinTexture; // Coin texture
        private Texture2D groundTexture; // Ground texture
        private Rectangle groundRect; // Ground rectangle

        // Constructor
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this); // Initialize graphics manager
            Content.RootDirectory = "Content"; // Set content root directory
            IsMouseVisible = true; // Show mouse cursor

            // Set full screen resolution
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
        }

        // Coins
        //private List<Coin> coins;
        //private Texture2D coinTexture;

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
        //private Rectangle groundRect;
        //private Texture2D groundTexture; // reuse instead of allocating every Draw

        // Input state
        private KeyboardState currentKey;
        private KeyboardState previousKey;

        // Plattformar
        private List<Platform> platforms;
        private Texture2D platformTexture;

        protected override void Initialize()
        {
            groundRect = new Rectangle(0, 950, 1920, 130); // Ground at the bottom of the screen
            moveLimiter = new Move(_graphics.PreferredBackBufferWidth); // Screen width limiter

            camera = new Camera2D(GraphicsDevice.Viewport); // Initialize camera

            // Optional world size: let’s say 4000x1080 for a scrolling level
            // Example: camera.SetBounds(new Rectangle(0, 0, 4000, 1080)); // Uncomment to set world bounds
            base.Initialize();
        }

        // Load game content
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // Initialize SpriteBatch

            Texture2D playerTexture = Content.Load<Texture2D>("MarioOneLife"); // Load player texture
            coinTexture = Content.Load<Texture2D>("Coin"); // Load coin texture

            groundTexture = new Texture2D(GraphicsDevice, 1, 1); // Create ground texture
            groundTexture.SetData(new[] { Color.ForestGreen }); // Set ground color

            // Initialize player
            player = new Player(playerTexture, new Vector2(100, 850), moveLimiter, groundRect, GraphicsDevice);

            // Initialize coins at different positions
            coins = new List<Coin>
            {
                new Coin(coinTexture, new Vector2(200, 350), GraphicsDevice), // Coin 1
                new Coin(coinTexture, new Vector2(300, 350), GraphicsDevice), // Coin 2
                new Coin(coinTexture, new Vector2(400, 350), GraphicsDevice) // Coin 3
            };

            // Initialize platform texture
            platformTexture = new Texture2D(GraphicsDevice, 1, 1);
            platformTexture.SetData(new[] { Color.Brown });

            // Initialize platforms
            platforms = new List<Platform>
            {
                new Platform(platformTexture, new Rectangle(500, 800, 200, 20)), // Platform 1
                new Platform(platformTexture, new Rectangle(800, 700, 200, 20)), // Platform 2
                new Platform(platformTexture, new Rectangle(1200, 600, 200, 20)) // Platform 3
            };

        }

        // Update game logic
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // Exit Game on Escape key
                Exit();

            // Update player
            player.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            player.HandlePlatforms(platforms, dt);


            // Update coins
            foreach (var coin in coins)
                coin.Update(player.GetHitbox());

            // Update camera to follow player
            player.Update(gameTime);
            camera.Follow(player.Motion.Position);

            base.Update(gameTime);
        }

        // Draw game objects
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear screen with a color

            // Draw ground using the single reused texture
            //_spriteBatch.Draw(groundTexture, groundRect, Color.White);          

            _spriteBatch.Draw(groundTexture, groundRect, Color.White); // Draw ground
            player.Draw(_spriteBatch);

            foreach (var plat in platforms)
                plat.Draw(_spriteBatch);

            // Draw uncollected coins
            foreach (var coin in coins)
                if (!coin.Collected)
                    coin.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
// TODO: Add more game features like enemies, power-ups, levels, etc.
// TODO: Implement terrain and background rendering.
// TODO: Improve Camera2D functionality if needed.
// TODO: Implement level design and loading.
// TODO: Refine player animations and states.
// TODO: Implement enemy AI and interactions.
// TODO: Implement coin collection logic and scoring system.
// TODO: Implement scoring and UI elements.
// TODO: Implement sound effects and background music. (Optional)
// TODO: Implement level transitions and checkpoints. (Optional)