using Monitoring.Db.Models;
using NetworkCommunications.Extentions;
using NetworkCommunications.MethodTypes;
using System.Text.Json;
using static Monitoring.Db.Interfaces.Enums;
namespace NetworkCommunications.Dtos
{
    public class ClientToServerDto
    {
        public ClientToServerDto()
        {

        }
        public string Type { get; set; }
        public string BoardID { get; set; }
        public string ADC0 { get; set; }
        public string ADC1 { get; set; }
        public string ADC2 { get; set; }
        public string ADC3 { get; set; }
        public string ADC4 { get; set; }
        public string ADC5 { get; set; }
        public string ADC6 { get; set; }
        public string ADC7 { get; set; }
        public string ADC8 { get; set; }
        public string ADC9 { get; set; }
        public string ADC10 { get; set; }
        public string ADC11 { get; set; }
        public string ADC12 { get; set; }
        public string ADC13 { get; set; }
        public string ADC14 { get; set; }
        public string ADC15 { get; set; }
        public string ADC16 { get; set; }
        public string IPPart1 { get; set; }
        public string IPPart2 { get; set; }
        public string IPPart3 { get; set; }
        public string IPPart4 { get; set; }
        public string LEDColorW { get; set; }
        public string LEDColorR { get; set; }
        public string LEDColorG { get; set; }
        public string LEDColorB { get; set; }
        public string Status { get; set; }
        public string Month { get; set; }
        public string Date { get; set; }
        public string Year { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Second { get; set; }

        public class RangeHardCode
        {
            public Stop stop { get; set; }
            public Start start { get; set; }


            public class Stop
            {
                public string Range { get; set; }
                public int IgnoreDuration { get; set; }
                public int IgnoreDiffrence { get; set; }
            }

            public class Start
            {
                public string Range { get; set; }
                public int IgnoreDuration { get; set; }
                public int IgnoreDiffrence { get; set; }
            }

        }

        public ValidationResultItem GetRangeName(ClientPuls clientPuls)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BoardStatusConverter());
            var ranges = JsonSerializer.Deserialize<List<PulsRangeDto>>(clientPuls.puls.TrackingRange, options);

            var pulsvalue = long.Parse(clientPuls.Value);


            int index = 0;
            foreach (var range in ranges)
            {
                if (pulsvalue >= range.StartRange && pulsvalue <= range.EndRange)
                {
                    return new ValidationResultItem
                    {
                        InRange = true,
                        RangeIndex = index,
                        RangeName = range.Name,
                        Message = "",
                        IgnoredDurationThreshold = range.IgnoreDuration,
                        IgnoredDifferenceThreshold = range.IgnoreDifference,
                        Value = pulsvalue,
                    };
                }
                index++;
            }

            throw new Exception("Not in any range");
            return new ValidationResultItem
            {
                InRange = false,
                RangeIndex = -1,
                RangeName = PulsStatus.None,
                Message = "Not in the Range",
                Value = null,
            };
        }
        //public async Task<Dictionary<string, BoardStatus>> GetRangeName(ClientData clientData)
        //{
        //    var rangeconfig = new RangeHardCode()
        //    {
        //        stop = new RangeHardCode.Stop()
        //        {
        //            Range = "0-1500",
        //            IgnoreDuration = 0,
        //            IgnoreDiffrence = 1500
        //        },
        //        start = new RangeHardCode.Start()
        //        {
        //            Range = "1501-4095",
        //            IgnoreDuration = 120,
        //            IgnoreDiffrence = 2094
        //        }
        //    };

        //    var results = new Dictionary<string, BoardStatus>();
        //    var validADC0 = ValidationExtensions.ValidateInRangeHardCode(ADC0, rangeconfig);
        //    results["ADC0"] = validADC0.RangeName;

        //    var validADC1 = ValidationExtensions.ValidateInRangeHardCode(ADC1, rangeconfig);
        //    results["ADC1"] = validADC1.RangeName;

        //    var validADC2 = ValidationExtensions.ValidateInRangeHardCode(ADC2, rangeconfig);
        //    results["ADC2"] = validADC2.RangeName;

        //    var validADC3 = ValidationExtensions.ValidateInRangeHardCode(ADC3, rangeconfig);
        //    results["ADC3"] = validADC3.RangeName;

        //    var validADC4 = ValidationExtensions.ValidateInRangeHardCode(ADC4, rangeconfig);
        //    results["ADC4"] = validADC4.RangeName;

