using Microsoft.Xna.Framework;

namespace SuperMarioH3.MoveableObjects
{
    public class VectorPosition
    {
        // Position and Velocity as Vector2 for easier manipulation
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        // Constructor to initialize position and velocity
        public VectorPosition(float x = 0, float y = 0)
        {
            Position = new Vector2(x, y);
            Velocity = Vector2.Zero;
        }

        // Apply gravity to the vertical component of velocity
        public void ApplyGravity(float gravity, float deltaTime)
        {
            var v = Velocity;
            v.Y += gravity * deltaTime; // Gravity affects only Y component
            Velocity = v;
        }

        // Update position based on velocity and deltaTime
        public void Update(float deltaTime)
        {
            Position += Velocity * deltaTime;  
        }

        // Clamp position within screen bounds using Move limiter
        public void ClampToScreen(Move moveLimiter, Vector2 size)
        {
            var pos = Position;
            moveLimiter.Clamp(ref pos, size);
            Position = pos;
        }
    }
}
