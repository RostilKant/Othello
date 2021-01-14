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
            IPlayer first = null;
            IPlayer second = null;
            while (true)
            {
                var command = Console.ReadLine();
                var splitCommand = new List<string>(command?.Split(new char[0])!);

                for (int i = 3; i >= splitCommand.Count; i--)
                {
                    splitCommand.Add("");
                }
                

                switch (splitCommand?[0].ToLower())
                {
                    case "start":
                        switch (splitCommand?[1].ToLower())
                        {
                            case "computer":
                                first = new HumanPlayer(_game);
                                second = new HumanPlayer(_game);
                                break;
                            case "ai":
                                first = new HumanPlayer(_game);
                                second = new HumanPlayer(_game);
                                break;
                            case "computerVSai":
                                first = new HumanPlayer(_game);
                                second = new HumanPlayer(_game);
                                break;
                        }
                        _game.StartGame();
                        break;
                    case "move":
                        break;
                    case "restart":
                        _game.RestartGame();
                        break;
                    case "finish":
                        break;
                    case "exit":
                        break;
                    default:
                        while (true)
                        {
                            first?.MakeMove();
                            second?.MakeMove();
                        }
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