using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Game.Engine;

namespace Game.Gameplay
{
    public class Obstacle
    {
        public Vector2 Position;
        public Vector2 Size;
        public Texture2D Texture;
        public Color Color;
        public Rectangle Rect {get{return new Rectangle(Position.X, Position.Y,Size.X,Size.Y);}}
        public Obstacle(Vector2 pos, Vector2 size, Texture2D texture, Color? color = null)
        {
            Position = pos;
            Size = size;
            Texture = texture;
            Color = color ?? Color.White;
        }
        
        public void DrawScaled()
        {
            //ScaledRenderer.DrawTextureScaled(Texture, Position, Size, Color.White);
            ScaledRenderer.DrawScaled(Texture, Position.X, Position.Y, Size.X, Size.Y, Color.White);
        }
    }
}