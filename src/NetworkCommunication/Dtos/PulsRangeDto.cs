using static Monitoring.Db.Interfaces.Enums;

namespace NetworkCommunications.Dtos
{
    public class PulsRangeDto
    {
        public PulsStatus Name { get; set; }
        public int StartRange { get; set; }
        public int EndRange { get; set; }
        public int IgnoreDuration { get; set; }
        public int IgnoreDifference { get; set; }
    }

}