        //    var validADC5 = ValidationExtensions.ValidateInRangeHardCode(ADC5, rangeconfig);
        //    results["ADC5"] = validADC5.RangeName;

        //    var validADC6 = ValidationExtensions.ValidateInRangeHardCode(ADC6, rangeconfig);
        //    results["ADC6"] = validADC6.RangeName;

        //    var validADC7 = ValidationExtensions.ValidateInRangeHardCode(ADC7, rangeconfig);
        //    results["ADC7"] = validADC7.RangeName;

        //    var validADC8 = ValidationExtensions.ValidateInRangeHardCode(ADC8, rangeconfig);
        //    results["ADC8"] = validADC8.RangeName;

        //    var validADC9 = ValidationExtensions.ValidateInRangeHardCode(ADC9, rangeconfig);
        //    results["ADC9"] = validADC9.RangeName;

        //    var validADC10 = ValidationExtensions.ValidateInRangeHardCode(ADC10, rangeconfig);
        //    results["ADC10"] = validADC10.RangeName;

        //    var validADC11 = ValidationExtensions.ValidateInRangeHardCode(ADC11, rangeconfig);
        //    results["ADC11"] = validADC11.RangeName;

        //    var validADC12 = ValidationExtensions.ValidateInRangeHardCode(ADC12, rangeconfig);
        //    results["ADC12"] = validADC12.RangeName;

        //    var validADC13 = ValidationExtensions.ValidateInRangeHardCode(ADC13, rangeconfig);
        //    results["ADC13"] = validADC13.RangeName;

        //    var validADC14 = ValidationExtensions.ValidateInRangeHardCode(ADC14, rangeconfig);
        //    results["ADC14"] = validADC14.RangeName;

        //    var validADC15 = ValidationExtensions.ValidateInRangeHardCode(ADC15, rangeconfig);
        //    results["ADC15"] = validADC15.RangeName;

        //    var validADC16 = ValidationExtensions.ValidateInRangeHardCode(ADC16, rangeconfig);
        //    results["ADC16"] = validADC16.RangeName;


        //    string statusRange = "0-150,151-255";
        //    // Validate Status
        //    var validStatus = Status.ValidateInRange(statusRange);
        //    results["Status"] = validADC0.RangeName;

        //    return results;


        //}
        //public Dictionary<string, (bool InRange, int? Value, string Message, int RangeIndex, int IgnoredDifferenceThreshold, double IgnoredDurationThreshold, bool PulsHasChnaged, BoardStatus RangeName)> Validate(ClientToServerDto latestMessage)


        //public static ValidationResultItem ValidateADC(string adcValue, RangeHardCode rangeConfig, string latestValue)
        //{
        //    var validation = adcValue.ValidateInRangeHardCode(rangeConfig);
        //    return new ValidationResultItem
        //    {
        //        InRange = validation.InRange,
        //        Value = adcValue.ToInt(),
        //        Message = validation.errorMessage,
        //        RangeIndex = validation.RangeIndex,
        //        IgnoredDifferenceThreshold = validation.RangeName == BoardStatus.Stopped ? rangeConfig.stop.IgnoreDiffrence : rangeConfig.start.IgnoreDiffrence,
        //        IgnoredDurationThreshold = validation.RangeName == BoardStatus.Stopped ? rangeConfig.stop.IgnoreDuration : rangeConfig.start.IgnoreDuration,
        //        PulsHasChanged = adcValue != latestValue.ToString(),
        //        RangeName = validation.RangeName
        //    };
        //}


        //public Dictionary<string, ValidationResultItem> Validate(ClientToServerDto latestMessage)
        //{
        //    var results = new Dictionary<string, ValidationResultItem>();
        //    var rangeConfig = new RangeHardCode()
        //    {
        //        stop = new RangeHardCode.Stop()
        //        {
        //            Range = "0-1500",
        //            IgnoreDuration = 0,
        //            IgnoreDiffrence = 1500
        //        },
        //        start = new RangeHardCode.Start()
        //        {
        //            Range = "1501-4095",
        //            IgnoreDuration = 120,
        //            IgnoreDiffrence = 2094
        //        }
        //    };

