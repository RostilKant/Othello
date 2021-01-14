using System;
using System.Collections.Generic;

namespace Models
{
    public class GameBoardWithEvents: Game
    {
        private readonly CellState _firstPlayerColor = CellState.Black;
        private readonly CellState _secondPlayerColor = CellState.White;
        public CellState CurrentPlayerColor;

        private Board _board;
        
        public int PassedMovesCount = 0;
        public event Action<List<List<Cell>>> MoveMade;

        public event Action<List<List<Cell>>> GameStarted;

        public event Action<List<Tuple<int, int>>> AvailableCellsCalculated;

        public event Action<int, int> GameFinished;

        public event Action<int, int> ScoresCalculated;

        public event Action GameRestarted;

        public event Action MovePassed;
        public GameBoardWithEvents()
        {
            _board = new Board();
        }

        public List<List<Cell>> GetCells()
        {
            return _board.Cells;
        }

        public void UndoMove(List<List<Cell>> cellsBeforeMove)
        {
            _board = new Board(cellsBeforeMove);
            SwitchPlayer();
        }

        public List<Tuple<int, int>> GetAvailableCells()
        {
            var availableCells = Game.GetAvailableCells(CurrentPlayerColor, _board.Cells);
            return availableCells;
        }

        public bool IsGameFinished()
        {
            return Game.IsFull(_board.Cells)||PassedMovesCount >= 2;
        }

        public bool IsFirstPlayerWon()
        {
            var firstPlayerCellsCount = Game.CalculatePlayersScore(_firstPlayerColor, _board.Cells);
            var secondPlayerCellsCount = Game.CalculatePlayersScore(_secondPlayerColor, _board.Cells);
            return firstPlayerCellsCount < secondPlayerCellsCount;
        }

        public void MakeMove(Tuple<int, int> coords)
        {
            if (GetAvailableCells().Count == 0)
            {
                //Console.WriteLine(" -------------- GameManager.MakeMove() PASS");
                Pass();
            }
            else
            {
                PassedMovesCount = 0;
            }
            
            var cells = MarkCell(CurrentPlayerColor, coords, _board.Cells);
            MoveMade?.Invoke(cells);
            SwitchPlayer();
            AvailableCellsCalculated?.Invoke(GetAvailableCells());
            CalculatePlayersScore(cells);
            if (IsFull(cells)||PassedMovesCount==2)
            {
                FinishGame(cells);
                return;
            }
            _board = new Board(cells);
        }
        public void RestartGame(Tuple<int, int> blackHoleCoords)
        {
            _board = new Board();
            _board.SetBlackHole(blackHoleCoords);
            GameRestarted?.Invoke();
        }

        public void StartGame(Tuple<int, int> blackHoleCoords)
        {
            CurrentPlayerColor = _firstPlayerColor;

            _board.SetBlackHole(blackHoleCoords);

            GameStarted?.Invoke(_board.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
        }

        public void StartGame(Tuple<int, int> blackHoleCoords, Tuple<int, int> firstMove)
        {
            CurrentPlayerColor = _firstPlayerColor;

            _board.SetBlackHole(blackHoleCoords);

            GameStarted?.Invoke(_board.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
            MakeMove(firstMove);
        }

        public void CalculatePlayersScore(List<List<Cell>> cells)
        {
            ScoresCalculated?.Invoke(Game.CalculatePlayersScore(_firstPlayerColor, cells), Game.CalculatePlayersScore(_secondPlayerColor, cells));
        }

        public void FinishGame(List<List<Cell>> cells)
        {
            var firstPlayerCellsCount = Game.CalculatePlayersScore(_firstPlayerColor, cells);
            var secondPlayerCellsCount = Game.CalculatePlayersScore(_secondPlayerColor, cells);
            GameFinished?.Invoke(firstPlayerCellsCount, secondPlayerCellsCount);
        }
        
        public void Pass()
        {
            PassedMovesCount += 1;
            SwitchPlayer();
            MovePassed?.Invoke();
        }

        public void PassWithoutMassage()
        {
            PassedMovesCount += 1;
            SwitchPlayer();
        }
        public void SwitchPlayer()
        {
            CurrentPlayerColor = CurrentPlayerColor == _firstPlayerColor ? _secondPlayerColor : _firstPlayerColor;
        }
    }
}