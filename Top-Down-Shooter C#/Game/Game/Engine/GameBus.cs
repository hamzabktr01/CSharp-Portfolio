using System;


namespace Game
{
    public enum Soundtype
    {
        effekt,
        music,
    }
    internal static class GameBus
    {
        public static event Action<string, Soundtype>? GameEvent;

        public static event Action<int, string>? AnimationChangeEvent;


        public static void Emit(string eventName, Soundtype soundttype = Soundtype.effekt)
        {
            Console.WriteLine($"[GameBus] Event triggert");
            GameEvent?.Invoke(eventName, soundttype); // Ruft Methoden die registriert 
        }
        public static void Emit(int id, string newAnimationKey)
        {
            Console.WriteLine($"[GameBus] AnimationCHange id={id} key = {newAnimationKey}");
            AnimationChangeEvent?.Invoke(id, newAnimationKey);
        }
        public static void ClearAllListeners()
        {
            AnimationChangeEvent = null;
            GameEvent = null;
            
            Console.WriteLine("[GameBus] Alle listener entfernt");
        }
    }
}