        //    results["ADC0"] = ValidationExtensions.ValidateADC(ADC0, rangeConfig, latestMessage.ADC0);
        //    results["ADC1"] = ValidationExtensions.ValidateADC(ADC1, rangeConfig, latestMessage.ADC1);
        //    results["ADC2"] = ValidationExtensions.ValidateADC(ADC2, rangeConfig, latestMessage.ADC2);
        //    results["ADC3"] = ValidationExtensions.ValidateADC(ADC3, rangeConfig, latestMessage.ADC3);
        //    results["ADC4"] = ValidationExtensions.ValidateADC(ADC4, rangeConfig, latestMessage.ADC4);
        //    results["ADC5"] = ValidationExtensions.ValidateADC(ADC5, rangeConfig, latestMessage.ADC5);
        //    results["ADC6"] = ValidationExtensions.ValidateADC(ADC6, rangeConfig, latestMessage.ADC6);
        //    results["ADC7"] = ValidationExtensions.ValidateADC(ADC7, rangeConfig, latestMessage.ADC7);
        //    results["ADC8"] = ValidationExtensions.ValidateADC(ADC8, rangeConfig, latestMessage.ADC8);
        //    results["ADC9"] = ValidationExtensions.ValidateADC(ADC9, rangeConfig, latestMessage.ADC9);
        //    results["ADC10"] = ValidationExtensions.ValidateADC(ADC10, rangeConfig, latestMessage.ADC10);
        //    results["ADC11"] = ValidationExtensions.ValidateADC(ADC11, rangeConfig, latestMessage.ADC11);
        //    results["ADC12"] = ValidationExtensions.ValidateADC(ADC12, rangeConfig, latestMessage.ADC12);
        //    results["ADC13"] = ValidationExtensions.ValidateADC(ADC13, rangeConfig, latestMessage.ADC13);
        //    results["ADC14"] = ValidationExtensions.ValidateADC(ADC14, rangeConfig, latestMessage.ADC14);
        //    results["ADC15"] = ValidationExtensions.ValidateADC(ADC15, rangeConfig, latestMessage.ADC15);
        //    results["ADC16"] = ValidationExtensions.ValidateADC(ADC16, rangeConfig, latestMessage.ADC16);

        //    string statusRange = "0-150,151-255";
        //    var validStatus = Status.ValidateInRange(statusRange);
        //    results["Status"] = new ValidationResultItem
        //    {
        //        InRange = validStatus.InRange,
        //        Value = Status.ToInt(),
        //        Message = validStatus.errorMessage,
        //        RangeIndex = validStatus.RangeIndex,
        //        IgnoredDifferenceThreshold = validStatus.RangeName == BoardStatus.Stopped ? rangeConfig.stop.IgnoreDiffrence : rangeConfig.start.IgnoreDiffrence,
        //        IgnoredDurationThreshold = validStatus.RangeName == BoardStatus.Stopped ? rangeConfig.stop.IgnoreDuration : rangeConfig.start.IgnoreDuration,
        //        PulsHasChanged = Status != latestMessage.Status,
        //        RangeName = validStatus.RangeName
        //    };

        //    return results;
        //}


