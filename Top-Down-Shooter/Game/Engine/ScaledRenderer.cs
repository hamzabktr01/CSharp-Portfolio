using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using Raylib_cs;
namespace Game
{
    public static class ScaledRenderer
    {
        // Base Resolution
        public const int VIRTUAL_WIDTH = 800;
        public const int VIRTUAL_HEIGHT = 600;

        // Will be calculated internally
        public static float scale;
        public static float offsetX;
        public static float offsetY;
       public static bool hasBegun = false;

        // Calculates the scaling in every frame. Has to be BEFORE "DrawScaled()"!!!
        public static void Begin()
        {
            float windowWidth = Raylib_cs.Raylib.GetScreenWidth();
            float windowHeight = Raylib_cs.Raylib.GetScreenHeight();

            scale = MathF.Min(windowWidth / VIRTUAL_WIDTH, windowHeight / VIRTUAL_HEIGHT);
            offsetX = (windowWidth - VIRTUAL_WIDTH * scale) / 2f;
            offsetY = (windowHeight - VIRTUAL_HEIGHT * scale) / 2f;

            hasBegun = true;
        }

        // DrawScaled draws and and scales automatically all textures. Begin() HAS TO BE USED BEFOREHAND!!!

        public static void DrawScaled(Texture2D texture, float x, float y, float width, float height, Color? tint = null)
        {
            if (!hasBegun) throw new InvalidOperationException("Begin() hast to be called before DrawScaled()");

            // Source part that shall be drawn
            var source = new Raylib_cs.Rectangle(
            0f, 0f,
            texture.Width, texture.Height);

            // destination where and how big should the texture be?
            var destination = new Raylib_cs.Rectangle(
            offsetX + x * scale,
            offsetY + y * scale,
            width * scale,
            height * scale);
            // originpoint on the top left
            Vector2 origin = new Vector2(0f, 0f);

            // drawing of the scaled TExture
            Raylib.DrawTexturePro(texture, source, destination, origin, 0f, tint ?? Color.White);

        }
        public static void DrawScaled(Texture2D texture, Rectangle source, float x, float y, float width, float height, Color? tint = null)
        {
            if (!hasBegun) throw new InvalidOperationException("Begin() has to be called before DrawScaled(Texture, Rectangle,...)");

            Rectangle destination = new Rectangle(
                offsetX + x * scale,
                offsetY + y * scale,
                width * scale,
                height * scale);
            Vector2 origin = new Vector2(0, 0);

            Raylib.DrawTexturePro(texture, source, destination, origin, 0f, tint ?? Color.White);
        }

        public static void DrawRect(float x, float y, float width, float height, Color color)
        {
            if (!hasBegun) throw new InvalidOperationException("Begin() hast to be called before DrawRect()");

            Raylib.DrawRectangleRec(
                new Raylib_cs.Rectangle(
                    new Vector2(offsetX + x * scale, offsetY + y * scale),
                    new Vector2(width * scale, height * scale)
                ),
                color
            );
        }

        public static void DrawBounds(Color color)
        {
            if (!hasBegun) throw new InvalidOperationException("Begin() hast to be called before DrawBounds()");

            float scaledX = offsetX - 0.5f;
            float scaledY = offsetY - 0.5f;
            float scaledWidth = VIRTUAL_WIDTH * scale + 1;
            float scaledHight = VIRTUAL_HEIGHT * scale + 1;

            Raylib.DrawRectangleLinesEx(
                new Rectangle(
                (int)scaledX,
                (int)scaledY,
                (int)scaledWidth,
                (int)scaledHight
                ),
                2f,
                color


            );
        }

        //HAS to be called on the end of every Frame

        public static void End()
        {
            hasBegun = false;
        }


    }
}


