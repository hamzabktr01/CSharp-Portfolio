namespace DungeonEscape
{
    public abstract class Angriff
    {
        public string Bezeichnung { get; protected set; }
        public int Schaden { get; protected set; }

        public virtual void Ausfuehren(Monster monster)
        {
            monster.Lebenspunkte -= Schaden;
            if (monster.Lebenspunkte < 0) monster.Lebenspunkte = 0;
            Console.WriteLine(BeschreibungAngriff(monster));
        }
        public abstract string BeschreibungAngriff(Monster monster);
    }

    public class Feuerangriff : Angriff
    {
        public Feuerangriff()
        {
            Bezeichnung = "Feuerangriff";
            Schaden = 20;
        }

        // Beschreibung für den Feuerangriff
        public override string BeschreibungAngriff(Monster monster)
        {
            return $"Du schleuderst eine Feuerkugel auf {monster.Name}!\n\nDer Gegner wird von den Flammen verbrannt und erleidet {Schaden} Schaden.";
        }
    }

    public class Nahkampf : Angriff
    {
        public Nahkampf()
        {
            Bezeichnung = "Nahkampf";
            Schaden = 25;
        }

        // Beschreibung für den Nahkampfangriff
        public override string BeschreibungAngriff(Monster monster)
        {
            return $"Du stürmst auf {monster.Name} zu und versetzt ihm einen kräftigen Schlag!\n\nDer Angriff fügt {Schaden} Schaden zu.";
        }
    }

    public class MagischerAngriff : Angriff
    {
        public MagischerAngriff()
        {
            Bezeichnung = "Magischer Angriff";
            Schaden = 30;
        }

        // Beschreibung für den Magischen Angriff
        public override string BeschreibungAngriff(Monster monster)
        {
            return $"Mit einem magischen Zauber entfesselst du eine gewaltige Energieblase\n\n{monster.Name} wurde getroffen und {Schaden} Schaden wurde angerichtet.";
        }
    }

    public class Blitzangriff : Angriff
    {
        public Blitzangriff()
        {
            Bezeichnung = "Blitzangriff";
            Schaden = 35;
        }

        // Beschreibung für den Blitzangriff
        public override string BeschreibungAngriff(Monster monster)
        {
            return $"Ein greller Blitz zuckt vom Himmel und trifft {monster.Name} direkt!\n\nDer Blitz verursacht {Schaden} Schaden.";
        }
    }

    public class HeftigerSchlag : Angriff
    {
        public HeftigerSchlag()
        {
            Bezeichnung = "Heftiger Schlag";
            Schaden = 40;
        }

        // Beschreibung für den heftigen Schlag
        public override string BeschreibungAngriff(Monster monster)
        {
            return $"Du versuchst einen heftigen Schlag mit deiner gesamten Kraft und triffst {monster.Name} mit voller Wucht!\n\nDer Schlag verursacht {Schaden} Schaden.";
        }
    }
}
