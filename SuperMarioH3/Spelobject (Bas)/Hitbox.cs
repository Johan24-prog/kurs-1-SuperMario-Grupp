using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3
{
    public class Hitbox
    {
        public Rectangle Bounds { get; private set; }

        private Color _debugColor = Color.Red;
        private Texture2D _debugTexture;

        public Hitbox(GraphicsDevice graphicsDevice, int x, int y, int width, int height)
        {
            Bounds = new Rectangle(x, y, width, height);

            // Create a 1x1 texture for debugging visuals
            _debugTexture = new Texture2D(graphicsDevice, 1, 1);
            _debugTexture.SetData(new[] { Color.White });
        }

        public void Update(Vector2 position)
        {
            Bounds = new Rectangle((int)position.X, (int)position.Y, Bounds.Width, Bounds.Height);
        }

        public bool Intersects(Hitbox other)
        {
            return Bounds.Intersects(other.Bounds);
        }

        public void Draw(SpriteBatch spriteBatch, float alpha = 0.3f)
        {
            spriteBatch.Draw(_debugTexture, Bounds, _debugColor * alpha);
        }
    }
}
