namespace Models
{
    public class GameBoard
    {
        private const int FieldSize = 8;

        private readonly Player firstPlayer;
        private readonly Player secondPlayer;

        private Cell[,] field;
        
        public Player CurrentPlayer { get; set; }
        public Player Winner { get; set; }
        
        public bool isEnded { get; private set; }

        public Cell[,] Field => field.Clone() as Cell[,];

        public CellState GetCellValue(int x, int y) => field[x, y].State;

        public GameBoard()
        {
            
        }

        public void StartGame()
        {
            CurrentPlayer = firstPlayer;
            PrepareField();
        }

        protected virtual void PrepareField()
        {
            field = new Cell[FieldSize,FieldSize];
            for (var x = 0; x < field.GetLength(0); x++)
            {
                for (var y = 0; y < field.GetLength(1); y++)
                {
                    field[x, y] = new Cell(CellState.Empty);
                }
            }
            field[3,3].State = CellState.White;
            field[4,4].State = CellState.White;
            field[3,4].State = CellState.Black;
            field[4,3].State = CellState.Black;
        }
        
    }
}