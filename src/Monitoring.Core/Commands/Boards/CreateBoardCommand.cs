using Monitoring.Core.Dtos;

namespace Monitoring.Core.Commands.Boards
{
    public class CreateBoardCommand : Command
    {
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public int NumberOfPulses { get; set; }
        public List<PulsDto> Pulses { get; set; }
    }
}
