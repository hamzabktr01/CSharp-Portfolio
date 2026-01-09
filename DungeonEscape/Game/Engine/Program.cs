using System;
//DungeonEscape OOP-Projekt von: Rahman Salman; Bayraktar Hamza; Lalo Janiar; Jost Julia

namespace DungeonEscape
{
    class Program
    {
        static void Main(string[] args)
        {
            
            while (true)
            {
                //Einführung
                Console.Clear();
                Console.WriteLine("Willkommen zu Dungeon Escape!");
                Console.WriteLine("Du bist in einem dunklen Dungeon gefangen. Besiege die Monster und den Drachen, um zu entkommen.");
               
                
                Console.Write("\n\nGib deinem Helden einen Namen: ");
                string spielerName = Console.ReadLine();
                Console.Clear();
                Console.WriteLine($"\nBereite dich vor, tapferer Held! Dein Abenteuer beginnt jetzt!");
                System.Threading.Thread.Sleep(3000);

                // Erstellen des Spielers und des Dungeons
                Spieler spieler = new Spieler(spielerName);
                Dungeon dungeon = new Dungeon();

                // Angriffsauswahl
                Console.Clear();
                spieler.WaehleAngriffe();
                Console.Clear();

                while (spieler.Lebenspunkte > 0)
                {
                    dungeon.AnzeigeAktuellerRaum();

                    // Wenn ein Monster im Raum ist, wird der Kampf ausgelöst
                    if (dungeon.MonsterImRaum())
                    {
                        Console.WriteLine($"\nHier ist ein {dungeon.GetAktuellesMonster().Name}! Es hat {dungeon.GetAktuellesMonster().Lebenspunkte} HP.");
                        System.Threading.Thread.Sleep(3000);

                        Console.WriteLine("\nBereite dich vor, der Kampf beginnt!");
                        System.Threading.Thread.Sleep(3000);
                        Console.Clear();
                        dungeon.Kampf(spieler); // Kampf zwischen Spieler und Monster
                    }
                    else
                    {
                        Console.WriteLine("\nKein Monster im Raum.");
                    }

                    // Überprüfung ob der Spieler gestorben ist
                    if (spieler.Lebenspunkte <= 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"\nGame Over! {spieler.Name}, du bist gestorben.");
                        System.Threading.Thread.Sleep(2500);

                        Console.WriteLine("\nVielen Dank, dass du Dungeon Escape gespielt hast! (Neustart in 3 Sekunden)");
                        System.Threading.Thread.Sleep(3000);
                        break;
                    }

                    // Überprüfung, ob der Endboss besiegt wurde
                    if (dungeon.WurdeEndbossBesiegt())
                    {
                        Console.Clear();
                        Console.WriteLine($"Herzlichen Glückwunsch {spieler.Name}!");
                        Console.WriteLine("\nDu hast den Drachen besiegt und den Dungeon verlassen!");
                        Console.WriteLine("Möchtest du das Spiel beenden oder neu starten? (beenden/neustarten)");

                        string entscheidung = Console.ReadLine().ToLower();
                        if (entscheidung == "beenden") return;
                        if (entscheidung == "neustarten") break;
                    }

                    // Wenn kein Monster im Raum, wird der Spieler weiterbewegt
                    if (!dungeon.MonsterImRaum() && !dungeon.WurdeEndbossBesiegt())
                    {
                        dungeon.BewegeSpieler();
                        System.Threading.Thread.Sleep(2500);
                        Console.Clear();
                    }
                }
            }
        }
    }
}