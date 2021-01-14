using System;
using System.Collections.Generic;
using Controllers.Players;
using Models;

namespace Controllers
{
    public class ConsoleInput
    {
        private readonly GameBoardWithEvents _game;

        public ConsoleInput(GameBoardWithEvents gameBoardWithEvents)
        {
            _game= gameBoardWithEvents;
        }

        public void StartGame()
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
                        _game.StartGame();
                        break;
                    case "move":
                        int.TryParse(splitCommand[1], out var x);
                        int.TryParse(splitCommand[2], out var y);
                        _game.MakeMove(new Tuple<int, int>(x,y));
                        break;
                    case "restart":
                        _game.RestartGame();
                        break;
                    case "finish":
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Unknown command");
                        break;
                }
            }
        }
        
        /*public void StartGame()
        {
            var firstPlayer = new HumanPlayer(_game);
            var secondPlayer = new HumanPlayer(_game);
            
            //game loop
            while (true)
            {
                firstPlayer.MakeMove();
                secondPlayer.MakeMove();
            }
        }*/


        /*private void printError()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("PLEASE, input the correct coordinates");
            Console.ResetColor();
        }*/
    }
}