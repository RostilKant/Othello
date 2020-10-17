namespace Models
{
    public class HumanPlayer: IPlayer
    {
        public CellState State { get; set; }

        public HumanPlayer(CellState state)
        {
            State = state;
        }
    }
}