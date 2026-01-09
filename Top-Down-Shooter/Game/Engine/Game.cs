using Game.Gameplay;
using Raylib_cs;
using System;
using System.Numerics;
using System.Reflection.Metadata;

namespace Game.Engine
{
    public class Game
    {
        private int levelNumber;
        private LightCircleOverlay? lightOverlay;

        private Level currentLevel;
        private bool isRunning = true;
        private string levelPath;
        public Player player;
        public CoinLoader Coins;
        public TextureManager textureManager;

        public PowerUpLoader PowerUps;
        private List<Projectile> projectiles = new List<Projectile>();
        private int maxProjectiles = 3;
        public SoundManager soundManager = new SoundManager();
        private object EnemyB;
        public EnemyB enemytest1 = new EnemyB(100, 100);
        public EnemyB enemytest2 = new EnemyB(300, 200);
        public EnemyB enemytest3 = new EnemyB(590, 400);


        double powerUpStartTime = 0;
        double powerUpIntervall = 5;
        bool powerUpActive;

        public Game(string levelPath, int levelNumber)
        {
            this.levelPath = levelPath;
            this.levelNumber = levelNumber;
        }

        public void Run()
        {
            Raylib.InitAudioDevice();
            Raylib.InitWindow(ScaledRenderer.VIRTUAL_WIDTH, ScaledRenderer.VIRTUAL_HEIGHT, "Level 1 - scaled");
            Raylib.SetWindowState(ConfigFlags.ResizableWindow);
            Raylib.SetTargetFPS(60);




            soundManager.LoadFromFolder();

            textureManager = new TextureManager();
            textureManager.Load();
            player = new Player(10, 50, 10, Color.Yellow);
            Coins = new CoinLoader();
            PowerUps = new PowerUpLoader();
            currentLevel = new Level(levelPath);
            
            
            
            if(levelNumber == 2)
            {
                lightOverlay = new LightCircleOverlay(
                    ScaledRenderer.VIRTUAL_WIDTH,
                    ScaledRenderer.VIRTUAL_HEIGHT,
                    140f

                    );
            }

            if (!currentLevel.Isloaded)
            {
                Console.WriteLine("Level 1 could not be completed");
                Raylib.CloseWindow();
                return;
            }

            while (!Raylib.WindowShouldClose() && isRunning)
            {
                Update();
                Draw();
            }

            currentLevel.Unload();
            textureManager.Dispose();
            Raylib.CloseWindow();
        }


