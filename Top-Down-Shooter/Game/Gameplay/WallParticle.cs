using Raylib_cs;
using System;
using System.Numerics;


namespace Game.Gameplay
{
    public class WallParticle
    {
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Size;
        public Texture2D Texture;
        public Rectangle SourceRect;

        private float Lifetime = 1f;
        private float age = 0f;
        public bool IsAlive => age < Lifetime;

        public WallParticle(Vector2 startPos, Vector2 velocity, Texture2D texture, Rectangle sourceRect, Vector2 size)
        {
            Position = startPos;
            Velocity = velocity;
            Texture = texture;
            SourceRect = sourceRect;
            Size = size;
        }
        public void Update(float dt)
        {
            age += dt;
            Position += Velocity * dt;
            Velocity.Y += 200 * dt;
        }

        public void Draw()
        {
            if (!IsAlive) return;
            ScaledRenderer.DrawScaled(Texture,SourceRect,Position.X, Position.Y, Size.X, Size.Y, Color.Gray);
        }
    }
}
