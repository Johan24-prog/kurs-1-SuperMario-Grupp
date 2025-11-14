using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioH3.Spelobject__Bas_;
using System.Collections.Generic;

namespace SuperMarioH3
{
    public class Coin
    {
        public Vector2 Position;
        public bool Collected = false;

        private Texture2D texture;
        

        // Hitbox property
        public Hitbox Hitbox { get; private set; }

        // Width and Height properties
        public int Width => texture.Width;
        public int Height => texture.Height;

        public Coin(Texture2D texture, Vector2 position, GraphicsDevice graphicsDevice)
        {
            this.texture = texture;
            Position = position;
            Collected = false;
            Hitbox = new Hitbox(graphicsDevice, (int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        // Snap the coin to the top of the nearest platform below it
        public void SnapToPlatform(List<Platform> platforms)
        {
            // Get coin dimensions
            int coinWidth = texture.Width;
            int coinHeight = texture.Height;

            // Check each platform to see if the coin is above it
            foreach (var platform in platforms)
            {
                Rectangle tileRect = platform.Bounds;

                Rectangle coinRect = new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    coinWidth,
                    coinHeight
                );

                // Check if horizontally aligned
                bool horizontallyAligned =
                    coinRect.Right > tileRect.Left &&
                    coinRect.Left < tileRect.Right;

                if (horizontallyAligned)
                {
                    // Check if the platform is below the coin
                    if (tileRect.Top >= coinRect.Bottom)
                    {
                        // Snap the coin on top of the platform
                        Position = new Vector2(Position.X, tileRect.Top - coinHeight);
                        return; 
                    }
                }
            }
        }

        public void Update(Hitbox playerHitbox)
        {
            // Update hitbox position
            Hitbox.Update(Position);

            // Check for collision with player
            if (!Collected && Hitbox.Intersects(playerHitbox))
            {
                Collected = true;
                // TODO: Add score/sound logic here
                // Extra TODO: Consider adding a collected animation
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Collected)
            {
                spriteBatch.Draw(texture, Position, Color.White);
            }
        }
    }
}
