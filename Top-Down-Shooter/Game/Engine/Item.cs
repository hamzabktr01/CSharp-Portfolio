using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;
using Game.Gameplay;
namespace Game.Engine
{
    public class Item : Entity
    {
        public Raylib_cs.Rectangle ItemRectangle;

        public StripSheetAnimation? Animation;

        public Item(Vector2 vector2, int sizeX, int sizeY, uint health) : base(vector2, sizeX, sizeY, health)
        {
            ItemRectangle = new Raylib_cs.Rectangle(EntityVector2.X, EntityVector2.Y, SizeX, SizeY);

        }
        public Item(int startX, int startY, int sizeX, int sizeY, uint health) : base(startX, startY, sizeX, sizeY, health)
        {
            ItemRectangle = new Raylib_cs.Rectangle(EntityVector2.X, EntityVector2.Y, SizeX, SizeY);
        }
        public virtual void DrawItem()
        {
            if (Health > 0) return;
            ScaledRenderer.DrawRect(EntityVector2.X, EntityVector2.Y, ItemRectangle.Width, ItemRectangle.Height, Raylib_cs.Color.Blue);
        }

        public void Update_Item(Vector2 position, float radius)
        {

            if (CheckCollisionCircleRec(position, radius, ItemRectangle))
            {
                Health = 0;
                SizeX = 0;
                SizeY = 0;
            }
        }
        public virtual void DrawItemAnimated()
        {
            DrawItem();
        }
        public virtual void Update_Item(ref Player player)
        {
            if (Health <= 0)
            {
                ItemRectangle.Width = 0;
                ItemRectangle.Width = 0;

                return;
            }

            if (CheckCollisionCircleRec(player.Position, player.Radius, ItemRectangle))
            {
                Health = 0;

            }
        }

    }
}

