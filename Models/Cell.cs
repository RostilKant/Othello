namespace Models
{
    public enum CellState
    {
        Black,
        White,
        Empty,
        BlackHole
    }

    public class Cell
    {
        public CellState State { get; set; }

        public Cell(CellState state)
        {
            State = state;
        }
    }
}