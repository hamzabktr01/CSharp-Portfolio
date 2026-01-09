using System.Numerics;
using Raylib_cs;
using Game.Engine;
using static Raylib_cs.Raylib;

namespace Game.Gameplay
{
    public class FlameWallTile
    {
        public Vector2 Position;
        public Vector2 Size;

        private StripSheetAnimation _anim;

        public Rectangle Rect =>
            new Rectangle(Position.X,Position.Y,Size.X,Size.Y);

        public FlameWallTile(Vector2 pos, Vector2 size)
        {
            Position = pos;
            Size = size;

            _anim = TextureManager.Instance.CreateInstance(
                key: "FlameWall",
                fps: 6,
                loop: true
                );
        }

        public void Update(float  dt)
        {
            _anim.Update(dt);
        }

        public void Draw()
        {
            ScaledRenderer.DrawScaled(
                _anim.Texture,
                _anim.Source,
                Position.X,
                Position.Y,
                Size.X,
                Size.Y,
                Color.White
                );
        }
    }
}
