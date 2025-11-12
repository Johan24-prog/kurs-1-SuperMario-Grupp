/*using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SuperMarioH3.MoveableObjects
{
    public class Example
    {
        
        public Vector2 Position;
        public Vector2 Velocity;
        public Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

        const int Width = 16;
        const int Height = 16;

        const float MoveSpeed = 80f;
        const float MaxSpeedX = 120f;
        const float Gravity = 900f;
        const float JumpVelocity = -250f;
        const float MaxFall = 400f;
        const float GroundFriction = 0.8f;

        Texture2D sprite;
        TileMap map;

        AnimatedSprite anim;

        bool onGround = false;
        public Example(Texture2D sprite, TileMap map, Vector2 startPosition)
        {
            this.sprite = sprite;
            this.map = map;
            Position = startPosition;

            anim = new AnimatedSprite(sprite, 16, 16);
            anim.AddAnimation("Idle", 0, 1, 0,15f);
            anim.AddAnimation("Run", 1, 4, 0.09f);
            anim.AddAnimation("Jump", 5, 1, 0,10f);
            anim.Play("Idle");
        }
        public void Update(float dt)
        {
            var kb = Keyboard.GetState();
            float move = 0f;
            if (kb.IsKeyDown(Keys.Left) || kb.IsKeyDown(Keys.A)) move -= 1f;
            if (kb.IsKeyDown(Keys.Right) || kb.IsKeyDown(Keys.D)) move += 1f;

            // Horizontal movement
            Velocity.X += move * MoveSpeed * dt;

            // Clamp horizontal
            if (Velocity.X > MaxSpeedX) Velocity.X = MaxSpeedX;
            if (Velocity.X < -MaxSpeedX) Velocity.X = -MaxSpeedX;

            // Jumping
            if ((kb.IsKeyDown(Keys.Space) || kb.IsKeyDown(Keys.Up)) && onGround)
            {
                Velocity.Y = JumpVelocity;
                onGround = false;
            }

            // Gravity
            Velocity.Y += Gravity * dt;
            if (Velocity.Y > MaxFall) Velocity.Y = MaxFall;

            // Apply velocity
            Vector2 newPos = Position + Velocity * dt;

            // axis-aligned collision resolution: X then Y
            Position = ResolveCollisionsX(Position, newPos);
            Position = ResolveCollisionsY(Position, newPos);

            // Friction
            if (onGround)
            {
                Velocity.X *= GroundFriction;
                if (Math.Abs(Velocity.X) < 5f) anim.Play("Run");
                else anim.Play("Idle");
                anim.Flip = Velocity.X < -1f;
                anim.Update(dt);
            }
            Rectangle ExpandRect(Rectangle r, int pad) => new Rectangle(r.X - pad, r.Y - pad, r.Width + pad * 2, r.Height + pad * 2);

        private Vector2 ResolveCollisionsX(Vector2 oldPos, Vector2 attemptedPos)
        {
            Vector2 pos = oldPos;
            pos.X = attemptedPos.X;

            Rectangle bounds = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            // check tiles overlapping bounds
            var tiles = OverlappingTiles(bounds);
            foreach (var t in tiles)
            {
                if (!map.IsSolidTileAt(t.X, t.Y)) continue;
                Rectangle tileRect = new Rectangle(t.X * map.TileWidth, t.Y * map.TileHeight, map.TileWidth, map.TileHeight);
                if (bounds.Intersects(tileRect))
                {
                    if (Velocity.X > 0)
                    {
                        pos.X = tileRect.Left - Width;
                        Velocity.X = 0;
                    }
                    else if (Velocity.X < 0)
                    {
                        pos.X = tileRect.Right;
                        Velocity.X = 0;
                    }
                    bounds.X = (int)pos.X;
                }
            }
            return pos;
        }

        private Vector2 ResolveCollisionsY(Vector2 oldPos, Vector2 attemptedPos)
        {
            Vector2 pos = oldPos;
            pos.Y = attemptedPos.Y;

            Rectangle bounds = new Rectangle((int)pos.X, (int)pos.Y, Width, Height);
            var tiles = OverlappingTiles(bounds);
            onGround = false;
            foreach (var t in tiles)
            {
                if (!map.IsSolidTileAt(t.X, t.Y)) continue;
                Rectangle tileRect = new Rectangle(t.X * map.TileWidth, t.Y * map.TileHeight, map.TileWidth, map.TileHeight);
                if (bounds.Intersects(tileRect))
                {
                    if (Velocity.Y > 0)
                    {
                        pos.Y = tileRect.Top - Height;
                        Velocity.Y = 0;
                        onGround = true;
                    }
                    else if (Velocity.Y < 0)
                    {
                        pos.Y = tileRect.Bottom;
                        Velocity.Y = 0;
                    }
                    bounds.Y = (int)pos.Y;
                }
            }
            return pos;
        }

        // return list of tile coords overlapping bbox (small list)
        private List<Point> OverlappingTiles(Rectangle bbox)
        {
            int left = bbox.Left / map.TileWidth;
            int right = (bbox.Right - 1) / map.TileWidth;
            int top = bbox.Top / map.TileHeight;
            int bottom = (bbox.Bottom - 1) / map.TileHeight;

            var list = new List<Point>();
            for (int y = top; y <= bottom; y++)
                for (int x = left; x <= right; x++)
                    list.Add(new Point(x, y));
            return list;
        }

        public void Draw(SpriteBatch sb)
        {
            anim.Draw(sb, Position);
        }
    }

    // small helper for math absolute
    public static class Mathf
    {
        public static float Abs(float v) => (v < 0) ? -v : v;
    }
}

// Exaple usage:    
*/
        
