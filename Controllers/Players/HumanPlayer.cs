using System;
using System.Collections.Generic;
using Models;

namespace Controllers.Players
{
    public class HumanPlayer : IPlayer
    {
        private readonly GameBoardWithEvents _gameBoard;

        public HumanPlayer(GameBoardWithEvents gameBoard)
        {
            _gameBoard = gameBoard;
        }
        
        public Tuple<int, int> MakeMove()
        {
            var availableCells = _gameBoard.GetAvailableCells();
            var moveCoords = new Tuple<int, int>(-1, -1);

            //read from console white input isn`t correct
            do
            {
                var move = Console.ReadLine();
                var moveCommands = move?.Split(' ');
               
                int.TryParse(moveCommands?[1], out var x);
                int.TryParse(moveCommands?[2], out var y);
                
                if (move != null)
                {

                    if (!IsLegalMove(x, y))
                    {
                        if (move?[0].ToString() == "pass")
                        {
                            _gameBoard.PassWithoutMassage();
                            break;
                        }

                        continue;
                    }

                    moveCoords = new Tuple<int, int>(x,y);
                }

                if (availableCells.Contains(moveCoords))
                {
                    _gameBoard.MakeMove(moveCoords);
                    break;
                }
            }
            while (true);

            return moveCoords;
        }
        
        public static bool IsLegalMove(int x, int y)
        {
            return x <= 8 && y <= 8 && x >= 1 && y >= 1;
        }
    }
}