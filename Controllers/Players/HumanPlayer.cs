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
        
        public void MakeMove()
        {
            var availableCells = _gameBoard.GetAvailableCells();
            var moveCoords = new Tuple<int, int>(-1, -1);
            
            do
            {
                var move = Console.ReadLine();
                var moveCommands = move?.Split(' ');

                int x = Convert.ToInt32(moveCommands?[1]);
                int y = Convert.ToInt32(moveCommands?[2]);
                
                if (move != null)
                {

                    if (!IsLegalMove(x, y))
                    {
                        if (move?[0].ToString() == "pass")
                        {
                            _gameBoard.Pass();
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
        }
        
        public static bool IsLegalMove(int x, int y)
        {
            return x <= 7 && y <= 7 && x >= 0 && y >= 0;
        }
    }
}