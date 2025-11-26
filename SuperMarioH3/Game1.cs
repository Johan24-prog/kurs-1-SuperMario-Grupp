using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperMarioH3.MoveableObjects;
using SuperMarioH3.UI.Menu;

namespace SuperMarioH3
{
    public class Game1 : Game
    {
        //State of game
        public enum GameState { Menu, Playing }
        public GameState State { get; private set; }

        //Font
        SpriteFont font;

        //Menu
        MenuManager menuManager;

        // Fields
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Game objects
        private Move moveLimiter; // Screen movement limiter
        private Player player; // Player instance
        private List<Coin> coins; // List of coins
        private List<Platform> platforms; // List of platforms
        private Camera2D camera; // Camera instance

        private int coinCount = 0; // Collected coins count

        // Textures
        private Texture2D coinTexture; // Coin texture
        private Texture2D groundTexture; // Ground texture
        private Rectangle groundRect; // Ground rectangle
        private Texture2D platformTexture; // Platform texture
        private Texture2D mushroomTexture;
        private List<Enemy> enemies;
        private Texture2D goombaTexture;

        //End of level door
        private Texture2D doorTexture;
        private Rectangle doorRect;

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

        protected override void Initialize()
        {
            //Ändra denna till GameState.Playing för att skippa menyn
            State = GameState.Menu;

            menuManager = new MenuManager();

            groundRect = new Rectangle(0, 950, 1920 * 2, 130); // Ground at the bottom of the screen
            doorRect = new Rectangle(groundRect.Width - 100, groundRect.Y - groundRect.Height + 30, 50, 100);
            moveLimiter = new Move(groundRect.Width); // Screen width limiter

            camera = new Camera2D(GraphicsDevice.Viewport); // Initialize camera

            // Optional world size: let’s say 4000x1080 for a scrolling level
            // Example: camera.SetBounds(new Rectangle(0, 0, 4000, 1080)); // Uncomment to set world bounds
            base.Initialize();

            //Skapar olika menyer
            menuManager.RegisterMenu("Start", () =>
            new StartScreen(this, menuManager, font, _spriteBatch, mushroomTexture));

            menuManager.RegisterMenu("Options", () =>
            new OptionsScreen(this, menuManager, font, _spriteBatch, mushroomTexture));

            menuManager.ShowMenu("Start");
        }

        // Load game content
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice); // Initialize SpriteBatch

            //Load font
            font = Content.Load<SpriteFont>("Retro");

            Texture2D playerTexture = Content.Load<Texture2D>("MarioOneLife"); // Load player texture
            coinTexture = Content.Load<Texture2D>("Coin"); // Load coin texture
            //mushroom texture
            mushroomTexture = Content.Load<Texture2D>("PowerUpOne");

            groundTexture = new Texture2D(GraphicsDevice, 1, 1); // Create ground texture
            groundTexture.SetData(new[] { Color.ForestGreen }); // Set ground color

            //Door texture
            doorTexture = new Texture2D(GraphicsDevice, 1, 1);
            doorTexture.SetData(new[] { Color.DarkRed });

            // Initialize player
            player = new Player(playerTexture, new Vector2(100, 850), moveLimiter, groundRect, GraphicsDevice);

            goombaTexture = Content.Load<Texture2D>("Goomba");

            enemies = new List<Enemy>()
            {
                new Enemy(goombaTexture, new Vector2(400, groundRect.Top - goombaTexture.Height)),
                new Enemy(goombaTexture, new Vector2(1000, groundRect.Top - goombaTexture.Height)),
            };

            // Initialize coins at different positions
            coins = new List<Coin>
            {
                new Coin(coinTexture, new Vector2(500, 0), GraphicsDevice), // Coin 1
                new Coin(coinTexture, new Vector2(800, 0), GraphicsDevice), // Coin 2
                new Coin(coinTexture, new Vector2(1200, 0), GraphicsDevice) // Coin 3
            };

            // Initialize platform texture
            // Load platform picture here Ex. platformTexture = Content.Load<Texture2D>("platform");
            // TODO: Uncomment above and remove below when platform texture (picture) is available

            platformTexture = new Texture2D(GraphicsDevice, 1, 1);
            platformTexture.SetData(new[] { Color.Brown });

