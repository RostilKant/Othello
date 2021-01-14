using System;
using System.Collections.Generic;
using Models;

namespace Views
{
    public class ConsoleOutput
    {
        /*
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
            _board.ScoresCalculated += OnScoresCalculated;
            _board.MoveMade += OnFieldUpdated;
            // _board.GamePreparation += OnGamePreparation;
        }

       

        private void OnScoresCalculated(int firstPlayerScore,int secondPlayerScore )
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"BLACK: {firstPlayerScore}\n" +
                              $"WHITE: {secondPlayerScore}");
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
        

        private void OnFieldUpdated(List<List<Cell>> field)
        {
            var availableCells = _board.GetAvailableCells();
            Console.WriteLine("\n  0 1 2 3 4 5 6 7");
            for (var x = 0; x < field.Count; x++)
            {
                Console.Write($"{x}");

                for (var y = 0; y < field[x].Count; y++)
                {
                    switch (availableCells)
                    {
                        case null when _board.CurrentPlayerColor == CellState.Black:
                        {
                            if ((y == 4 && x == 5) || (y == 5 && x == 4) || (y == 3 && x == 2) || (y == 2 && x == 3))
                            {
                                Console.Write("|+");
                            }
                            else
                            {
                                Console.Write( $"|{V(field[y][x])}");
                            }

                            break;
                        }
                        case null when _board.CurrentPlayerColor == CellState.White:
                        {
                            if (y == 5 && x == 5)
                            {
                                Console.Write("|+");
                            }
                            else
                            {
                                Console.Write( $"|{V(field[y][x])}");
                            }

                            break;
                        }
                        default:
                        {
                            if (availableCells.Contains((new Tuple<int, int>(y,x))))
                            {
                                Console.Write("|+");
                            }
                            else
                            {
                                Console.Write($"|{V(field[y][x])}");
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
                    CellState.BlackHole => "X",
                    _ => "?"
                };
            }
        }

        private void OnGameStarted(List<List<Cell>> board)
        {
            Console.Out.Write("Game is started, field is cleared! Make your first move with");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" MOVE");
            Console.ResetColor();
            Console.Out.Write(" X Y\n");
        }
        */
        
         private List<List<Cell>> _cells;
        public void ListenTo(GameBoardWithEvents gameManager)
        {
            gameManager.MoveMade += DrawField;
            gameManager.GameStarted += SetCells;
            gameManager.AvailableCellsCalculated += OnFieldUpdated;
            gameManager.GameFinished += ShowFinish;
            gameManager.ScoresCalculated += ShowScores;
            // gameManager.WrongCellInputed += OnWrongCellInputed;
            
            gameManager.MovePassed += ShowPassMessage;
            
            GamePreparation();
        }

        private void GamePreparation()
        {
            Console.Out.Write("And his name iiiiis OTHELLO!!!Tuturutuuu tuturutuuu... Type");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" START");   
            Console.ResetColor();
            Console.Out.Write(" to play\n");
            
            Console.Out.Write("If you want play against computer type");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" START COMPUTER\n");
            Console.ResetColor();
            
        }
        public void ShowPassMessage()
        {
            Console.WriteLine("pass");
        }

        public void ShowFinish(int firstPlayerScore, int secondPlayerScore)
        {
            Console.WriteLine($"***GG***");
            if (firstPlayerScore > secondPlayerScore)
                Console.WriteLine("WHITES WON");
            else if (secondPlayerScore > firstPlayerScore)
                Console.WriteLine("BLACKS WON");
            else
                Console.WriteLine("TIE");
        }

        public void ShowScores(int firstPlayerScore, int secondPlayerScore)
        {
            Console.WriteLine($"WHITES: {firstPlayerScore}\nBLACKS: {secondPlayerScore}");
        }

        public void SetCells(List<List<Cell>> cells)
        {
            this._cells = cells;
        }

        public void DrawField(List<List<Cell>> cells)
        {
            this._cells = cells;
        }
        
         private void OnFieldUpdated(List<Tuple<int, int>> availableCells)
        {
            Console.WriteLine("\n  0 1 2 3 4 5 6 7");
            for (var x = 0; x < _cells.Count; x++)
            {
                Console.Write($"{x}");

                for (var y = 0; y < _cells[x].Count; y++)
                {
                    switch (availableCells)
                    {
                        default:
                        {
                            if (availableCells.Contains((new Tuple<int, int>(y,x))))
                            {
                                Console.Write("|+");
                            }
                            else
                            {
                                Console.Write($"|{V(_cells[y][x])}");
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
                    CellState.BlackHole => "X",
                    _ => "?"
                };
            }
        }
         
         private void OnWrongCellInputed()
         {
             Console.ForegroundColor = ConsoleColor.Red;
             Console.Out.WriteLine("You inputed incorrect cell coord, please choose one with '+'");
             Console.ResetColor();
         }


    }
}