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
        

        public event Action ChangingTurn;
        
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
            return IsFull(_board.Cells)||PassedMovesCount >= 2;
        }
        public void MakeMove(Tuple<int, int> coords)
        {
            if (GetAvailableCells().Count == 0)
            {
                ChangedTurn();
                if (IsGameFinished())
                {
                    FinishGame(GetCells());
                }
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
        
        public void RestartGame()
        {
            _board = new Board();
            _board.SetBlackHole(GenerateBlackHoleCoords());
            GameRestarted?.Invoke();
        }

        public void StartGame()
        {
            CurrentPlayerColor = _firstPlayerColor;

            _board.SetBlackHole(GenerateBlackHoleCoords());

            GameStarted?.Invoke(_board.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
        }

        public void StartGame(Tuple<int, int> firstMove)
        {
            CurrentPlayerColor = _firstPlayerColor;

            _board.SetBlackHole(GenerateBlackHoleCoords());

            GameStarted?.Invoke(_board.Cells);

            var availableCells = GetAvailableCells();
            AvailableCellsCalculated?.Invoke(availableCells);
            MakeMove(firstMove);
        }

        private void CalculatePlayersScore(List<List<Cell>> cells)
        {
            ScoresCalculated?.Invoke(Game.CalculatePlayersScore(_firstPlayerColor, cells), Game.CalculatePlayersScore(_secondPlayerColor, cells));
        }

        private void FinishGame(List<List<Cell>> cells)
        {
            var firstPlayerCellsCount = Game.CalculatePlayersScore(_firstPlayerColor, cells);
            var secondPlayerCellsCount = Game.CalculatePlayersScore(_secondPlayerColor, cells);
            GameFinished?.Invoke(firstPlayerCellsCount, secondPlayerCellsCount);
        }
        public void ChangedTurn()
        {
            PassedMovesCount++;
            SwitchPlayer();
            //MovePassed?.Invoke();
        }

        private void SwitchPlayer()
        {
            CurrentPlayerColor = CurrentPlayerColor == _firstPlayerColor ? _secondPlayerColor : _firstPlayerColor;
        }

        private Tuple<int,int> GenerateBlackHoleCoords()
        {
            var r = new Random();
            int x;
            int y;
            do
            {
                x = r.Next(0, 7);
                y = r.Next(0, 7);
            } while (x == 3 || x == 4 || y == 3 || y == 4);

            return new Tuple<int, int>(x,y);
        }
    }
}