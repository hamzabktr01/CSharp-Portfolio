using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;

namespace Game.Gameplay
{
    internal class EnemyC : BaseEnemy
    {
        public EnemyC(int x, int y) : base(10f, Color.Violet)
        {
            EnemySpeed = 1;
            EnemyHealth = 5;
        }
    }
}
