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
            var availableCells = _board.GetAllAvailableCells;
            Console.WriteLine("\n  0 1 2 3 4 5 6 7");
            for (var x = 0; x < field.GetLength(0); x++)
            {
                Console.Write($"{x}");

                for (var y = 0; y < field.GetLength(1); y++)
                {
                    switch (availableCells)
                    {
                        case null when _board.CurrentPlayer.State == CellState.Black:
                        {
                            if ((y == 4 && x == 5) || (y == 5 && x == 4) || (y == 3 && x == 2) || (y == 2 && x == 3))
                            {
                                Console.Write("|+");
                            }
                            else
                            {
                                Console.Write( $"|{V(field[y, x])}");
                            }

                            break;
                        }
                        case null when _board.CurrentPlayer.State == CellState.White:
                        {
                            if (y == 5 && x == 5)
                            {
                                Console.Write("|+");
                            }
                            else
                            {
                                Console.Write( $"|{V(field[y, x])}");
                            }

                            break;
                        }
                        default:
                        {
                            if (availableCells.Contains((y, x)))
                            {
                                Console.Write("|+");
                                availableCells.Remove((y, x));
                            }
                            else
                            {
                                Console.Write($"|{V(field[y, x])}");
                            }
                            // Console.Write( ? "|+" : $"|{V(field[y, x])}");
                            break;
                        }
                    }
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