            // Initialize platforms
            platforms = new List<Platform>
            {
                new Platform(platformTexture, new Rectangle(500, 800, 200, 20)), // Platform 1
                new Platform(platformTexture, new Rectangle(800, 700, 200, 20)), // Platform 2
                new Platform(platformTexture, new Rectangle(1200, 600, 200, 20)) // Platform 3
            };

            // Snap coins to nearest platform
            foreach (var coin in coins)
            {
                coin.SnapToPlatform(platforms);
            }

        }

        // Update game logic
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape)) // Exit Game on Escape key
                Exit();

            if (State == GameState.Menu)
            {
                menuManager.Update(gameTime);
            }
            else
            {
                // Update player movement and gravity
                player.Update(gameTime);
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Update enemies
                Rectangle worldBounds = new Rectangle(0, 0, 1920, 1080); // Example level size
                foreach (var enemy in enemies)
                    enemy.Update(gameTime, platforms, groundRect, worldBounds);

                // Let the player class handle collisions with platforms
                player.HandlePlatforms(platforms, dt);

                // Update coins (hitbox comes from Player class)
                foreach (var coin in coins)
                {
                    bool wasCollected = coin.Collected; // Store previous collected state

                    coin.Update(player.GetHitbox());

                    // If coin was not collected before but is now, increment coin count
                    if (!wasCollected && coin.Collected)
                    {
                        coinCount++;
                    }
                }

                // Handle collisions between player and enemies
                foreach (var enemy in enemies)
                {
                    if (enemy.IsDead)
                        continue;

                    Rectangle playerBox = player.GetHitbox().Bounds;
                    Rectangle enemyBox = enemy.Hitbox;

                    // --- STOMPING (hoppa på fienden) ---
                    bool stomp =
                        playerBox.Bottom <= enemyBox.Top + 10 &&
                        player.Motion.Velocity.Y > 0 &&
                        playerBox.Intersects(enemyBox);

                    if (stomp)
                    {
                        enemy.Kill();
                        player.Motion.Velocity = new Vector2(player.Motion.Velocity.X, -450f); // studsa upp
                        continue;
                    }

                    // --- SIDOKOLLISION (spelaren tar skada) ---
                    if (playerBox.Intersects(enemyBox))
                    {
                        // TODO: implementera skada / livssystem
                        // Exempel:
                        // player.Die();
                    }
                }
                foreach (var enemy in enemies)
                {
                    enemy.UpdateAI(player.Motion.Position);
                    enemy.Update(gameTime, platforms, groundRect, worldBounds);
                }

                // Remove enemies that are marked for removal
                enemies.RemoveAll(e => e.ReadyToRemove);

                // Update camera to follow player
                camera.Follow(player.Motion.Position);
                base.Update(gameTime);
            }
        }

        // Draw game objects
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue); // Clear screen with a color

            if (State == GameState.Menu)
                menuManager.Draw(gameTime);
            else
            {
                _spriteBatch.Begin(transformMatrix: camera.Transform);
                // Draw ground using the single reused texture
                //_spriteBatch.Draw(groundTexture, groundRect, Color.White);          

                _spriteBatch.Draw(groundTexture, groundRect, Color.White); // Draw ground
                _spriteBatch.Draw(doorTexture, doorRect, Color.White);
                player.Draw(_spriteBatch);

                foreach (var plat in platforms)
                    plat.Draw(_spriteBatch);

                // Draw enemies
                foreach (var enemy in enemies)
                    enemy.Draw(_spriteBatch);

                // Draw uncollected coins
                foreach (var coin in coins)
                    if (!coin.Collected)
                        coin.Draw(_spriteBatch);

                Vector2 coinPos = new Vector2(10, 10);

                // Draw coin icon
                _spriteBatch.Draw(
                    coinTexture,
                    coinPos,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1f,
                    SpriteEffects.None,
                    0f);

                // Icon width for positioning text
                float iconWidthScaled = coinTexture.Width * 1f;
                float spacing = 10f;  // Space between icon and text

                // Draw coin count               
                _spriteBatch.DrawString(
                    font,
                    "x " + coinCount,
                    new Vector2(coinPos.X + iconWidthScaled + spacing, coinPos.Y + 5),
                    Color.Yellow,
                    0f,
                    Vector2.Zero,
                    0.6f,
                    SpriteEffects.None,
                    0f);

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
        public void StartGame() => State = GameState.Playing;
        public void ToggleFullScreen(bool fullScreen)
        {
            _graphics.IsFullScreen = fullScreen;
            _graphics.ApplyChanges();
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