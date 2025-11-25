using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SuperMarioH3
{
    public class Enemy
    {
        private Texture2D texture;
        private Vector2 position;
        private Vector2 velocity;
        private Rectangle hitbox;

        private int direction = -1; // -1 = left, 1 = right
        private const float Gravity = 900f;
        private const float MoveSpeed = 80f;
        private const float MaxFallSpeed = 1200f;
        private float deathTimer = 0f;

        private bool isOnGround;
        private bool isChasing = false;
        private float chaseRange = 300f;     // How close the player needs to be to start chasing
        private float stopChaseRange = 450f; // How far the player needs to be to stop chasing


        public bool IsDead { get; private set; } = false;
        public Rectangle Hitbox => hitbox;

        public Enemy(Texture2D texture, Vector2 startPosition)
        {
            this.texture = texture;
            position = startPosition;
            hitbox = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime, List<Platform> platforms, Rectangle groundRect, Rectangle worldBounds)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsDead)
            {
                deathTimer += dt;
                return;
            }

            // Horizontal movement
            velocity.X = MoveSpeed * direction;

            // Apply gravity
            velocity.Y += Gravity * dt;
            if (velocity.Y > MaxFallSpeed)
                velocity.Y = MaxFallSpeed;

            // Predict new position
            Vector2 newPosition = position + velocity * dt;
            Rectangle newHitbox = new Rectangle((int)newPosition.X, (int)newPosition.Y, hitbox.Width, hitbox.Height);

            isOnGround = false;
            bool hitWall = false;

            // PLATFORM COLLISION
            foreach (var plat in platforms)
            {
                if (!newHitbox.Intersects(plat.Bounds))
                    continue;

                // LANDING ON PLATFORM
                if (hitbox.Bottom <= plat.Bounds.Top && newHitbox.Bottom >= plat.Bounds.Top)
                {
                    newPosition.Y = plat.Bounds.Top - hitbox.Height;
                    velocity.Y = 0;
                    isOnGround = true;
                }
                else
                {
                    hitWall = true; // side collision
                }
            }

            // GROUND COLLISION
            if (newPosition.Y + hitbox.Height >= groundRect.Top)
            {
                newPosition.Y = groundRect.Top - hitbox.Height;
                velocity.Y = 0;
                isOnGround = true;
            }

            // TURN AROUND IF HIT WALL OR AT WORLD EDGE
            if (hitWall && isOnGround)
                direction *= -1;

            // World bounds check: if at left or right edge, turn around
            if (newPosition.X <= worldBounds.Left)
            {
                newPosition.X = worldBounds.Left;
                direction = 1;
            }
            else if (newPosition.X + hitbox.Width >= worldBounds.Right)
            {
                newPosition.X = worldBounds.Right - hitbox.Width;
                direction = -1;
            }

            // Apply final position
            position = newPosition;
            hitbox.X = (int)position.X;
            hitbox.Y = (int)position.Y;
        }

        public void UpdateAI(Vector2 playerPos)
        {
            if (IsDead)
                return;

            float distance = Vector2.Distance(playerPos, position);

            // Start chasing if player is within range
            if (!isChasing && distance < chaseRange)
                isChasing = true;

            // Stop chasing if player is far away
            if (isChasing && distance > stopChaseRange)
                isChasing = false;

            if (isChasing)
            {
                // Follow the player
                if (playerPos.X < position.X)
                    direction = -1;
                else if (playerPos.X > position.X)
                    direction = 1;
            }

            // If not chasing, continue patrolling
        }



        public void Kill() => IsDead = true;

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                spriteBatch.Draw(texture, position, Color.White);
        }
        public bool ReadyToRemove => IsDead && deathTimer > 0.4f;
    }
}
// TODO: Add aggressive behaviors for enemies, such as chasing the player.
// TODO: Make the enemy be able to patrol between two points.
// TODO: Make the enemy be able to jump over obstacles, platforms, etc.
// -------------------------- OPTIONAL --------------------------
// TODO: Implement animation frames for enemy movement and death.
// TODO: Integrate enemy AI for more complex behaviors.
// TODO: Add different enemy types with unique behaviors.
// TODO: Implement sound effects for enemy actions.
// TODO: Optimize collision detection for multiple enemies.
// TODO: Add particle effects on enemy death.
// TODO: Integrate enemy AI for more complex behaviors.