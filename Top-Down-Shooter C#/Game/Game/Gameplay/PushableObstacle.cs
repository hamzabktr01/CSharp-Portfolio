using System.Numerics;
using Raylib_cs;
using Game.Engine;

namespace Game.Gameplay
{
    public class PushableObstacle
    {
        public Vector2 Position;
        public Vector2 Size;
        public Texture2D Texture;


        public Rectangle Rect =>
            new Rectangle(Position.X,Position.Y,Size.X,Size.Y);

        public PushableObstacle(Vector2 pos, Vector2 size,Texture2D texture)
        {
            Position = pos;
            Size = size;
            Texture = texture;
        }

       


        public void Draw()
        {
            ScaledRenderer.DrawScaled(
                Texture,
                Position.X,
                Position.Y,
                Size.X,
                Size.Y,
                Color.White
                );
        }
    }
}
