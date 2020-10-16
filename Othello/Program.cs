using System;
using System.Drawing;
using Controllers;
using Models;
using Views;

namespace Othello
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameBoardWithEvents(new Player(CellState.Black), new Player(CellState.White));
            var output = new ConsoleOutput();
            output.ListenTo(game);
            
            var input = new ConsoleInput();
            input.ReadCommands(game);
        }
    }
}
