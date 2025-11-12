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
