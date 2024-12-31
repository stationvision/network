namespace Monitoring.Core.Dtos
{
    public class BoardDto
    {
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public int NumberOfPulses { get; set; }
        public IEnumerable<PulsDto> Pulses { get; set; }
    }
}
