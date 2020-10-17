using System;
using Models;

namespace Views
{
    public class ConsoleOutput
    {
        private GameBoardWithEvents _board;

        public ConsoleOutput()
        {
            GamePreparation();
        }
        public void ListenTo(GameBoardWithEvents board)
        {
            _board = board;
            _board.GameStarted += OnGameStarted;
            _board.GameFinished += OnGameFinish;
            _board.GameRestarted += OnGameRestarted;
            _board.FieldUpdated += OnFieldUpdated;
            _board.WrongCellInputed += OnWrongCellInputed;
            _board.ScoresCalculated += OnScoresCalculated;
            // _board.GamePreparation += OnGamePreparation;
        }

        private void GamePreparation()
        {
            Console.Out.Write("And his name iiiiis OTHELLO!!!Tuturutuuu tuturutuuu... Type");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" START");   
            Console.ResetColor();
            Console.Out.Write(" to play\n");
            
            Console.Out.Write("If you want play against computer input");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" START COMPUTER");
            Console.ResetColor();
            Console.Out.Write(" to play\n");

        }

        private void OnScoresCalculated(int firstPlayerScore,int secondPlayerScore )
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"BLACK: {firstPlayerScore}\n" +
                              $"WHITE: {secondPlayerScore}");
            Console.ResetColor();
        }

        private void OnWrongCellInputed()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Out.WriteLine("You inputed incorrect cell coord, please choose one with '+'");
            Console.ResetColor();
        }

        private void OnGameRestarted()
        {
            Console.Out.Write("Game is restarted, field is cleared! Make your first move with");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" MOVE");
            Console.ResetColor();
            Console.Out.Write(" X Y\n");
        }

        private void OnGameFinish(int arg1, int arg2)
        {
            Console.Out.WriteLine($"Good game! The {_board.Winner} WIN!!!GRAZ!\n" +
                                  $"If u want to play a new game input START");
            
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
            Console.Out.Write("Game is started, field is cleared! Make your first move with");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" MOVE");
            Console.ResetColor();
            Console.Out.Write(" X Y\n");
        }
        
        
    }
}