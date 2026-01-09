using System.Numerics;
using Raylib_cs;
using Game.Engine;
using System.Collections.Generic;

namespace Game.Gameplay
{
    public class DestructibleWall
    {
        public Vector2 Position;
        public Vector2 Size;
        public Texture2D Texture;
        public bool IsDestroyed => destroyFinished;

        private bool destroyStarted = false;
        private bool destroyFinished = false;
        public bool IsCollidable { get; private set; } = true;

        private List<WallParticle> particles = new();

        public DestructibleWall(Vector2 pos, Vector2 size, Texture2D texture)
        {
            Position = pos;
            Size = size;
            Texture = texture;
        }
        public void Destroy()
        {
            if (!IsCollidable) return;
            destroyStarted = true;

            Vector2 halfSize = Size / 2f;

            particles.Add(new WallParticle(Position, new Vector2(-150, -200), Texture, new Rectangle(0, 0, Texture.Width / 2f, Texture.Height / 2f), halfSize));
            particles.Add(new WallParticle(Position + new Vector2(halfSize.X, 0), new Vector2(150, -200), Texture, new Rectangle(Texture.Width / 2f, 0, Texture.Width / 2f, Texture.Height / 2f), halfSize));
            particles.Add(new WallParticle(Position + new Vector2(0, halfSize.Y), new Vector2(-150, -100), Texture, new Rectangle(0, Texture.Height / 2f, Texture.Width / 2f, Texture.Height / 2f), halfSize));
            particles.Add(new WallParticle(Position + halfSize, new Vector2(150, -100), Texture, new Rectangle(Texture.Width / 2f, Texture.Height / 2f, Texture.Width / 2f, Texture.Height / 2f), halfSize));
            IsCollidable = false;
        }

        public void Update(float delaTime)
        {
            if (!destroyStarted) return;

            bool allDead = true;
            foreach (var piece in particles)
            {
                piece.Update(delaTime);
                if (piece.IsAlive) allDead = false;
            }
        }
        public void DrawScaled()
        {
            if (!destroyStarted)
            {
                ScaledRenderer.DrawScaled(Texture, new Rectangle(0,0, Texture.Width, Texture.Height),Position.X, Position.Y, Size.X, Size.Y, Color.Gray);
            }
            else
            {
                foreach (var p in particles)
                    p.Draw();
            }
        }
        public bool CheckHit(Vector2 point)
        {
            return !IsDestroyed &&
                point.X >= Position.X && point.X <= Position.X + Size.X &&
                point.Y >= Position.Y && point.Y <= Position.Y + Size.Y;
        }



    }
}
