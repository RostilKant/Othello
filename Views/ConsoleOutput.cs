using System;
using System.Collections.Generic;
using Models;

namespace Views
{
    public class ConsoleOutput
    {
        private List<List<Cell>> _cells;
        public void ListenTo(GameBoardWithEvents gameboard)
        {
            gameboard.MoveMade += DrawBoard;
            gameboard.GameStarted += SetCells;
            gameboard.AvailableCellsCalculated += OnFieldUpdated;
            gameboard.GameFinished += ShowFinish;
            gameboard.ScoresCalculated += PlayersScoresCalculated;
            // gameManager.WrongCellInputed += OnWrongCellInputed;
            
            gameboard.ChangingTurn += ShowPassMessage;
            
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
            
            Console.Out.Write("If you want play against AI type");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.Write(" START AI\n");
            Console.ResetColor();
            
            
        }

        private static void ShowPassMessage()
        {
            Console.WriteLine("pass");
        }

        private static void ShowFinish(int firstPlayerScore, int secondPlayerScore)
        {
            Console.WriteLine($"***GG***");
            if (firstPlayerScore > secondPlayerScore)
                Console.WriteLine("WHITES WON");
            else if (secondPlayerScore > firstPlayerScore)
                Console.WriteLine("BLACKS WON");
            else
                Console.WriteLine("TIE");
        }

        private static void PlayersScoresCalculated(int firstPlayerScore, int secondPlayerScore)
        {
            Console.WriteLine($" BLACKS: {firstPlayerScore}\n WHITES: {secondPlayerScore}");
        }

        private void SetCells(List<List<Cell>> cells)
        {
            _cells = cells;
        }

        private void DrawBoard(List<List<Cell>> cells)
        {
            _cells = cells;
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