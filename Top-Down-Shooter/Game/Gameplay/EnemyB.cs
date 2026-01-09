using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Game.Engine;
using Game.Gameplay;

namespace Game.Gameplay
{
    public class EnemyB : BaseEnemy
    {        
        public Vector2 Movement;
        public Vector2 FacingDirection { get; private set; }
        public float DirectionChangeTime = 1.5f;
        private float directionTimer = 0f;
        private Random rng = new Random();
   
        public EnemyB(int x, int y) : base(10f, Color.Red)
        {
            Position = new Vector2(x, y);
            EnemySpeed = 1;
            EnemyHealth = 2;
            EnemyIsAlive = true;
            FacingDirection = new Vector2(1, 0);
        }      

        public Vector2 UpdateEnemyB(float fT, Level currentLevel)
        {
            directionTimer -= fT;

            if (directionTimer <= 0f)
            {
                directionTimer = DirectionChangeTime;

                int dir = rng.Next(4);

                switch (dir)
                {
                    case 0: Movement = new Vector2(0, -EnemySpeed); break;
                    case 1: Movement = new Vector2(0, EnemySpeed); break;
                    case 2: Movement = new Vector2(-EnemySpeed, 0); break;
                    case 3: Movement = new Vector2(EnemySpeed, 0); break;
                }
            }
          
            if (Movement != Vector2.Zero)
            {
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
                    foreach (var dWall in currentLevel.destructibleWalls)
                    {
                        if (!dWall.IsDestroyed)
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
                else
                {
                    directionTimer = 0f;
                }
            }
            return Position;
        }
    }
}