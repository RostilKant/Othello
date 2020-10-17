namespace Models
{
    public class ComputerPlayer: IPlayer
    {
        public ComputerPlayer(CellState state)
        {
            State = state;
        }
        public CellState State { get; set; }
    }
}