using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Models
{
    public class GameBoard
    {
        Random _r = new Random();

        private const int FieldSize = 8;
        
        private static readonly List<(int, int)> Directions = 
            new List<(int, int)> {(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)};

        protected  IPlayer FirstPlayer { get; set; }
        protected  IPlayer SecondPlayer { get; set; }

        private Cell[,] _field;
        
        public IPlayer CurrentPlayer { get; set; }
        public string Winner { get; set; }
        
        public Cell[,] Field => _field.Clone() as Cell[,];

        public CellState GetCellValue(int x, int y) => _field[x, y].State;

        public List<(int, int)> GetAllAvailableCells { get; private set; }
        

        public virtual void StartGame(string choosePlayer)
        {
            FirstPlayer = new HumanPlayer(CellState.Black);

            if (choosePlayer.ToLower() == "computer")
            {
                SecondPlayer = new ComputerPlayer(CellState.White);
            }
            else
            {
                SecondPlayer = new HumanPlayer(CellState.White);
            }

            CurrentPlayer = FirstPlayer;
            PrepareField();
        }
        
        public virtual void RestartGame()
        {
            _field = new Cell[FieldSize,FieldSize];
            CurrentPlayer = FirstPlayer;
            PrepareField();
        }

        protected virtual void PrepareField()
        {
            _field = new Cell[FieldSize,FieldSize];
            for (var x = 0; x < _field.GetLength(0); x++)
            {
                for (var y = 0; y < _field.GetLength(1); y++)
                {
                    _field[x, y] = new Cell(CellState.Empty);
                }
            }
            
            CreateBlackHole();
            
            _field[3,3].State = CellState.White;
            _field[4,4].State = CellState.White;
            _field[3,4].State = CellState.Black;
            _field[4,3].State = CellState.Black;

            void CreateBlackHole()
            {
                int x, y = 0;
                do
                {
                    x = _r.Next(0, 7);
                    y = _r.Next(0, 7);
                } while (x == 3 || x == 4 || y == 3 || y == 4);
                
                _field[x, y].State = CellState.BlackHole;
            }
        }
        
        public void MakeMove((int, int) coords)
        {
            GetAllAvailableCells ??= GetAvailableCells(GetOppositeColor(CurrentPlayer.State));
            
            /*if (!GetAllAvailableCells.Contains(coords))
            {
                WrongCellInput();
                return;
            }*/

            if (CurrentPlayer is HumanPlayer)
            {
                MarkCell(coords, CurrentPlayer);
                //MarkCell(GetAllAvailableCells[_r.Next(0, GetAllAvailableCells.Count)], CurrentPlayer);

                CalculatePlayersScore();
                SwitchPlayer();
            }

            if (ChangeMove())
            {
                return;
            }
            
            if (CurrentPlayer is ComputerPlayer)
            {
                MarkCell(GetAllAvailableCells[_r.Next(0, GetAllAvailableCells.Count)], CurrentPlayer);
                CalculatePlayersScore();
                SwitchPlayer();
            }
            
            if (GetAvailableCells(GetOppositeColor(CurrentPlayer.State)).Count == 0 && 
                GetAvailableCells(CurrentPlayer.State).Count == 0)
            {
                FinishGame();
            }

            bool ChangeMove()
            {
                if (GetAllAvailableCells.Count == 0)
                {
                    SwitchPlayer();
                    return true;
                }

                return false;
            }
        }

        protected virtual void WrongCellInput()
        {
            
        }

        protected virtual void MarkCell((int, int) coords, IPlayer player)
        {
            if (!IsLegalCoords(coords)) return;
            
            _field[coords.Item1, coords.Item2].State = player.State;
            UpdateField(player.State, coords);
            GetAllAvailableCells = GetAvailableCells(CurrentPlayer.State);
        }

        private void UpdateField(CellState color, (int, int) coords)
        {
            foreach(var direction in Directions) {
                if (IsDirectionAvailable(color,coords.Item1, coords.Item2,
                    direction.Item1, direction.Item2)) {
                    var currentRow = coords.Item1;
                    var currentCol = coords.Item2;
                    var rowDirection = direction.Item1;
                    var colDirection = direction.Item2;
                    do {
                        currentRow += rowDirection;
                        currentCol += colDirection;

                        if (currentRow < 0 || currentRow > _field.GetLength(0) - 1 || 
                            currentCol < 0 || currentCol > _field.GetLength(0) - 1) {
                            break;
                        }

                        if (_field[currentRow,currentCol].State == color || 
                            _field[currentRow,currentCol].State == CellState.BlackHole) {
                            break;
                        }

                        _field[currentRow, currentCol].State = color;

                    } while (true);
                }
            }
            GetAllAvailableCells = GetAvailableCells(GetOppositeColor(CurrentPlayer.State));
        }

        public List<(int, int)> GetAvailableCells(CellState state){
            var availableCells = new List<(int, int)>();
            
            for (var i = 0; i < _field.GetLength(0); i++) {
                for (var j = 0; j < _field.GetLength(1); j++)
                { 
                    if (IsCellAvailable(GetOppositeColor(state), i,j)) {
                        availableCells.Add((i,j));
                        //Console.Write($"{i} {j} --");
                    }
                }
            }
            return availableCells;
        }

        private bool IsCellAvailable(CellState state, int row, int column)
        {
            if (_field[row, column].State != CellState.Empty)
            {
                return false;
            }

            if (_field[row, column].State == CellState.BlackHole)
            {
                return false;
            }

            return Directions.Any(direction => 
                IsDirectionAvailable(state, row, column, direction.Item1, direction.Item2));
        }

        private bool IsDirectionAvailable(CellState state, int row, int column, int rowDirection, int colDirection)
        { 
            var oppositeColorEncountered = false;
            do
            {
                row += rowDirection;
                column += colDirection;
                
                if (row < 0 || row > _field.GetLength(0) - 1 || column < 0 || column > _field.GetLength(0) - 1) {
                    return false;
                }

                if (IsCellEmpty(row, column)) {
                    return false;
                } else if (_field[row,column].State == state) {
                    return oppositeColorEncountered;
                } else if (_field[row,column].State == CellState.BlackHole)
                {
                    return false;
                }
                else {
                    oppositeColorEncountered = true;
                }
            } while (true);
        }

        private bool IsCellEmpty(int row, int column)
        {
            return _field[row, column].State == CellState.Empty;
        }
        
        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == FirstPlayer ? SecondPlayer : FirstPlayer;
        }
        
        public static CellState GetOppositeColor(CellState color)
        {
            return color switch
            {
                CellState.Black => CellState.White,
                CellState.White => CellState.Black,
                CellState.BlackHole => CellState.BlackHole,
                _ => CellState.Empty
            };
        }
        
        public bool IsFull()
        {
            for (int i = 0; i < _field.GetLength(0); i++)
            {
                for (int j = 0; j < _field.GetLength(1); j++)
                {
                    if (_field[i,j].State == CellState.Empty)
                        return false;
                }
            }
            return true;
        }

        public virtual void CalculatePlayersScore()
        { }

        public int CountPlayerCells(CellState state)
        {
            var counter = 0;
            for (int i = 0; i < FieldSize; i++)
            {
                for (int j = 0; j < FieldSize; j++)
                {
                    if (_field[i,j].State == state)
                    {
                        counter++;
                    }
                }
            }

            return counter;
        }
        
        public virtual void FinishGame()
        {
            Winner = CountPlayerCells(FirstPlayer.State) < 
                     CountPlayerCells(GetOppositeColor(SecondPlayer.State)) ? "WHITES" : "BLACK";
        }

        private bool IsLegalCoords((int, int) coords)
        {
            if(coords.Item1 > 7 || coords.Item1 < 0 || coords.Item2 > 7 || coords.Item2 < 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.WriteLine("PLEASE, input the correct coordinates");
                Console.ResetColor();
                return false;
            }
            return true;
        }
    }
}