        private void Update()
        {

            float dt = Raylib.GetFrameTime();

            currentLevel.Update(dt);

            if (player.IsAlive)
            {
                player.UpdatePlayer(currentLevel);
                Coins.UpdateItems(ref player);
                PowerUps.UpdateItems(ref player);
                float currentTime = Raylib.GetFrameTime();

                if (Raylib.IsKeyPressed(KeyboardKey.Space) && projectiles.Count < maxProjectiles)
                {
                    projectiles.Add(new Projectile(player.Position, player.FacingDirection));
                    GameBus.Emit("laser_shooting_sfx");
                }
                if (Raylib.IsKeyPressed(KeyboardKey.R))
                {
                    projectiles.Clear();
                }

                if (Raylib.IsKeyPressed(KeyboardKey.P))
                {
                    player.TakeDamage(1);
                }
                if (Raylib.IsKeyPressed(KeyboardKey.U) && player.PowerUpAvailable)
                {
                    powerUpActive = true;
                    player.PlayerSpeed = 5;
                    projectiles.Clear();
                    player.PowerUpAvailable = false;
                    powerUpStartTime = Raylib.GetTime();

                }
                if (powerUpActive)
                {
                    double elapsedtime = Raylib.GetTime() - powerUpStartTime;
                    if (elapsedtime > powerUpIntervall)
                    {
                        player.PlayerSpeed = 2;
                        powerUpActive = false;
                    }
                }

            }
    
            foreach(var f in currentLevel.flameWalls)
            {
                if(Raylib.CheckCollisionRecs(player.Rect,f.Rect))
                {
                    player.TakeDamage(1); //sofort tot
                    break;
                }
            }

            foreach (var projectile in projectiles)
            {
                projectile.UpdateProjectile();

                foreach (var wall in currentLevel.walls)
                {
                    if (Raylib.CheckCollisionRecs(projectile.Rect, wall.Rect))
                    {
                        projectile.DeactivateProjectile();
                        break;
                    }
                }
                if (projectile.projectileIsActive)
                {
                    foreach (var obstacle in currentLevel.obstacles)
                    {
                        if (Raylib.CheckCollisionRecs(projectile.Rect, obstacle.Rect))
                        {
                            projectile.DeactivateProjectile();
                            break;
                        }
                    }
                }
                if (projectile.projectileIsActive)
                {
                    foreach (var dWall in currentLevel.destructibleWalls)
                    {
                        Rectangle dwallRect = new Rectangle(dWall.Position.X, dWall.Position.Y, dWall.Size.X, dWall.Size.Y);

                        if (!dWall.IsDestroyed && Raylib.CheckCollisionRecs(projectile.Rect, dwallRect))
                        {
                            projectile.DeactivateProjectile();
                            dWall.Destroy();
                            break;
                        }
                    }
                }
                if (projectile.projectileIsActive)
                {
                    foreach (PowerUp powerUp in PowerUps.ItemList)
                    {
                        if (Raylib.CheckCollisionRecs(projectile.Rect, powerUp.ItemRectangle))
                        {
                            powerUp.takeDamage(10);
                        }
                    }
                }
                if (projectile.projectileIsActive)
                {
                    if (Raylib.CheckCollisionRecs(projectile.Rect, enemytest1.Rect))
                    {
                        enemytest1.EnemyTakeDamage(1);
                        projectile.DeactivateProjectile();
                        break;
                    }
                }
                if (projectile.projectileIsActive)
                {
                    if (Raylib.CheckCollisionRecs(projectile.Rect, enemytest2.Rect))
                    {
                        enemytest2.EnemyTakeDamage(1);
                        projectile.DeactivateProjectile();
                        break;
                    }
                }
                if (projectile.projectileIsActive)
                {
                    if (Raylib.CheckCollisionRecs(projectile.Rect, enemytest3.Rect))
                    {
                        enemytest3.EnemyTakeDamage(1);
                        projectile.DeactivateProjectile();
                        break;
                    }
                }

            }
            if (enemytest1.EnemyIsAlive)
            {
                float fT = Raylib.GetFrameTime();
                enemytest1.UpdateEnemyB(fT, currentLevel);
            }

            if (enemytest2.EnemyIsAlive)
            {
                float fT = Raylib.GetFrameTime();
                enemytest2.UpdateEnemyB(fT, currentLevel);
            }
            if (enemytest3.EnemyIsAlive)
            {
                float fT = Raylib.GetFrameTime();
                enemytest3.UpdateEnemyB(fT, currentLevel);
            }

            if (Raylib.CheckCollisionRecs(player.Rect, enemytest1.Rect))
            {
                if (enemytest1.EnemyIsAlive)
                {
                    player.TakeDamage(1);
                }
            }
            if (Raylib.CheckCollisionRecs(player.Rect, enemytest2.Rect))
            {
                if (enemytest2.EnemyIsAlive)
                {
                    player.TakeDamage(1);
                }
            }
            if (Raylib.CheckCollisionRecs(player.Rect, enemytest3.Rect))
            {
                if (enemytest3.EnemyIsAlive)
                {
                    player.TakeDamage(1);
                }
            }

            foreach (var dwall in currentLevel.destructibleWalls)
                dwall.Update(dt);

        }

        private void Draw()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            ScaledRenderer.Begin();
            player.DrawPlayer();
            enemytest1.DrawEnemy();
            enemytest2.DrawEnemy();
            enemytest3.DrawEnemy();

            Coins.DrawItems();
            PowerUps.DrawItems();
            currentLevel.Draw();
            foreach (var p in projectiles) p.Draw();

            if (levelNumber == 2 && lightOverlay != null)
            {
                lightOverlay.Draw(player.Position);
            }

            if (player.IsAlive)
            {
                float s = ScaledRenderer.scale;
                float ox = ScaledRenderer.offsetX;
                float oy = ScaledRenderer.offsetY;

                int fontSize = (int)(20*s);
                float paddingRight = 120 *s;

                int uiX=(int)(ox +ScaledRenderer.VIRTUAL_WIDTH*s-paddingRight);
                int uiY =(int)(oy+10*s);

                Raylib.DrawText($"HP:{player.PlayerHealth}",uiX,uiY,fontSize,Color.Red);
                Raylib.DrawText($"Amo:{maxProjectiles-projectiles.Count}",uiX,uiY+fontSize,fontSize,Color.White);
                Raylib.DrawText($"Points:{player.Points}",uiX,uiY +(fontSize*2),fontSize,Color.Gold);
                
                    
                
            }
            else
            {
                string gameOverText = "Game Over";
                int fontSize=(int)(50*ScaledRenderer.scale);
                int textWidth = Raylib.MeasureText(gameOverText, fontSize);
                int x=(Raylib.GetScreenWidth()/2)-(textWidth/2);
                int y=(Raylib.GetScreenHeight()/2)-(fontSize/2);
                
                Raylib.DrawText(gameOverText, x,y,fontSize,Color.Red);
            }

           

            ScaledRenderer.End();
            Raylib.EndDrawing();

        }
    }
}