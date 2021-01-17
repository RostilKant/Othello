using System;
using Models;

namespace Controllers.Players
{
    public class ComputerPlayer : IPlayer
    {
        private readonly GameBoardWithEvents _gameBoard;
        private readonly Random _random = new Random();


        public ComputerPlayer(GameBoardWithEvents gameBoard)
        {
            _gameBoard = gameBoard;
        }
        public void MakeMove()
        {
            var availableCells = _gameBoard.GetAvailableCells();
            
            if (availableCells.Count == 0)
            {
                _gameBoard.ChangedTurn();
                return;
            }
            _gameBoard.MakeMove(availableCells[_random.Next(availableCells.Count)]);
        }
    }
}