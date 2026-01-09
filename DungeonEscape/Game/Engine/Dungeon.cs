using DungeonEscape;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonEscape
{
    public class Dungeon
    {
        private int aktuellerRaum;
        private List<Monster> monster;


        public Monster this[int raumIndex]
        {
            get
            {
                if (raumIndex >= 0 && raumIndex < monster.Count)
                    return monster[raumIndex];
                else
                    throw new ArgumentOutOfRangeException("Raumindex liegt außerhalb des gültigen Bereichs.");
            }
        }


        public Dungeon()
        {
            aktuellerRaum = 0;
            monster = new List<Monster>
                {
                    new Monster("Skelett", 80, 20),
                    new Monster("Ork", 100, 25),
                    new Monster("Drache", 120, 35)
                };
        }

        public void AnzeigeAktuellerRaum()
        {
            Console.WriteLine($"\nDu bist im {aktuellerRaum + 1}. Raum des Dungeons:");
        }

        public bool MonsterImRaum()
        {
            return monster[aktuellerRaum].Lebenspunkte > 0;
        }

        public Monster GetAktuellesMonster()
        {
            return monster[aktuellerRaum];
        }

        public void Kampf(Spieler spieler)
        {
            while (spieler.Lebenspunkte > 0 && monster[aktuellerRaum].Lebenspunkte > 0)
            {
                spieler.Angreifen(monster[aktuellerRaum]);

                if (monster[aktuellerRaum].Lebenspunkte > 0)
                {
                    Console.Clear();
                    Console.WriteLine($"\n{monster[aktuellerRaum].Name} greift {spieler.Name} an!");
                    spieler.Lebenspunkte -= monster[aktuellerRaum].Angriffsschaden;
                    if (spieler.Lebenspunkte < 0) spieler.Lebenspunkte = 0;
                    Console.WriteLine($"\n{spieler.Name} hat noch {spieler.Lebenspunkte} HP.");
                }

                System.Threading.Thread.Sleep(3000);
            }


            if (monster[aktuellerRaum].Lebenspunkte <= 0)
            {
                
                Console.Clear();
                Console.WriteLine($"\n{monster[aktuellerRaum].Name} wurde besiegt!");
                System.Threading.Thread.Sleep(3000);


                BewegeSpieler();
                Console.WriteLine("\nDu gehst zum nächsten Raum...");
                System.Threading.Thread.Sleep(3000);
                Console.Clear();
            }
        }

        public bool WurdeEndbossBesiegt()
        {
            return monster[2].Lebenspunkte <= 0;
        }

        public void BewegeSpieler()
        {
            if (aktuellerRaum < monster.Count - 1)
                aktuellerRaum++;
        }
    }


}

