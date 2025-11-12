using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMarioH3.Spelobject__Bas_;

namespace SuperMarioH3
{
    public class Coin
    {
        public Vector2 Position;
        public bool Collected = false;

        private Texture2D texture;
        private Rectangle bounds;

        // Hitbox property
        private Rectangle hitbox;
        public Rectangle Hitbox => hitbox;

        // Width and Height properties
        public int Width => texture.Width;
        public int Height => texture.Height;

        public Coin(Texture2D texture, Vector2 position)
        {
            this.texture = texture;
            Position = position;
            Collected = false;
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        }

        public void Update(Rectangle playerRect)
        {
            // Keep bounds in sync with Position in case Position ever changes
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            // Update hitbox
            hitbox = new Rectangle((int)Position.X, (int)Position.Y, texture.Width, texture.Height); 

            if (!Collected && bounds.Intersects(playerRect))
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
