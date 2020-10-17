using System;
using System.Collections.Generic;

namespace Models
{
    public class GameBoardWithEvents: GameBoard
    {
        public event Action<Cell[,]> GameStarted;

        public event Action<Cell[,]> FieldUpdated;

        public event Action<int, int> GameFinished;

        public event Action WrongCellInputed;

        public event Action<int, int> ScoresCalculated;

        public event Action GamePreparation;

        public event Action GameRestarted;
        public GameBoardWithEvents()
        {
            GamePreparation?.Invoke();
        }

        public override void StartGame(string choosePlayer)
        {
            base.StartGame(choosePlayer);
            GameStarted?.Invoke(Field);
        }

        protected override void PrepareField()
        {
            base.PrepareField();
            FieldUpdated?.Invoke(Field);
        }
        
        protected override void MarkCell((int, int) coords, IPlayer player)
        {
            base.MarkCell(coords, player);
            FieldUpdated?.Invoke(Field);
        }

        public override void RestartGame()
        {
            base.RestartGame();
            GameRestarted?.Invoke();
            // FieldUpdated?.Invoke(Field);
        }
        
        public override void CalculatePlayersScore()
        {
            ScoresCalculated?.Invoke(CountPlayerCells(FirstPlayer.State), CountPlayerCells(SecondPlayer.State));
        }

        public override void FinishGame()
        {
            base.FinishGame();
            GameFinished?.Invoke(CountPlayerCells(FirstPlayer.State), CountPlayerCells(SecondPlayer.State));
        }

        protected override void WrongCellInput()
        {
            WrongCellInputed?.Invoke();
            base.WrongCellInput();
        }
    }
}