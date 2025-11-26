using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3.Level
{
    public class Door
    {
        private Texture2D doorTexture;
        private Rectangle doorRect;
        bool doorReached;

        public Door(Game1 game, Rectangle groundRect)
        {
            doorTexture = new Texture2D(game.GraphicsDevice, 1, 1);
            doorTexture.SetData(new[] { Color.DarkRed });

            doorRect = new Rectangle(groundRect.Width - 100, groundRect.Y - groundRect.Height + 30, 50, 100);
            doorReached = false;
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(doorTexture, doorRect, Color.White);
        }
    }
}
