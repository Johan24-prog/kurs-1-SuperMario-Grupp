using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3
{
    public class Hitbox
    {
        // Rectangle representing the hitbox area
        public Rectangle Bounds { get; private set; }

        private Color _debugColor = Color.Red;
        private Texture2D _debugTexture;

        // Constructor to initialize hitbox with position and size
        public Hitbox(GraphicsDevice graphicsDevice, int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);

            // Create a 1x1 texture for debugging visuals
            _debugTexture = new Texture2D(graphicsDevice, 1, 1);
            _debugTexture.SetData(new[] { Color.White });
        }

        // Update hitbox position
        public void Update(Vector2 position)
        {
            Bounds = new Rectangle((int)position.X, (int)position.Y, Bounds.Width, Bounds.Height);
        }
        // Check for intersection with another hitbox
        public bool Intersects(Hitbox other)
        {
            return Bounds.Intersects(other.Bounds);
        }
        // Draw the hitbox for debugging purposes
        public void Draw(SpriteBatch spriteBatch, float alpha = 0.3f)
        {
            spriteBatch.Draw(_debugTexture, Bounds, _debugColor * alpha);
        }
    }
}
