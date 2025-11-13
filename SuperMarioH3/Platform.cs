using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioH3
{
    public class Platform
    {
        // Platformens rektangel (position och storlek) och textur
        private Rectangle _bounds;
        private Texture2D _texture;

        // Få tillgång till plattformens rektangel
        public Rectangle Bounds => _bounds;

        // Skapa en ny plattform med en textur och position/storlek
        public Platform(Texture2D texture, Rectangle bounds)
        {
            _texture = texture; // Texturen som ritas
            _bounds = bounds; // Rektangeln som beskriver plattformens position och storlek
        }

        // Rita plattformen
        public void Draw(SpriteBatch spriteBatch)
        {
            // Rita plattformens textur inom dess rektangel
            spriteBatch.Draw(_texture, _bounds, Color.White);
        }

        // Kolla kollision mellan spelaren och plattformen
        public bool CheckCollision(Rectangle playerHitbox, ref Vector2 playerPosition, ref Vector2 playerVelocity)
        {
            // Kolla om spelarens hitbox kolliderar med plattformens rektangel
            if (playerHitbox.Bottom > _bounds.Top &&
                playerHitbox.Top < _bounds.Bottom &&
                playerHitbox.Right > _bounds.Left &&
                playerHitbox.Left < _bounds.Right)
            {
                // Spelaren landar på plattformen
                playerPosition.Y = _bounds.Top - playerHitbox.Height;
                playerVelocity.Y = 0; // Stoppar spelarens fall
                return true; // Kollision inträffade
            }
            return false; // Ingen kollision
        }
    }
}
