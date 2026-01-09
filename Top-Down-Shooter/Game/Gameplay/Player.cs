using Raylib_cs;
using static Raylib_cs.Raylib;
using System.Numerics;
using Game.Engine;



namespace Game.Gameplay
{

    public class Player
    {
        public Vector2 Position;
        public float Radius;
        public Color PlayerColor;
        public int PlayerSpeed;
        public int PlayerHealth;
        public bool IsAlive;
        public Vector2 FacingDirection { get; private set; }
        public int Points { get; set; }
        public bool PowerUpAvailable = false;

        private Vector2 startPosition;
        private bool isInvulnerable = false;
        private float invulnerableTimer = 0 - 0f;
        private float blinkTimer = 0.0f;
        private bool isVisible = true;
        public float stepInterval = 0.35f;
        float stepTimer = 0.0f;
        private bool step = false;
        StripSheetAnimation playeranimation = TextureManager.Instance.CreateInstance(
                key: "player_idle",
                fps: 12,
                loop: true,
                id: 5
                );
        public Rectangle Rect
        {
            get { return new Rectangle(Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2); }
        }


        public Player(int startX, int startY, float radius, Color color)
        {

            Position = new Vector2(startX, startY);
            startPosition = new Vector2(startX, startY);
            Radius = radius;
            PlayerColor = color;
            PlayerSpeed = 2;
            PlayerHealth = 3;
            IsAlive = true;
            FacingDirection = new Vector2(1, 0);
            Points = 0;

        }
        public Vector2 UpdatePlayer(Level currentLevel)
        {
            float datetime = GetFrameTime();
            playeranimation?.Update(datetime);
            if (PlayerSpeed == 5)
            {
                stepInterval = stepInterval / 2;
            }
            else
            {
                stepInterval = 0.35f;
            }

            if (isInvulnerable)
            {

                float dt = Raylib.GetFrameTime();
                invulnerableTimer -= dt;
                blinkTimer += dt;
                if (blinkTimer >= 0.1f)
                {
                    isVisible = !isVisible;
                    blinkTimer = 0.0f;
                }
                if (invulnerableTimer <= 0)
                {
                    isInvulnerable = false;
                    isVisible = true;
                }
            }
            Vector2 Movement = Vector2.Zero;

            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                Movement.Y -= PlayerSpeed;
                FacingDirection = new Vector2(0, -1);
                if (playeranimation.Key != "player_walk_up")
                {
                    GameBus.Emit(5, "player_walk_up");
                }
                stepTimer += datetime;
                if (stepTimer >= stepInterval)
                {
                    if(step == true)
                    {
                        GameBus.Emit("footstep_1");
                        step = false;
                        stepTimer = 0.0f;
                    }
                    if(step == false)
                    {
                        GameBus.Emit("footstep_3");
                        step = true;
                        stepTimer = 0.0f;
                    }
                    else
                    {
                        stepTimer = 0.0f;
                    }
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                Movement.Y += PlayerSpeed;
                FacingDirection = new Vector2(0, 1);
                if (playeranimation.Key != "player_walk_down")
                {
                    GameBus.Emit(5, "player_walk_down");
                }
                stepTimer += datetime;
                if (stepTimer >= stepInterval)
                {
                    if (step == true)
                    {
                        GameBus.Emit("footstep_1");
                        step = false;
                        stepTimer = 0.0f;
                    }
                    if (step == false)
                    {
                        GameBus.Emit("footstep_3");
                        step = true;
                        stepTimer = 0.0f;
                    }
                    else
                    {
                        stepTimer = 0.0f;
                    }
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                Movement.X -= PlayerSpeed;
                FacingDirection = new Vector2(-1, 0);
                if (playeranimation.Key != "player_walk_left")
                {
                    GameBus.Emit(5, "player_walk_left");
                }
                stepTimer += datetime;
                if (stepTimer >= stepInterval)
                {
                    if (step == true)
                    {
                        GameBus.Emit("footstep_1");
                        step = false;
                        stepTimer = 0.0f;
                    }
                    if (step == false)
                    {
                        GameBus.Emit("footstep_3");
                        step = true;
                        stepTimer = 0.0f;
                    }
                    else
                    {
                        stepTimer = 0.0f;
                    }
                }
            }
            else if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                Movement.X += PlayerSpeed;
                FacingDirection = new Vector2(1, 0);
                if (playeranimation.Key != "player_walk_right")
                {
                    GameBus.Emit(5, "player_walk_right");
                }
                stepTimer += datetime;
                if (stepTimer >= stepInterval)
                {
                    if (step == true)
                    {
                        GameBus.Emit("footstep_1");
                        step = false;
                        stepTimer = 0.0f;
                    }
                    if (step == false)
                    {
                        GameBus.Emit("footstep_3");
                        step = true;
                        stepTimer = 0.0f;
                    }
                    else
                    {
                        stepTimer = 0.0f;
                    }
                }
            }
            else if (playeranimation.Key != "player_idle")
            {
                GameBus.Emit(5, "player_idle");
            }
            if (Movement != Vector2.Zero)
            {
                float slowMult = 1f;

                foreach (var tile in currentLevel.stickyTiles)
                {
                    if (Raylib.CheckCollisionRecs(this.Rect, tile.Rect))
                    {
                        slowMult = tile.SlowMultiplier;
                        break;
                    }
                }

                Movement *= slowMult;

                Vector2 nextPosition = Position + Movement;

                Rectangle nextRect = new Rectangle(nextPosition.X - Radius, nextPosition.Y - Radius, Radius * 2, Radius * 2);
                bool isColliding = false;

                foreach (var wall in currentLevel.walls)
                {
                    if (Raylib.CheckCollisionRecs(nextRect, wall.Rect))
                    {
                        isColliding = true;
                        break;
                    }
                }
                if (!isColliding)
                {
                    foreach (var group in currentLevel.rotatingGroups)
                    {
                        foreach (var gwall in group)
                        {
                            if (Raylib.CheckCollisionRecs(nextRect, gwall.Rect))
                            {
                                isColliding = true;
                                break;
                            }
                        }

                        if (isColliding) break;

                    }
                }
                if (!isColliding)
                {
                    foreach (var dWall in currentLevel.destructibleWalls)
                    {
                        if (dWall.IsCollidable)
                        {

                            Rectangle dWallRect = new Rectangle(dWall.Position.X, dWall.Position.Y, dWall.Size.X, dWall.Size.Y);
                            if (Raylib.CheckCollisionRecs(nextRect, dWallRect))
                            {
                                isColliding = true;
                                break;
                            }
                        }
                    }
                }
                if (!isColliding)
                {
                    foreach (var obstacle in currentLevel.obstacles)
                    {
                        Rectangle obstacleRect = new Rectangle(obstacle.Position.X, obstacle.Position.Y, obstacle.Size.X, obstacle.Size.Y);
                        if (Raylib.CheckCollisionRecs(nextRect, obstacleRect))
                        {
                            isColliding = true;
                            break;
                        }
                    }
                }

                //PUSHABLE OBSTACLE
                if (!isColliding)
                {
                    foreach (var box in currentLevel.pushables)
                    {
                        if (Raylib.CheckCollisionRecs(nextRect, box.Rect))
                        {
                            Vector2 newBoxPos = box.Position + Movement;

                            Rectangle boxNext =
                                new Rectangle(newBoxPos.X, newBoxPos.Y, box.Size.X, box.Size.Y);

                            bool blocked = false;

                            foreach (var wall in currentLevel.walls)
                            {
                                if (Raylib.CheckCollisionRecs(boxNext, wall.Rect))
                                {
                                    blocked = true;
                                    break;
                                }
                            }

                            if (!blocked)
                            {
                                box.Position = newBoxPos;
                                Position = nextPosition;
                                return Position;
                            }
                            else
                            {
                                isColliding = true;
                            }

                        }
                    }
                }

                if (!isColliding)
                {
                    if (nextRect.X < 0 ||
                    nextRect.X + nextRect.Width > ScaledRenderer.VIRTUAL_WIDTH ||
                    nextRect.Y < 0 ||
                    nextPosition.Y + nextRect.Height > ScaledRenderer.VIRTUAL_HEIGHT)
                    {
                        isColliding = true;
                    }
                }
                if (!isColliding)
                {
                    Position = nextPosition;
                }
            }
            return Position;
        }
        public void TakeDamage(int amount)
        {
            if (isInvulnerable) return;
            PlayerHealth -= amount;

            if (PlayerHealth <= 0)
            {
                PlayerHealth = 0;
                IsAlive = false;

            }
            else
            {
                Position = startPosition;
                isInvulnerable = true;
                invulnerableTimer = 3.0f;
                isVisible = true;
                blinkTimer = 0.0f;
            }
        }
        public void DrawPlayer()
        {
            if (IsAlive && isVisible)
            {
                ScaledRenderer.DrawScaled(playeranimation.Texture, playeranimation.Source, Rect.X, Rect.Y, Rect.Width, Rect.Height, Color.White);
            }
        }
    }
}