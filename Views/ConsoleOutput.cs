using System;
using Models;

namespace Views
{
    public class ConsoleOutput
    {
        private GameBoardWithEvents _board;

        public void ListenTo(GameBoardWithEvents board)
        {
            _board = board;
            _board.GameStarted += OnGameStarted;
            _board.FieldUpdated += OnFieldUpdated;
        }

        private void OnFieldUpdated(Cell[,] field)
        {
            Console.WriteLine("  0 1 2 3 4 5 6 7");
            for (var x = 0; x < field.GetLength(0); x++)
            {
                Console.Write($"{x}");

                for (var y = 0; y < field.GetLength(1); y++)
                {
                    Console.Write($"|{V(field[y,x])}");
                }
                Console.Write($"| {x}");
                Console.WriteLine();
            }
            Console.WriteLine("  0 1 2 3 4 5 6 7");

            string V(Cell cell)
            {
                return cell.State switch
                {
                    CellState.Empty => " ",
                    CellState.Black => "B",
                    CellState.White => "W",
                    _ => "X"
                };
            }
        }

        private void OnGameStarted(Cell[,] field)
        {
            Console.Out.WriteLine("Game is started! Make your first move with MOVE X Y");
        }
        
        
    }
}