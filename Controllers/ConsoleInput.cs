using System;
using Models;

namespace Controllers
{
    public class ConsoleInput
    {
        public void ReadCommands(GameBoardWithEvents game)
        {
            string command = null;
            Console.Out.WriteLine("And his name iiiiis OTHELLO!!!Tuturutuuu tuturutuuu... Type START to play ");

            while (true)
            {
                command = Console.ReadLine();
                var splitCommand = command?.Split(new char[0]);

                switch (splitCommand?[0].ToLower())
                {
                    case "start":
                        game.StartGame();
                        break;
                    case "move":
                        var x = int.Parse(splitCommand[1]);
                        var y = int.Parse(splitCommand[2]);
                        game.MakeMove((x,y));
                        break;
                    default:
                        Console.WriteLine("GG");
                        break;
                }
            }
        }
    }
}