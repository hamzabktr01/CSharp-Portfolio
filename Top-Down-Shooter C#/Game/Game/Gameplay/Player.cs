using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using Game.Engine;
using Game.Gameplay;
using System.Diagnostics.Tracing;
using System.Security.Cryptography.X509Certificates;

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
private float invulnerableTimer = 0-0f;
private float blinkTimer = 0.0f;
private bool isVisible =true;
public Rectangle Rect
        {
            get{ return new Rectangle(Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2); }
        }
        

    public Player(int startX, int startY, float radius, Color color)
    {
        
        Position = new Vector2(startX, startY);
        startPosition = new Vector2(startX,startY);
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
            if(isInvulnerable)
            {
                float dt = Raylib.GetFrameTime();
                invulnerableTimer -= dt;
                blinkTimer += dt;
                if(blinkTimer >= 0.1f)
                {
                    isVisible =!isVisible;
                    blinkTimer=0.0f;
                }
                if(invulnerableTimer<= 0)
                {
                    isInvulnerable=false;
                    isVisible =true;
                }
            }
            Vector2 Movement = Vector2.Zero;

        if (Raylib.IsKeyDown(KeyboardKey.W))
        {
            Movement.Y -= PlayerSpeed;
            FacingDirection = new Vector2(0, -1);
        }
       else if (Raylib.IsKeyDown(KeyboardKey.S))
        {
            Movement.Y += PlayerSpeed;
            FacingDirection = new Vector2(0, 1);
        }
       else if (Raylib.IsKeyDown(KeyboardKey.A))
        {
            Movement.X-= PlayerSpeed;
            FacingDirection = new Vector2(-1, 0);
        }
        else  if (Raylib.IsKeyDown(KeyboardKey.D))
       {
           Movement.X += PlayerSpeed;
            FacingDirection = new Vector2(1, 0);
       }
            if (Movement != Vector2.Zero)
            {
                float slowMult = 1f;

                foreach(var tile in currentLevel.stickyTiles)
                {
                    if(Raylib.CheckCollisionRecs(this.Rect,tile.Rect))
                    {
                        slowMult = tile.SlowMultiplier;
                        break;
                    }
                }

                Movement *= slowMult;

                Vector2 nextPosition = Position + Movement;

                Rectangle nextRect = new Rectangle(nextPosition.X - Radius, nextPosition.Y - Radius,Radius * 2, Radius * 2);
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
                        foreach(var gwall in group)
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
                if(!isColliding)
                {
                    foreach(var box in currentLevel.pushables)
                    {
                        if(Raylib.CheckCollisionRecs(nextRect, box.Rect))
                        {
                            Vector2 newBoxPos = box.Position + Movement;

                            Rectangle boxNext = 
                                new Rectangle(newBoxPos.X, newBoxPos.Y, box.Size.X, box.Size.Y);

                            bool blocked = false;

                            foreach(var wall in currentLevel.walls)
                            {
                                if(Raylib.CheckCollisionRecs(boxNext,wall.Rect))
                                {
                                    blocked = true;
                                    break;
                                }
                            }

                            if(!blocked)
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

                if(!isColliding)
                {
                    if(nextRect.X < 0 ||
                    nextRect.X + nextRect.Width > ScaledRenderer.VIRTUAL_WIDTH||
                    nextRect.Y < 0 ||
                    nextPosition.Y+ nextRect.Height > ScaledRenderer.VIRTUAL_HEIGHT)
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
        if(isInvulnerable) return;
        PlayerHealth -= amount;

        if (PlayerHealth <= 0)
        {
            PlayerHealth = 0;
            IsAlive = false;
            
        }
            else
            {
                Position =startPosition;
                isInvulnerable =true;
                invulnerableTimer =3.0f;
                isVisible=true;
                blinkTimer=0.0f;
            }
    }
    public void DrawPlayer()
    {
        if (IsAlive&&isVisible)
        {
          ScaledRenderer.DrawRect(Rect.X, Rect.Y, Rect.Width, Rect.Height, PlayerColor);
        }
    }
}
}