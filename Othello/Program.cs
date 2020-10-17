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
            IPlayer firstPlayer;
            IPlayer secondPlayer;
            var game = new GameBoardWithEvents();
            var output = new ConsoleOutput();
            output.ListenTo(game);
            
            var input = new ConsoleInput();
            input.ReadCommands(game);
        }
    }
}
