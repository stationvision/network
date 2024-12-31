namespace NetworkCommunications.Dtos
{
    public class RequireDataDto
    {
        private static readonly object _lockObject = new object();
        private double? _DeviationTime;

        //public double? DeviationTime { get; set; }
        public bool IncreaseProductionTime { get; set; }
        public double? DeviationTime
        {
            get
            {
                lock (_lockObject)
                {
                    return _DeviationTime;
                }
            }
            set
            {
                lock (_lockObject)
                {
                    _DeviationTime = value;
                }
            }
        }
    }
}
