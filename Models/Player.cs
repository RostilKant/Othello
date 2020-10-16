namespace Models
{
    public class Player
    {
        public CellState State { get; set; }

        public Player(CellState state)
        {
            State = state;
        }
        
    }
}