using Game.Gameplay;

using System;
using Raylib_cs;

namespace Game.Engine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string levelPath = "../../../Assets/Level1_Labyrinth.txt";
            Game game = new Game(levelPath,1);
            game.Run();
        }
    }
}


