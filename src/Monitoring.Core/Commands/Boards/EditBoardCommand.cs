namespace Monitoring.Core.Commands.Boards
{
    public class EditBoardCommand : Command
    {
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public int NumberOfPulses { get; set; }
        public string[] Pulses { get; set; }
    }
}
