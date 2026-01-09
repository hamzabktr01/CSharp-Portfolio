using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace Game.Gameplay
{
    internal class EnemyA : BaseEnemy
    {
        public EnemyA(int x, int y) : base(20f, Color.Red)
        {
            EnemySpeed = 2;
            EnemyHealth = 2;
        }
    }
}
