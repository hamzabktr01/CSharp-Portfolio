/*using System.IO;
using Raylib_cs;
using Level2.Assets;
using System.Numerics;
using Color = Raylib_cs.Color;

namespace Game.Engine
{
    public class MapLevel2
    {
        private int[,] map;
        private readonly int titleSize;
        private int width;
        private int height;

        public MapLevel2(int titleSize)
        {
            titleSize = tileSize;
            LoadMapFromFile();
        }

        private void LoadMapFromFile()
        {
            string path = path.Combine("assets", "level2.txt");

            if (!File.Exists(path))
            {
                //Wenn Datei fehlt: einfache Wandmap, damit es nicht crasht
                width = 20;
                height = 15;
                map = new int[height, width];
                for (int r = 0; r < height; r++)
                    for (int c = 0; c < width; c++)
                        map[r, c] = 1;
                return;
            }

            string[] lines = File.ReadAllLines(path);
            height = lines.Length;
            width = lines[0].Length;

            map = new int[height, width];

            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < width; c++)
                {
                    char ch = lines[r][c];
                    map[r, c] = (ch == '1') ? 1 : 0;
                }
            }
        }

        public void Draw()
        {
            Color black = new Color(0, 0, 0, 255);
            Color white = new Color(255, 255, 255, 255);

            for (int r = 0; r < height; r++)
            {
                for (int c = 0; c < _width; c++)
                {
                    int x = c * tileSize;
                    int y = r * tileSize;

                    if (map[r, c] == 1)
                    {
                        //Wand: Textur zeichnen
                        Raylib.DrawTextureEx(
                            TextureManager.WallTexture,
                            new Vector2(x, y),
                            0f,
                            (float)_tileSize / TextureManager.WallTexture.Width,
                            white
                        );

                    }
                    else
                    {
                        //Weg: schwarz
                        Raylib.DrawRectangle(x, y, tileSize, tileSize, black);
                    }
                }
            }
        }
    }
}*/