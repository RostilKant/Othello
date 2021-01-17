using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Controllers.Players
{
    // ReSharper disable once InconsistentNaming
    public class AIPlayer : IPlayer
    {
        private readonly GameBoardWithEvents _gameBoard;

        readonly int[][] _sev = {
            new[]{500, -110, 20, 23, 22, 20, -110, 502},
            new[]{-109, -146,  -19, -6, -6, -20, -146, -111},
            new[]{19, -20,  -27, -4, -4, -28, -20, 20},
            new[]{25, -6,  -4, 0 , 0, -4, -5, 26},
            new[]{26, -7,  -4, 0 , 0, -4, -6, 27},
            new[]{21, -20,  -28, -4, -4, -28, -20, 19},
            new[]{-110, -146,  -20, -6, -6, -20, -146, -110},
            new[]{501, -110, 20, 24, 23, 20, -110, 501}
        };

        public AIPlayer(GameBoardWithEvents gameBoard)
        {
            _gameBoard = gameBoard;
        }
        
        public void MakeMove()
        {
            var availableCells = _gameBoard.GetAvailableCells();

            if (availableCells.Count == 0)
            {
                _gameBoard.Pass();
                return;
            }

            var bestMove = availableCells[0];
            var bestScore = int.MaxValue;

            foreach (var move in availableCells)
            {
                var cellsBeforeMove = MakeShallowCopy(_gameBoard.GetCells());
                _gameBoard.MakeMove(move);
                var score = MiniMax(3, int.MinValue, int.MaxValue, true);
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }

                _gameBoard.UndoMove(cellsBeforeMove);
            }

            Console.WriteLine(bestMove);
            Console.WriteLine(bestScore);
            _gameBoard.MakeMove(bestMove);
        }

        public int MiniMax(int depth, int alpha, int beta, bool isMinimizing)
        {
            if(depth == 0 || _gameBoard.IsGameFinished())
            {
                return EvalFunc(_gameBoard.GetCells());
            }

            return isMinimizing ? Min(depth, alpha, beta) : Max(depth, alpha, beta);
        }
        
        private int Min(int depth, int alpha, int beta)
        {
            var bestScore = int.MaxValue;
            var availableMoves = _gameBoard.GetAvailableCells();

            if (availableMoves.Count == 0)
            {
                var cellsBeforeMove = MakeShallowCopy(_gameBoard.GetCells());
                _gameBoard.PassWithoutMassage();
                var score = MiniMax(depth - 1, alpha, beta, false);
                bestScore = GetMin(score, bestScore);
                _gameBoard.UndoMove(cellsBeforeMove);
            }
            else
            {

                foreach (var move in availableMoves)
                {
                    var cellsBeforeMove = MakeShallowCopy(_gameBoard.GetCells());
                    _gameBoard.MakeMove(move);
                    var score = MiniMax(depth - 1, beta, alpha, false);
                    bestScore = GetMin(score, bestScore);
                    beta = GetMin(beta, bestScore);

                    _gameBoard.UndoMove(cellsBeforeMove);

                    if (beta <= alpha)
                        break;
                }
            }
            //Console.WriteLine(bestScore);
            return bestScore;
        }
        
        private int Max(int depth, int alpha, int beta)
        {
            var bestScore = int.MinValue;
            var availableMoves = _gameBoard.GetAvailableCells();

            if (availableMoves.Count == 0)
            {
                var cellsBeforeMove = MakeShallowCopy(_gameBoard.GetCells());
                _gameBoard.PassWithoutMassage();
                var score = MiniMax(depth - 1, alpha, beta, true);
                bestScore = GetMax(score, bestScore);
                _gameBoard.UndoMove(cellsBeforeMove);
            }
            else
            {
                foreach (var move in availableMoves)
                {
                    var cellsBeforeMove = MakeShallowCopy(_gameBoard.GetCells());
                    _gameBoard.MakeMove(move);
                    var score = MiniMax(depth - 1, beta, alpha, true);
                    bestScore = GetMax(score, bestScore);
                    alpha = GetMax(beta, bestScore);

                    _gameBoard.UndoMove(cellsBeforeMove);

                    if (beta <= alpha)
                        break;
                }
            }
            //Console.WriteLine(bestScore);
            return bestScore;
        }

        private int EvalFunc(List<List<Cell>> cells) {
            var score = 0;
            const CellState playerColor = CellState.White;
            
            for (int i = 0; i < 8; i++) {
                for (int j = 0; j < 8; j++) {
                    if (cells[i][j].State == playerColor) {
                        score += _sev[i][j];
                    }
                }
            }
            return score;
        }

        private static int GetMin(int firstValue, int secondValue) => 
            firstValue < secondValue ? firstValue : secondValue;
        
        private static int GetMax(int firstValue, int secondValue) =>
            firstValue > secondValue ? firstValue : secondValue;
        
        private List<List<Cell>> MakeShallowCopy(List<List<Cell>> list) => 
            list.Select(row => row.Select(cell => new Cell(cell.State)).ToList()).ToList();
        
    }
}