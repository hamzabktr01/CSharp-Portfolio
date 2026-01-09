using Raylib_cs;
using System.Numerics;

namespace Game.Engine
{
    public class LightCircleOverlay
    {
        private int width;
        private int height;
        private float radius;

        public LightCircleOverlay(int width, int height, float radius)
        {
            this.width = width;
            this.height = height;
            this.radius = radius;
        }

        public void Draw(Vector2 playerPos)
        {

            float cx = playerPos.X;
            float cy = playerPos.Y;

            float r = radius;
            float r2 = r * r;

            //1) oben dunkel

            float topH = cy - r;
            if (topH > 0)
                ScaledRenderer.DrawRect(0, 0, width, topH, new Color(0, 0, 0, 240));

            //2) unten dunkel

            float bottomY = cy + r;

            if (bottomY < height)
                ScaledRenderer.DrawRect(0, bottomY, width, height - bottomY, new Color(0, 0, 0, 240));

            //3)Mitte: pro Scanline links/rechts dunkel (Kreis bleibt frei)
            int yStart = (int)MathF.Max(0, cy - r);
            int yEnd = (int)MathF.Min(height - 1, cy + r);

            for (int y = yStart; y <= yEnd; y++)
            {
                float dy = y - cy;
                float x = MathF.Sqrt(MathF.Max(0, r2 - dy * dy));

                float leftW = cx - x;
                float rightX = cx + x;

                //links dunkel
                if (leftW > 0)
                    ScaledRenderer.DrawRect(0, y, leftW, 1, new Color(0, 0, 0, 240));

                //rechts dunkel
                if (rightX < width)
                    ScaledRenderer.DrawRect(rightX, y, width - rightX, 1, new Color(0, 0, 0, 240));
            }

        }
    }
}
