using System.Numerics;
using Raylib_cs;
using Game.Engine;


namespace Game.Gameplay
{
    public class Level
    {
        private string levelPath;
        private Texture2D walltexture;
        private Texture2D boxTexture;
        public bool hasRotated = false;
        public float rotationTimer = 0f;

        public List<Wall> walls = new List<Wall>();
        public List<Obstacle> obstacles = new List<Obstacle>();
        public List<DestructibleWall> destructibleWalls = new List<DestructibleWall>();
        public List<PushableObstacle> pushables = new List<PushableObstacle>();
        public List<List<GroupWall>> rotatingGroups = new List<List<GroupWall>>();
        public List<StickyTile> stickyTiles = new List<StickyTile>();
        public List<FlameWallTile> flameWalls = new();
        

        private float rotationInterval = 10f;
        

        public bool Isloaded
        {
            get; private set;
        }

        public Level(string path)
        {
            levelPath = path;
            load();

        }

        private void load()
        {
            
            if (!File.Exists(levelPath))
            {
                Console.WriteLine($"Level 1 could not be found {levelPath}");
                Isloaded = false;
                return;

            }

            string[] lines = File.ReadAllLines(levelPath);
            int mapWidth = lines[0].Length; //the number of horizontal cells, based on the length of the first row
            int mapHeigth = lines.Length; //the number of vertical cells, based on the number of rows

            walltexture = Raylib.LoadTexture("../../../Assets/texturewalls.jpg");
            boxTexture = Raylib.LoadTexture("../../../Assets/Box.png");

            if(walltexture.Id == 0 || boxTexture.Id == 0)
            {
                Console.WriteLine("Error loading textures!");
                return;
            }

            // calculate tile size to fit virtual screen
            float tileWidth = (float)ScaledRenderer.VIRTUAL_WIDTH / mapWidth;
            float tileHeight = (float)ScaledRenderer.VIRTUAL_HEIGHT / mapHeigth;
            float tileSize = MathF.Min(tileWidth, tileHeight);

            // Calculate offsets to center the level
            float totalLevelWidth = mapWidth * tileSize;
            float totalLevelHeight = mapHeigth * tileSize;
            float offsetX = (ScaledRenderer.VIRTUAL_WIDTH - totalLevelWidth) / 2f;
            float offsetY = (ScaledRenderer.VIRTUAL_HEIGHT - totalLevelHeight) / 2f;


            for(int y = 0; y < mapHeigth; y++)
            {
                for(int x = 0; x < mapWidth; x++)
                {
                    if (x >= lines[y].Length) continue;

                    Vector2 pos = new Vector2(offsetX + x * tileSize, offsetY + y * tileSize);

                    Vector2 size = new Vector2(tileSize, tileSize);
                    char tile = lines[y][x];

                    Rectangle rect = new Rectangle(pos.X, pos.Y, size.X, size.Y);

                    switch (tile)
                    {
                        case '1':

                            walls.Add(new Wall(pos, size, Color.Brown, walltexture));
                            break;
                        case '2':
                            AddWallToGroup(0, rect, walltexture, true);
                            break;
                        case '3':
                            destructibleWalls.Add(new DestructibleWall(pos, size, walltexture));
                            break;
                        case '4':
                            AddWallToGroup(1, rect,walltexture, true);
                            break;
                        case '5':
                            AddWallToGroup(2, rect, walltexture,false);
                            break;
                        case '6':
                            AddWallToGroup(3, rect, walltexture, false);
                            break;
                        case '7':
                            AddWallToGroup(4, rect, walltexture, false);
                            break;
                        case '8':
                            pushables.Add(new PushableObstacle(pos, size, boxTexture));
                            break;
                        case '9':
                            stickyTiles.Add(new StickyTile(pos, size, 0.5f));
                            break;
                        case 'A':
                            flameWalls.Add(new FlameWallTile(pos, size));
                            break;


                    }
                }
            }

            Isloaded = true;
            Console.WriteLine("Level loaded sucessfully");
        }
        private void AddWallToGroup(int groupID, Rectangle rect, Texture2D texture, bool isVertical = false )
        {
            while (rotatingGroups.Count <= groupID)
                rotatingGroups.Add(new List<GroupWall>());

            rotatingGroups[groupID].Add(new GroupWall(rect, texture, isVertical));
        }
        public void Update(float deltaTime)
        {
            foreach(var dWall in destructibleWalls)
                dWall.Update(deltaTime);

            rotationTimer += deltaTime;

            if(rotationTimer >= rotationInterval)
            {
                foreach(var group in rotatingGroups)
                {
                    if (group.Count == 0) continue;

                    float baseX = group.Min(w => w.Rect.X);
                    float baseY = group.Min(w => w.Rect.Y);

                    float tile = group[0].Rect.Width;

                    if (group[0].IsVertical)
                    {
                        //wenn vertikal, horizontal verschieben
                        for (int i = 0; i < group.Count; i++)
                        {
                            group[i].SetTarget(new Vector2(baseX + tile * i, baseY));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < group.Count; i++)
                            group[i].SetTarget(new Vector2(baseX, baseY + tile * i));
                    }
                    
                }
                rotationTimer = 0f;
            }

            foreach (var group in rotatingGroups)
                foreach (var gw in group)
                    gw.Update(deltaTime, 2f);

            foreach (var f in flameWalls)
            {
                f.Update(deltaTime);
            }
        }
        public void Draw()
        {
            ScaledRenderer.DrawBounds(Color.DarkGray);

            foreach (var wall in walls)
            {
                wall.DrawScaled();
            }
            foreach (var obs in obstacles)
            {
                obs.DrawScaled();
            }
            foreach(var dWall in destructibleWalls)
            {
                dWall.DrawScaled();
            }
            foreach (var group in rotatingGroups) 
            {
                foreach(var gwall in group)
                {
                    gwall.Draw();
                }
            } 
            foreach(var p in pushables)
            {
                p.Draw();
            }
            foreach(var tile in stickyTiles)
            {
                tile.Draw();
            }
            foreach (var f in flameWalls)
            {
                f.Draw();
            }
            
        }

        public void checkWallHit(Vector2 hitPosition)
        {
            foreach(var wall in destructibleWalls)
            {
                if(!wall.IsDestroyed && wall.CheckHit(hitPosition))
                {
                    wall.Destroy();
                    break;
                }
            }
        }

        public void Unload()
        {
            if(walltexture.Id != 0)
               Raylib.UnloadTexture(walltexture);
            if (boxTexture.Id != 0)
                Raylib.UnloadTexture(boxTexture);
        }
    }
}