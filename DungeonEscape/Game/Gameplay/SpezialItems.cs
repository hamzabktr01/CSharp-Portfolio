using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonEscape
{
    public class SpezialItem
    {
        public string Bezeichnung { get; }
        public int Heilung { get; }
        public bool IstFluchtbombe { get; }

        public SpezialItem(int itemNummer)
        {
            if (itemNummer == 1)
            {
                Bezeichnung = "Heiltrank";
                Heilung = 100;
                IstFluchtbombe = false;
            }
            else
            {
                Bezeichnung = "Fluchtbombe";
                Heilung = 0;
                IstFluchtbombe = true;
            }
        }

        public void Anwenden(Spieler spieler, Monster monster)
        {
            if (IstFluchtbombe)
            {
               
                Random rand = new Random();
                double chance = rand.NextDouble();  

                if (chance < 0.5)
                {
                   
                    Console.WriteLine("\nDu hast die Fluchtbombe verwendet! Du entkommst dem Kampf.");
                    monster.Lebenspunkte = 0; 
                }
                else
                {
                   
                    Console.WriteLine("\nDie Fluchtbombe hat nicht funktioniert! Du bleibst im Kampf.");
                }
            }
            else
            {
                Console.WriteLine($"\nDu hast einen Heiltrank verwendet! {spieler.Name} heilt sich um {Heilung} HP.");
                spieler.Lebenspunkte += Heilung;
            }
        }

    }
}