using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonEscape
{
    public class Monster
    {
        public string Name { get; }
        public int Lebenspunkte { get; set; }
        public int Angriffsschaden { get; }

        public Monster(string name, int lebenspunkte, int angriffsschaden)
        {
            Name = name;
            Lebenspunkte = lebenspunkte;
            Angriffsschaden = angriffsschaden;
        }
    }
}
