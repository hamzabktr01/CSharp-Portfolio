using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonEscape
{
    public class Spieler
    {
        public string Name { get; }
        public int Lebenspunkte { get; set; }
        public List<Angriff> VerfügbareAngriffe { get; set; }
        private SpezialItem spezialItem;
        private bool spezialItemVerwendet;

        public Spieler(string name)
        {
            Name = name;
            Lebenspunkte = 150;
            VerfügbareAngriffe = new List<Angriff>();
            spezialItemVerwendet = false;
        }

        public void WaehleAngriffe()
        {
            Console.WriteLine("Du hast 3 Angriffe zur Verfügung. Wähle weise!\n");
            Console.WriteLine("1) Feuerangriff (20 Schaden)");
            Console.WriteLine("2) Nahkampf (25 Schaden)");
            Console.WriteLine("3) Magischer Angriff (30 Schaden)");
            Console.WriteLine("4) Blitzangriff (35 Schaden)");
            Console.WriteLine("5) Heftiger Schlag (40 Schaden)");

            HashSet<int> auswahl = new HashSet<int>();
            while (auswahl.Count < 3)
            {
                Console.Write($"\nWähle Angriff {auswahl.Count + 1} (1-5): ");
                if (int.TryParse(Console.ReadLine(), out int auswahlNummer) && auswahlNummer >= 1 && auswahlNummer <= 5)
                {
                    auswahl.Add(auswahlNummer);
                }
                else
                {
                    Console.WriteLine("\nUngültige Eingabe. Bitte wähle eine Nummer zwischen 1 und 5.");
                }
            }

            foreach (int nummer in auswahl)
            {
                switch (nummer)
                {
                    case 1: VerfügbareAngriffe.Add(new Feuerangriff()); break;
                    case 2: VerfügbareAngriffe.Add(new Nahkampf()); break;
                    case 3: VerfügbareAngriffe.Add(new MagischerAngriff()); break;
                    case 4: VerfügbareAngriffe.Add(new Blitzangriff()); break;
                    case 5: VerfügbareAngriffe.Add(new HeftigerSchlag()); break;
                }
            }
            Console.Clear();
            Console.WriteLine("\nWähle ein Spezial-Item:\n");
            Console.WriteLine("1) Heiltrank (stellt 100 HP wieder her)");
            Console.WriteLine("2) Fluchtbombe (50% Chance zu entkommen)");

            while (spezialItem == null)
            {
                Console.Write("\nWähle ein Item (1-2): ");
                if (int.TryParse(Console.ReadLine(), out int itemNummer) && itemNummer >= 1 && itemNummer <= 2)
                {
                    spezialItem = new SpezialItem(itemNummer);
                }
                else
                {
                    Console.WriteLine("\nUngültige Eingabe. Bitte wähle 1 oder 2.");
                }
            }
        }

        public void Angreifen(Monster monster)
        {
            Console.Clear();
            Console.WriteLine($"{Name} (HP: {Lebenspunkte}), wähle deinen Angriff:");

            bool eingabeGültig = false;

            while (!eingabeGültig)
            {
                
                for (int i = 0; i < VerfügbareAngriffe.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {VerfügbareAngriffe[i].Bezeichnung} ({VerfügbareAngriffe[i].Schaden} Schaden)");
                }

                Console.WriteLine($"S) {spezialItem.Bezeichnung}");

                try
                {
                    string eingabe = Console.ReadLine();

                    if (eingabe.ToUpper() == "S" && !spezialItemVerwendet)
                    {
                        spezialItem.Anwenden(this, monster);
                        spezialItemVerwendet = true;
                        eingabeGültig = true;
                    }
                    else if (eingabe.ToUpper() == "S")
                    {
                        Console.WriteLine("\nDu hast das Spezialitem bereits verwendet!");
                        eingabeGültig = true;
                    }
                    else if (int.TryParse(eingabe, out int auswahl) && auswahl > 0 && auswahl <= VerfügbareAngriffe.Count)
                    {
                        Console.Clear();
                        Console.WriteLine($"\n{Name} verwendet {VerfügbareAngriffe[auswahl - 1].Bezeichnung}\n");
                        VerfügbareAngriffe[auswahl - 1].Ausfuehren(monster);
                        System.Threading.Thread.Sleep(3000);
                        Console.WriteLine($"\n{monster.Name} hat noch {monster.Lebenspunkte} Lebenspunkte.");
                        eingabeGültig = true;
                    }
                    else
                    {
                        throw new InvalidOperationException("Ungültige Eingabe! Bitte wähle einen gültigen Angriff oder das Spezialitem.");
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.Clear();
                    Console.WriteLine(ex.Message);  
                }
            }

         
            System.Threading.Thread.Sleep(3000);
        }
    }
}

