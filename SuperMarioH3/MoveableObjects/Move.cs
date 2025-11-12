using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioH3.MoveableObjects
{
    internal class Move
    {
        // The width of the game screen
        private readonly int ScreenWidth;       

        internal Move(int screenWidth)
        {
            ScreenWidth = screenWidth;
        }
        // Prevents the object from moving outside the screen bounds
        internal void Clamp(ref Vector2 position, Vector2 size)
        {
            // Stop at left edge
            if (position.X < 0)
                position.X = 0;
            // Stop at right edge
            if (position.X + size.X > ScreenWidth)
                position.X = ScreenWidth - size.X;

        }
    }
}
