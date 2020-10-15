namespace Models
{
    public class Player
    {
        public CellState State { get; }

        public Player(CellState state)
        {
            State = state;
        }
    }
}