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

        // Methods
        protected override void Initialize()
        {
            groundRect = new Rectangle(0, 950, 1920, 130); // Ground at the bottom of the screen
            moveLimiter = new Move(_graphics.PreferredBackBufferWidth); // Screen width limiter
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
                new Coin(coinTexture, new Vector2(200, 350), GraphicsDevice),
                new Coin(coinTexture, new Vector2(300, 350), GraphicsDevice),
                new Coin(coinTexture, new Vector2(400, 350), GraphicsDevice)
            };
        }

        // Update game logic
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // Exit Game on Escape key
                Exit();

            // Update player
            player.Update(gameTime);

            // Update coins
            foreach (var coin in coins)
                coin.Update(player.GetHitbox());

            base.Update(gameTime);
        }

        // Draw game objects
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear screen with a color
            _spriteBatch.Begin();

            _spriteBatch.Draw(groundTexture, groundRect, Color.White); // Draw ground
            player.Draw(_spriteBatch);

            // Draw uncollected coins
            foreach (var coin in coins)
                if (!coin.Collected)
                    coin.Draw(_spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
