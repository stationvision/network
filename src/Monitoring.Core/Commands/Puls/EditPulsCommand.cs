namespace Monitoring.Core.Commands.Puls
{
    public class EditPulsCommand : Command
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string pulseType { get; set; }
        public string BoardId { get; set; }
        public string BoardInput { get; set; }
        public string Nature { get; set; }
        public int NumberofTrackingRange { get; set; }
        public string TrackingRange { get; set; } //JsonData, every range has a name, starter range,End range and a ColorCode
        public int IgnorableDuration { get; set; } //seconds
        public int IgnoredDifferenceThreshold { get; set; }

    }
}
