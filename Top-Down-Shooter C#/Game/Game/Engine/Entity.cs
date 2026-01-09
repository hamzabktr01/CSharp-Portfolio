using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using System.Numerics;

namespace Game.Engine
{
    public class Entity
    {

        public Vector2 EntityVector2 { get; set; }
        public float SizeX { get; set; }
        public float SizeY { get; set; }

        Texture2D Texture { get; set; }

        public uint Health { get; set; }
        public Entity(Vector2 vector2, float sizeX, float sizeY, Texture2D texture, uint health)
        {

            EntityVector2 = vector2;
            SizeX = sizeX;
            SizeY = sizeY;
            Texture = texture;
            Health = health;
        }
        public Entity(Vector2 vector2, int sizeX, int sizeY, uint health)
        {
            //PositionX = positionX;
            //PositionY = positionY;
            EntityVector2 = vector2;
            SizeX = sizeX;
            SizeY = sizeY;
            Health = health;
        }
        public Entity(int startX, int startY, int sizeX, int sizeY, uint health)
        {
            EntityVector2 = new Vector2(startX, startY);
            SizeX = sizeX;
            SizeY = sizeY;
            Health = health;
        }
    }
}


