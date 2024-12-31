namespace Monitoring.Core.Dtos
{
    public class PulsDto
    {
        public string Name { get; set; }
        public string BoardInput { get; set; }
        public int NumberofTrackingRange { get; set; }
        public string TrackingRange { get; set; } //JsonData, every range has a name, starter range,End range and a ColorCode
        public int IgnoreDurationThreshold { get; set; } //seconds
        public int IgnoredDifferenceThreshold { get; set; }
        public PulseTypeDto PulseType { get; set; }
        public PulsenatureDto Nature { get; set; }

    }
}
