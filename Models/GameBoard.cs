using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Models
{
    public class GameBoard
    {
        private const int FieldSize = 8;
        
        private static readonly List<(int, int)> Directions = 
            new List<(int, int)> {(-1, -1), (0, -1), (1, -1), (-1, 0), (1, 0), (-1, 1), (0, 1), (1, 1)};

        private readonly Player firstPlayer;
        private readonly Player secondPlayer;

        private Cell[,] field;
        
        public Player CurrentPlayer { get; set; }
        public Player Winner { get; set; }
        
        public bool isEnded { get; private set; }

        public Cell[,] Field => field.Clone() as Cell[,];

        public CellState GetCellValue(int x, int y) => field[x, y].State;

        public List<(int, int)> GetAllAvailableCells { get; private set; }

        public GameBoard(Player firstPlayer, Player secondPlayer)
        {
            this.firstPlayer = firstPlayer;
            this.secondPlayer = secondPlayer;
        }

        public void StartGame()
        {
            CurrentPlayer = firstPlayer;
            PrepareField();
        }

        protected virtual void PrepareField()
        {
            field = new Cell[FieldSize,FieldSize];
            for (var x = 0; x < field.GetLength(0); x++)
            {
                for (var y = 0; y < field.GetLength(1); y++)
                {
                    field[x, y] = new Cell(CellState.Empty);
                }
            }
            
            field[3,3].State = CellState.White;
            field[4,4].State = CellState.White;
            field[3,4].State = CellState.Black;
            field[4,3].State = CellState.Black;
        }
        
        public void MakeMove((int, int) coords)
        {
            MarkCell(coords, CurrentPlayer);
            SwitchPlayer();
        }

        protected virtual void MarkCell((int, int) coords, Player player)
        {
            field[coords.Item1, coords.Item2].State = player.State;
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

                        if (currentRow < 0 || currentRow > field.GetLength(0) - 1 || 
                            currentCol < 0 || currentCol > field.GetLength(0) - 1) {
                            break;
                        }

                        if (field[currentRow,currentCol].State == color) {
                            break;
                        }

                        field[currentRow, currentCol].State = color;

                    } while (true);
                }
            }
            GetAllAvailableCells = GetAvailableCells(GetOppositeColor(CurrentPlayer.State));
        }

        public List<(int, int)> GetAvailableCells(CellState state){
            var availableCells = new List<(int, int)>();
            
            for (var i = 0; i < field.GetLength(0); i++) {
                for (var j = 0; j < field.GetLength(1); j++)
                { 
                    if (IsCellAvailable(GetOppositeColor(state), i,j)) {
                        availableCells.Add((i,j));
                        // Console.Write($"{i} {j} --");
                    }
                }
            }
            return availableCells;
        }

        private bool IsCellAvailable(CellState state, int row, int column)
        {
            if (field[row, column].State != CellState.Empty)
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
                
                if (row < 0 || row > field.GetLength(0) - 1 || column < 0 || column > field.GetLength(0) - 1) {
                    return false;
                }

                if (IsCellEmpty(row, column)) {
                    return false;
                } else if (field[row,column].State == state) {
                    return oppositeColorEncountered;
                } else {
                    oppositeColorEncountered = true;
                }
            } while (true);
        }

        private bool IsCellEmpty(int row, int column)
        {
            return field[row, column].State == CellState.Empty;
        }
        
        private void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == firstPlayer ? secondPlayer : firstPlayer;
        }
        
        public static CellState GetOppositeColor(CellState color)
        {
            return color switch
            {
                CellState.Black => CellState.White,
                CellState.White => CellState.Black,
                _ => CellState.Empty
            };
        }
        
        public bool IsEnded()
        {
            for (int i = 0; i < field.GetLength(0); i++)
            {
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    if (field[i,j].State == CellState.Empty)
                        return false;
                }
            }
            return true;
        }
    }
}