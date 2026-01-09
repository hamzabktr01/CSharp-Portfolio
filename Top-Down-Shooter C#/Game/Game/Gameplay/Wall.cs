using System.Numerics;
using Raylib_cs;
using Game.Engine;

namespace Game.Gameplay
{
    public class Wall
    {
        public Vector2 Position;
        public Vector2 Size;
        public Color Tint;
        public Texture2D Texture;
        public Rectangle Rect { get { return new Rectangle(Position.X, Position.Y, Size.X, Size.Y); } }


        public Wall(Vector2 position, Vector2 size, Color tint, Texture2D texture)
        {
            Position = position;
            Size = size;
            Tint = tint;
            Texture = texture;
        }   
     
        public void DrawScaled()
        {
            //ScaledRenderer.DrawTextureScaled(Texture, Position, Size, Tint);
            ScaledRenderer.DrawScaled(Texture, Position.X, Position.Y, Size.X, Size.Y, Color.White);
        }

    }
}
