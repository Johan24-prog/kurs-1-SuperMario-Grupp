using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3
{
    public class Coin
    {
        public Vector2 Position;
        public bool Collected = false;

        private Texture2D texture;
        private Rectangle bounds;

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
            // keep bounds in sync with Position in case Position ever changes
            bounds = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            if (!Collected && bounds.Intersects(playerRect))
            {
                Collected = true;
                // add score/sound logic here
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
