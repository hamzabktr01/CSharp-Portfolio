using Raylib_cs;
using System.Numerics;
using Game.Engine;

namespace Game.Gameplay
{
    public class StickyTile
    {
        public Vector2 Position;
        public Vector2 Size;

        //Optional: Texture, falls ich später eine möchte
        public Texture2D Texture;
        public bool HasTexture => Texture.Id != 0;

        //1.0 = normal, 0.5 = halb so schnell usw.
        public float SlowMultiplier { get; set; } = 0.5f;

        public Rectangle Rect => new Rectangle(Position.X,Position.Y,Size.X,Size.Y);

        public StickyTile(Vector2 pos, Vector2 size, float slowMultiplier = 0.5f, Texture2D texture = default)
        {
            Position = pos;
            Size = size;
            SlowMultiplier = slowMultiplier;
            Texture = texture;
        }

        public void Draw()
        {
            if(HasTexture)
            {
                ScaledRenderer.DrawScaled(Texture, Position.X,Position.Y,Size.X, Size.Y,Color.White);
            }
            else
            {
                //ohne Texture: gut sichtbar (z.b lila)
                ScaledRenderer.DrawRect(Position.X, Position.Y, Size.X, Size.Y, new Color(120, 60, 180, 200));
            }
        }
    }

}