        public static Dictionary<string, (long? Difference, bool IsAcceptable)> Compare(ClientToServerDto model1, ClientToServerDto model2)
        {
            const double acceptableRange = 0.1;
            var differences = new Dictionary<string, (long? Difference, bool IsAcceptable)>();

            differences["ADC0"] = ((model1.ADC0.ToByte() - model2.ADC0.ToByte()), (model1.ADC0.ToByte() - model2.ADC0.ToByte()) <= acceptableRange);
            differences["ADC1"] = (model1.ADC1.ToByte() - model2.ADC1.ToByte(), (model1.ADC1.ToByte() - model2.ADC1.ToByte()) <= acceptableRange);
            differences["ADC2"] = (model1.ADC2.ToByte() - model2.ADC2.ToByte(), (model1.ADC2.ToByte() - model2.ADC2.ToByte()) <= acceptableRange);
            differences["ADC3"] = (model1.ADC3.ToByte() - model2.ADC3.ToByte(), (model1.ADC3.ToByte() - model2.ADC3.ToByte()) <= acceptableRange);
            differences["ADC4"] = (model1.ADC4.ToByte() - model2.ADC4.ToByte(), (model1.ADC4.ToByte() - model2.ADC4.ToByte()) <= acceptableRange);
            differences["ADC5"] = (model1.ADC5.ToByte() - model2.ADC5.ToByte(), (model1.ADC5.ToByte() - model2.ADC5.ToByte()) <= acceptableRange);
            differences["ADC6"] = (model1.ADC6.ToByte() - model2.ADC6.ToByte(), (model1.ADC6.ToByte() - model2.ADC6.ToByte()) <= acceptableRange);
            differences["ADC7"] = (model1.ADC7.ToByte() - model2.ADC7.ToByte(), (model1.ADC7.ToByte() - model2.ADC7.ToByte()) <= acceptableRange);
            differences["ADC8"] = (model1.ADC8.ToByte() - model2.ADC8.ToByte(), (model1.ADC8.ToByte() - model2.ADC8.ToByte()) <= acceptableRange);
            differences["ADC9"] = (model1.ADC9.ToByte() - model2.ADC9.ToByte(), (model1.ADC9.ToByte() - model2.ADC9.ToByte()) <= acceptableRange);
            differences["ADC10"] = (model1.ADC10.ToByte() - model2.ADC10.ToByte(), (model1.ADC10.ToByte() - model2.ADC10.ToByte()) <= acceptableRange);
            differences["ADC11"] = (model1.ADC11.ToByte() - model2.ADC11.ToByte(), (model1.ADC11.ToByte() - model2.ADC11.ToByte()) <= acceptableRange);
            differences["ADC12"] = (model1.ADC12.ToByte() - model2.ADC12.ToByte(), (model1.ADC12.ToByte() - model2.ADC12.ToByte()) <= acceptableRange);
            differences["ADC13"] = (model1.ADC13.ToByte() - model2.ADC13.ToByte(), (model1.ADC13.ToByte() - model2.ADC13.ToByte()) <= acceptableRange);
            differences["ADC14"] = (model1.ADC14.ToByte() - model2.ADC14.ToByte(), (model1.ADC14.ToByte() - model2.ADC14.ToByte()) <= acceptableRange);
            differences["ADC15"] = (model1.ADC15.ToByte() - model2.ADC15.ToByte(), (model1.ADC15.ToByte() - model2.ADC15.ToByte()) <= acceptableRange);

            differences["IPPart1"] = (0, model1.IPPart1 == model2.IPPart1);
            differences["IPPart2"] = (0, model1.IPPart2 == model2.IPPart2);
            differences["IPPart3"] = (0, model1.IPPart3 == model2.IPPart3);
            differences["IPPart4"] = (0, model1.IPPart4 == model2.IPPart4);

            differences["LEDColorW"] = (model1.LEDColorW.ToByte() - model2.LEDColorW.ToByte(), (model1.LEDColorW.ToByte() - model2.LEDColorW.ToByte()) <= acceptableRange);
            differences["LEDColorR"] = (model1.LEDColorR.ToByte() - model2.LEDColorR.ToByte(), (model1.LEDColorR.ToByte() - model2.LEDColorR.ToByte()) <= acceptableRange);
            differences["LEDColorG"] = (model1.LEDColorG.ToByte() - model2.LEDColorG.ToByte(), (model1.LEDColorG.ToByte() - model2.LEDColorG.ToByte()) <= acceptableRange);
            differences["LEDColorB"] = (model1.LEDColorB.ToByte() - model2.LEDColorB.ToByte(), (model1.LEDColorB.ToByte() - model2.LEDColorB.ToByte()) <= acceptableRange);
            differences["Status"] = (model1.Status.ToByte() - model2.Status.ToByte(), (model1.Status.ToByte() - model2.Status.ToByte()) <= acceptableRange);
            differences["Month"] = (model1.Month.ToByte() - model2.Month.ToByte(), (model1.Month.ToByte() - model2.Month.ToByte()) <= acceptableRange);
            differences["Date"] = (model1.Date.ToByte() - model2.Date.ToByte(), (model1.Date.ToByte() - model2.Date.ToByte()) <= acceptableRange);
            differences["Year"] = (model1.Year.ToByte() - model2.Year.ToByte(), (model1.Year.ToByte() - model2.Year.ToByte()) <= acceptableRange);
            differences["Hour"] = (model1.Hour.ToByte() - model2.Hour.ToByte(), (model1.Hour.ToByte() - model2.Hour.ToByte()) <= acceptableRange);
            differences["Minute"] = (model1.Minute.ToByte() - model2.Minute.ToByte(), (model1.Minute.ToByte() - model2.Minute.ToByte()) <= acceptableRange);
            differences["Second"] = (model1.Second.ToByte() - model2.Second.ToByte(), (model1.Second.ToByte() - model2.Second.ToByte()) <= acceptableRange);

            return differences;
        }


    }


}
