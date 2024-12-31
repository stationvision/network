using static Monitoring.Db.Interfaces.Enums;

namespace NetworkCommunications.MethodTypes
{
    public struct ValidationResultItem
    {
        public bool InRange { get; set; }
        public long? Value { get; set; }
        public string Message { get; set; }
        public int RangeIndex { get; set; }
        public int IgnoredDifferenceThreshold { get; set; }
        public double IgnoredDurationThreshold { get; set; }
        public bool PulsHasChanged { get; set; }
        public PulsStatus RangeName { get; set; }
    }
    public struct GetRangeNameResultItem
    {
        public bool InRange { get; set; }
        public int RangeIndex { get; set; }
        public PulsStatus RangeName { get; set; }
        public string errorMessage { get; set; }

    }

    //public static class ValidationExtensions
    //{
    //    public static (bool InRange, int RangeIndex, BoardStatus RangeName, string errorMessage) ValidateInRangeHardCode(this string value, RangeHardCode data)
    //    {
    //        try
    //        {
    //            var convertedData = long.Parse(value);
    //            string[] ranges = { data.start.Range, data.stop.Range };
    //            string[] rangeNames = { "start", "stop" };
    //            int index = 0;

    //            foreach (string range in ranges)
    //            {
    //                string[] bounds = range.Split('-');
    //                int lowerBound = int.Parse(bounds[0]);
    //                int upperBound = int.Parse(bounds[1]);

    //                if (convertedData >= lowerBound && convertedData <= upperBound)
    //                {
    //                    return (InRange: true, RangeIndex: index, RangeName: index == 0 ? BoardStatus.Running : BoardStatus.Stopped, errorMessage: "");
    //                }
    //                index++;
    //            }

    //            return (InRange: false, RangeIndex: -1, RangeName: BoardStatus.None, errorMessage: "Not in any range");
    //        }
    //        catch (Exception ex)
    //        {
    //            return (InRange: false, RangeIndex: -1, RangeName: BoardStatus.None, errorMessage: ex.Message);
    //        }
    //    }


    //}
}
