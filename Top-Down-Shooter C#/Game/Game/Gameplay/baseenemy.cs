using System;
using Raylib_cs;
using System.Numerics;
using System.Collections.Generic;
using Game.Engine;
using Game.Gameplay;

namespace Game.Gameplay
{
    public class BaseEnemy
    {
        public Vector2 Position;
        public float Radius;
        public Color EnemyColor;
        public int EnemySpeed;
        public int EnemyHealth;
        public bool EnemyIsAlive;
        public Rectangle Rect
        {
            get { return new Rectangle(Position.X - Radius, Position.Y - Radius, Radius * 2, Radius * 2); }
        }

        public BaseEnemy(float radius, Color color)
        {           
            Radius = radius;
            EnemyColor = color;
            EnemySpeed = 0;
            EnemyHealth = 3;
        }

        public void EnemyTakeDamage(int amount)
        {
            EnemyHealth -= amount;
            if (EnemyHealth <= 0)
            {
                EnemyHealth = 0;
                EnemyIsAlive = false;
                //Enemy defeated
            }
        }

        public void DrawEnemy()
        {
            if (EnemyIsAlive)
            {
                ScaledRenderer.DrawRect(Rect.X, Rect.Y, Rect.Width, Rect.Height, EnemyColor);
            }
        }
    }
}
