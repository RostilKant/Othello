using System;
using System.Collections.Generic;
using Models;

namespace Controllers
{
    public class ConsoleInput
    {
        public void ReadCommands(GameBoardWithEvents game)
        {
            string command = null;

            while (true)
            {
                command = Console.ReadLine();
                List<string> splitCommand = new List<string>(command?.Split(new char[0])!);

                for (int i = 3; i >= splitCommand.Count; i--)
                {
                    splitCommand.Add("");
                }
                

                switch (splitCommand?[0].ToLower())
                {
                    case "start":
                        game.StartGame(splitCommand[1].ToLower());
                        game.CalculatePlayersScore();
                        break;
                    case "move":
                        int.TryParse(splitCommand[1], out var x);
                        int.TryParse(splitCommand[2], out var y);
                        game.MakeMove((x,y));
                        break;
                    case "restart":
                        game.RestartGame();
                        game.CalculatePlayersScore();
                        break;
                    case "finish":
                        game.FinishGame();
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }

        private void printError()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("PLEASE, input the correct coordinates");
            Console.ResetColor();
        }
    }
}