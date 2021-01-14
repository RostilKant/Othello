using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Models;

namespace Models
{
     public class Game
    {
        private static readonly List<Tuple<int, int>> Directions = new List<Tuple<int, int>>()
        {
            new Tuple<int, int>(-1, -1),
            new Tuple<int, int>(-1, 0),
            new Tuple<int, int>(-1, 1),
            new Tuple<int, int>(0, -1),
            new Tuple<int, int>(0, 1),
            new Tuple<int, int>(1, -1),
            new Tuple<int, int>(1, 0),
            new Tuple<int, int>(1, 1)
        };

        public static List<List<Cell>> MarkCell(CellState playerColor, Tuple<int, int> coords, List<List<Cell>> cells)
        {
            var cellsCopy = new List<List<Cell>>(cells);
            cellsCopy[coords.Item1][coords.Item2].State = playerColor;
            cellsCopy = UpdateField(playerColor, coords, cellsCopy);
            return cellsCopy;
        }

    
        private static List<List<Cell>> UpdateField(CellState playerColor, Tuple<int, int> coords,
            IEnumerable<List<Cell>> cells)
        {
            var cellsCopy = new List<List<Cell>>(cells);
            var row = coords.Item1;
            var column = coords.Item2;

            foreach (var direction in Directions.Where(direction => IsDirectionAvailable(playerColor, 
                row, column, direction.Item1, direction.Item2, cellsCopy)))
            {
                cellsCopy = MarkLine(playerColor, row, column, direction.Item1, direction.Item2,
                    cellsCopy);
            }

            return cellsCopy;
        }


        private static CellState GetOppositeColor(CellState color) => 
            color switch
            {
                CellState.Black => CellState.White,
                CellState.White => CellState.Black,
                _ => CellState.Empty
            };
        

        
        public static List<Tuple<int, int>> GetAvailableCells(CellState playerColor, List<List<Cell>> cells)
        {
            var availableCells = new List<Tuple<int, int>>();
            for (int i = 0; i < cells.Count; i++)
            {
                for (int j = 0; j < cells[i].Count; j++)
                {
                    var cell = cells[i][j];
                    if (cell.State == CellState.Empty && IsCellAvailable(playerColor, i, j, cells))
                    {
                        availableCells.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
            return availableCells;
        }
        
        public static int CalculatePlayersScore(CellState playerColor, List<List<Cell>> cells)
        {
            var counter = 0;
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    if (cell.State == playerColor)
                        counter++;
                }
            }
            return counter;
        }

        public static bool IsFull(IEnumerable<List<Cell>> cells)
        {
            foreach (var row in cells)
            {
                foreach (var cell in row)
                {
                    if (cell.State == CellState.Empty)
                        return false;
                }
            }
            return true;
        }

        private static List<List<Cell>> MarkLine(CellState playerColor, int row, int column, int rowDirection,
            int columnDirection, IEnumerable<List<Cell>> cells)
        {
            var cellsCopy = new List<List<Cell>>(cells);
            row += rowDirection;
            column += columnDirection;
            while (IsLegalCoords(row, column, cellsCopy) &&
                   cellsCopy[row][column].State == GetOppositeColor(playerColor))
            {
                cellsCopy[row][column].State = playerColor;
                row += rowDirection;
                column += columnDirection;
            }

            return cellsCopy;
        }
        
        private static bool IsCellAvailable(CellState playerColor, int row, int column,
            List<List<Cell>> cells)
        {
            return Directions.Any(direction => IsDirectionAvailable(playerColor, row, column, 
                direction.Item1, direction.Item2, cells));
        }


        private static bool IsDirectionAvailable(CellState playerColor, int row, int column, int rowDirection, int columnDirection,
            List<List<Cell>> cells)
        {
            row += rowDirection;
            column += columnDirection;
            var cellsInLine = 0;
            while (IsLegalCoords(row, column, cells) &&
                   cells[row][column].State == GetOppositeColor(playerColor))
            {
                cellsInLine += 1;
                row += rowDirection;
                column += columnDirection;
            }

            return (IsLegalCoords(row, column, cells) &&
                    cells[row][column].State == playerColor && cellsInLine > 0);
        }

        private static bool IsLegalCoords(int rowIndex, int columnIndex, List<List<Cell>> cells)
        {
            return (rowIndex >= 0 && columnIndex >= 0 && rowIndex < cells.Count && columnIndex < cells.Count);
        }
    }
}