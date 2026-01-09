/*
 * using System.IO;
using Raylib_cs;
using RImage = Raylib_cs.Image;
using Color = Raylib_cs.Color;

namespace Game.Assets
{
    public static class TextureManager
    {
        public static Texture2D WallTexture;

        public static void LoadTextures()
        {
            string path = Path.Combine("assets", "wall2.png");

            if(File.Exists(path))
            {
                //echte Textur laden
                WallTexture = RayLib.LoadTexture(path);
            }
            else
            {
                //Fallback-Textur (blaues Kästchen), falls wall2.png fehlt
                Color blue = new Color(0, 121, 241, 255);   //beliebiges Blau
                RImage img = Raylib.GenImageColor(40, 40, blue);
                WallTexture = Raylib.LoadTextureFromImage(img);
                Raylib.UnloadImage(img);

            }
        }

        public static void UnloadTextures()
        {
            //einfache Sicherheitsabfrage
            if(WallTexture.Width > 0 && WallTexture.Height > 0)
            {
                Raylib.UnloadTexture(WallTexture);
            }
        }
    }
}*/