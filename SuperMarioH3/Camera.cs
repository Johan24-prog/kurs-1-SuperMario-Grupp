using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioH3
{
    public class Camera2D
    {
        // Transformation matrix for the camera
        public Matrix Transform { get; private set; }

        // Camera properties
        private Viewport _viewport;
        private Vector2 _position;
        private float _zoom;
        private float _rotation;

        // Optional world bounds for clamping
        private Rectangle? _bounds;

        // Constructor
        public Camera2D(Viewport viewport)
        {
            _viewport = viewport; // Store the viewport
            _zoom = 1.0f; // Default zoom level
            _rotation = 0f; // Default rotation
            _position = Vector2.Zero; // Default position
        }

        // Set world bounds for the camera
        public void SetBounds(Rectangle bounds) 
        {
            _bounds = bounds; // Store the bounds
        }

        // Follow a target position
        public void Follow(Vector2 target)
        {
            // Center camera on target
            _position = target - new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);

            // Clamp camera to world bounds if they exist
            if (_bounds.HasValue)
            {
                // Clamp position within bounds
                var b = _bounds.Value;
                _position.X = MathHelper.Clamp(_position.X, b.Left, b.Right - _viewport.Width);
                _position.Y = MathHelper.Clamp(_position.Y, b.Top, b.Bottom - _viewport.Height);
            }

            // Create transformation matrix
            Transform = Matrix.CreateTranslation(new Vector3(-_position, 0f)) *
                        Matrix.CreateRotationZ(_rotation) *
                        Matrix.CreateScale(_zoom, _zoom, 1f);
        }

        // Zoom in or out
        public void Zoom(float amount)
        {
            _zoom = MathHelper.Clamp(_zoom + amount, 0.5f, 3f); // Clamp zoom level
        }

        // Rotate the camera
        public void Rotate(float amount)
        {
            _rotation += amount; // Update rotation
        }
    }
}
