using System;

namespace Models
{
    public class GameBoardWithEvents: GameBoard
    {
        public event Action<Cell[,]> GameStarted;

        public event Action<Cell[,]> FieldUpdated;

        public GameBoardWithEvents(Player firstPlayer, Player secondPlayer) : base(firstPlayer, secondPlayer)
        {
            
        }

        protected override void PrepareField()
        {
            base.PrepareField();
            GameStarted?.Invoke(Field);
            FieldUpdated?.Invoke(Field);
        }
        
        protected override void MarkCell((int, int) coords, Player player)
        {
            base.MarkCell(coords, player);
            FieldUpdated?.Invoke(Field);
        }
    }
}