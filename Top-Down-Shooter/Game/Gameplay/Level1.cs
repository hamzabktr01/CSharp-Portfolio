using System;
using System.IO;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;
using Game.Engine;

namespace Game.Gameplay
{
    //<summary>
    //Liest eine 0/1-Datei und zeichnet NUR an den Auﬂenkanten W‰nde,
    // und auch nur dort, wo am Dateirand '1' steht. Ihnen wird ignoriert.
    // </summary>

    public class Level1
    {
        private readonly string filePath;

        private Texture2D wallTexture;
        private readonly List<Wall> edgeWalls = new();

        private int mapWidth;
        private int mapHeight;
        private float tileSize;
        private float offsetX;
        private float offsetY;

        public Level1(string filePath)
        {
            this.filePath = filePath;
        }

        public void Load()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"Datei nicht gefunden: {filePath}");
                return;
            }

            var lines = File.ReadAllLines(filePath);
            mapHeight = lines.Length;
            mapWidth = lines[0].Length;

            wallTexture = Raylib.LoadTexture("Assets/texturewalls.jpg");
            if (wallTexture.Id == 0)
            {
                Console.WriteLine("Fehler: Assets/Texture_wall.png konnte nicht geladen werden.");
                return;
            }

            float tileW = (float)ScaledRenderer.VIRTUAL_WIDTH / mapWidth;
            float tileH = (float)ScaledRenderer.VIRTUAL_HEIGHT / mapHeight;
            tileSize = MathF.Min(tileW, tileH);

            float totalW = mapWidth * tileSize;
            float totalH = mapHeight * tileSize;
            offsetX = (ScaledRenderer.VIRTUAL_WIDTH - totalW) / 2f;
            offsetY = (ScaledRenderer.VIRTUAL_HEIGHT - totalH) / 2f;

            edgeWalls.Clear();

            //Obere Kante (y=0)
            for (int x = 0; x < mapWidth; x++)
                if (lines[0][x] == '1')
                    edgeWalls.Add(MakeWall(x, 0));

            //Untere Kante (y=mapHeight-1)
            for (int x = 0; x < mapWidth; x++)
                if (lines[mapHeight - 1][x] == '1')
                    edgeWalls.Add(MakeWall(x, mapHeight - 1));

            //Linke Kante (x = 0)
            for (int y = 0; y < mapHeight; y++)
                if (lines[y][0] == '1')
                    edgeWalls.Add(MakeWall(0, y));

            //Rechte Kante (x=mapWidth-1)
            for (int y = 0; y < mapHeight; y++)
                if (lines[y][mapWidth - 1] == '1')
                    edgeWalls.Add(MakeWall(mapWidth - 1, y));

        }

        private Wall MakeWall(int gridX, int gridY)
        {
            Vector2 pos = new Vector2(offsetX + gridX * tileSize, offsetY + gridY * tileSize);
            Vector2 size = new Vector2(tileSize, tileSize);
            return new Wall(pos, size, Color.White, wallTexture);
        }

       
        public void Draw()
        {
            foreach (var w in edgeWalls)
                w.DrawScaled();
        }

        public void Unload()
        {
            if (wallTexture.Id != 0) Raylib.UnloadTexture(wallTexture);
        }


    }


}