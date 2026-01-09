
using Raylib_cs;
using Game.Engine;
using System.Numerics;


namespace Game.Gameplay
{
    public class Walls
    {
        public Vector2 Position;
        public Vector2 Size;
        public Color Color;
        public Texture2D Texture;


        public Walls(Vector2 pos, Vector2 size, Color color, Texture2D texture = default)
        {
            Position = pos;
            Size = size;
            Color = color;
            Texture = texture;

            
        }

        public void DrawScaled()
        {
            //ScaledRenderer.DrawTextureScaled(Texture, Position, Size, Color.White);
            ScaledRenderer.DrawScaled(Texture, Position.X, Position.Y, Size.X, Size.Y, Color.White);
        }
       
    }
}