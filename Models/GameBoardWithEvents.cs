using System;

namespace Models
{
    public class GameBoardWithEvents: GameBoard
    {
        public event Action<Cell[,]> GameStarted;

        public GameBoardWithEvents()
        {
            
        }

        protected override void PrepareField()
        {
            base.PrepareField();
            GameStarted?.Invoke(Field);
        }
    }
}