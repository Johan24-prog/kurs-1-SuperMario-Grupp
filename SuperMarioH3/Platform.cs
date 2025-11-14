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
        // Returnerar true om en kollision sker och justerar spelarens position och hastighet
        // Adjusted this part to fix the player snapping to the platform and getting stuck issue - Johan
        public bool CheckCollision(Rectangle playerHitbox, Rectangle playerOldHitbox,
                           ref Vector2 playerPosition, ref Vector2 playerVelocity)
        {
            // Only when falling downward
            if (playerVelocity.Y >= 0)
            {
                // Check if the player was above the platform in the previous frame
                bool wasAbove = playerOldHitbox.Bottom <= _bounds.Top;

                // Check if the player intersects the top of the platform
                bool intersectsTop =
                    playerHitbox.Bottom >= _bounds.Top &&
                    playerHitbox.Top < _bounds.Bottom &&
                    playerHitbox.Right > _bounds.Left &&
                    playerHitbox.Left < _bounds.Right;

                // If both conditions are met, snap the player to the top of the platform
                if (wasAbove && intersectsTop)
                {
                    // Snap player to the top of the platform
                    playerPosition.Y = _bounds.Top - playerHitbox.Height;
                    playerVelocity.Y = 0;
                    return true;
                }
            }
            // No collision detected
            return false;
        }
    }
}
