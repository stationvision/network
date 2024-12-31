using Monitoring.Db.Models;
using NetworkCommunications.Dtos;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Monitoring.Db.Interfaces.Enums;

namespace NetworkCommunications.Extentions
{
    public static class Validator
    {
        //This class is used to convert the sending data from client to server 
        //رشته ارسال داده ازکالینت ها به رسور :
        public static bool IsFromBoardToServer(this string[] data)
        {
            try
            {
                var Type = data[0];
                if (Type == "E" || Type == "Z" || Type == "W")
                    return true;

                return false;

            }
            catch
            {
                return false;
            }
        }

        //IsSettingsFromServerToClient
        //رشته ارسال تنظیمات رسور به کالینت
        public static bool IsSettingsFromServerToClient(this string[] data)
        {
            try
            {
                if (data[0] != "55" && data[1] == "AA")
                    return false;


                return true;

            }
            catch
            {
                return false;
            }
        }

        //IsCommandsFromServerToClient
        //رشته ارسال دستورات رسور به کالینت
        public static bool IsCommandsFromServerToClientTOChangeStatus(this string[] data)
        {
            try
            {
                if (data[0] != "44" && data[1] == "BB")
                    return false;

                return true;

            }
            catch
            {
                return false;
            }
        }

        //IsTimeAndDateFromServerToClient
        //رشته ارسال زمان و تاریخ رسور به کالینت
        public static bool IsTimeAndDateFromServerToClient(this string[] data)
        {
            try
            {
                if (data[0] != "66" && data[1] == "99")
                    return false;

                return true;

            }
            catch
            {
                return false;
            }
        }

        //IsTimeoutSettingFromServerToClient
        //رشته ارسال تنظیمات تایم اوت رسور به کالینت
        public static bool IsTimeoutSettingFromServerToClient(this string[] data)
        {
            try
            {
                if (data[0] != "33" && data[1] == "CC")
                    return false;

                return true;

            }
            catch
            {
                return false;
            }
        }


        public static MessageType DetermineMessageType(this string[] data)
        {
            if (data.IsFromBoardToServer())
                return MessageType.FromBoardsToServer;
            //if (data.IsSettingsFromServerToClient())
            //    return MessageType.SettingsFromServerToClient;
            //if (data.IsCommandsFromServerToClientTOChangeStatus())
            //    return MessageType.CommandsFromServerToClientTOChangeStatus;
            //if (data.IsTimeAndDateFromServerToClient())
            //    return MessageType.TimeAndDateFromServerToClient;
            //if (data.IsTimeoutSettingFromServerToClient())
            //    return MessageType.TimeoutSettingFromServerToClient;

            return MessageType.Unknown;
        }
        public static MessageType DetermineMessageType(this string data)
        {
            var arraydata = data.Split(",");
            if (arraydata.IsFromBoardToServer())
                return MessageType.FromBoardsToServer;
            //if (data.IsSettingsFromServerToClient())
            //    return MessageType.SettingsFromServerToClient;
            //if (data.IsCommandsFromServerToClientTOChangeStatus())
            //    return MessageType.CommandsFromServerToClientTOChangeStatus;
            //if (data.IsTimeAndDateFromServerToClient())
            //    return MessageType.TimeAndDateFromServerToClient;
            //if (data.IsTimeoutSettingFromServerToClient())
            //    return MessageType.TimeoutSettingFromServerToClient;

            return MessageType.Unknown;
        }

        public static string GetBoardId(this string[] data)
        {
            try
            {
                if (data.IsFromBoardToServer())
                    //return data[0].Substring(1, data[0].Length - 1);
                    return data[1];
                if (data.IsSettingsFromServerToClient())
                    return string.Concat(data[20], data[21], data[22], data[23]);
                if (data.IsCommandsFromServerToClientTOChangeStatus())
                    return string.Concat(data[1], data[2], data[3], data[4]);
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string GetPulsDate(this string[] data)
        {
            try
            {
                if (data.IsFromBoardToServer())
                    return data[22].Trim() + " " + data[23].Trim(); //Date + time index in Array
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }


    //E,000000000,00000006,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,0000,192.168.001.001,100:080:255:255,055,05/04/24,00:04:36
    public static class ClientToServerConvertors
    {
        public static ClientToServerDto Deserialize(this string[] parts)
        {
            var type = parts[0];
            var ipParts = parts[19].Split('.');
            var ledColors = parts[20].Split(':');
            string BoardID = parts[1];
            var clientData = new ClientToServerDto
            {
                Type = type,
                BoardID = BoardID,
                ADC0 = parts[2],
                ADC1 = parts[3],
                ADC2 = parts[4],
                ADC3 = parts[5],
                ADC4 = parts[6],
                ADC5 = parts[7],
                ADC6 = parts[8],
                ADC7 = parts[9],
                ADC8 = parts[10],
                ADC9 = parts[11],
                ADC10 = parts[12],
                ADC11 = parts[13],
                ADC12 = parts[14],
                ADC13 = parts[15],
                ADC14 = parts[16],
                ADC15 = parts[17],
                ADC16 = parts[18],
                IPPart1 = ipParts[0],
                IPPart2 = ipParts[1],
                IPPart3 = ipParts[2],
                IPPart4 = ipParts[3],
                LEDColorW = ledColors[0],
                LEDColorR = ledColors[1],
                LEDColorG = ledColors[2],
                LEDColorB = ledColors[3],
                Status = parts[20],
                Month = DateTime.Parse(parts[22], CultureInfo.InvariantCulture).Month.ToString(),
                Date = DateTime.Parse(parts[22], CultureInfo.InvariantCulture).Day.ToString(),
                Year = DateTime.Parse(parts[22], CultureInfo.InvariantCulture).ToString("yy"),
                Hour = TimeSpan.Parse(RemoveInvalidCharacters(parts[23])).Hours.ToString(),
                Minute = TimeSpan.Parse(RemoveInvalidCharacters(parts[23])).Minutes.ToString(),
                Second = TimeSpan.Parse(RemoveInvalidCharacters(parts[23])).Seconds.ToString()

            };

            return clientData;
        }
        static string RemoveInvalidCharacters(string input)
        {
            // فقط اعداد، دو نقطه و نقطه را نگه می‌داریم
            char[] validCharacters = input.Where(c => char.IsDigit(c) || c == ':' || c == '.').ToArray();
            return new string(validCharacters);
        }
        public static ClientToServerDto Deserialize(this string data)
        {
            var parts = data.Split(',');
            var clientData = parts.Deserialize();
            return clientData;
        }

        public static string Serialize(this ClientToServerDto data)
        {
            var ledColors = string.Join(",",
                data.LEDColorW,
                data.LEDColorR,
                data.LEDColorG,
                data.LEDColorB);

            return $"{data.Type}{data.BoardID}," +
                   $"{data.ADC0.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC1.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC2.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC3.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC4.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC5.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC6.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC7.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC8.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC9.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC10.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC11.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC12.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC13.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC14.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC15.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.ADC16.ToString(CultureInfo.InvariantCulture)}," +
                   $"{data.IPPart1},{data.IPPart2},{data.IPPart3},{data.IPPart4}," +
                   $"{data.Status}," +
                   $"{data.Month:D2},{data.Date:D2},{data.Year:D2}," +
                   $"{data.Hour:D2},{data.Minute:D2},{data.Second:D2}," +
                   $"{ledColors}";
        }
        public static string ToJson(this string[] data)
        {

            var deserializedData = data.Deserialize();
            return JsonSerializer.Serialize(deserializedData, JsonSerializerConfig.DefaultOptions);
        }
        public static string ToJson(this string data)
        {
            var arraydata = data.Split(',');
            var deserializedData = arraydata.Deserialize();
            return JsonSerializer.Serialize(deserializedData, JsonSerializerConfig.DefaultOptions);
        }

        public static byte? ToByte(this string data)
        {
            try
            {
                return byte.Parse(data);
            }
            catch (ParsingException ex)
            {
                //TODO: Trigger OnMessageSentToErrorQueue
                Console.WriteLine($"Parse To Byte Error. {ex.Message}");
                return null;
            }
        }

        //If RangeIndex is -1, it means the data is not in the range
        //public static (bool InRange, int RangeIndex, string errorMessage, BoardStatus RangeName) ValidateInRange(this string data, string InrageValue)
        //{
        //    try
        //    {
        //        var convertedData = int.Parse(data);
        //        string[] ranges = InrageValue.Split(',');
        //        int index = 0;

        //        foreach (string range in ranges)
        //        {
        //            string[] bounds = range.Split('-');
        //            int lowerBound = int.Parse(bounds[0]);
        //            int upperBound = int.Parse(bounds[1]);

        //            if (convertedData >= lowerBound && convertedData <= upperBound)
        //            {
        //                return (InRange: true, RangeIndex: index, errorMessage: "", index == 0 ? BoardStatus.Stopped : BoardStatus.Running);
        //            }
        //            index++;
        //        }

        //        return (InRange: true, RangeIndex: -1, errorMessage: "Not in any range", BoardStatus.None);
        //    }
        //    catch (Exception ex)
        //    {
        //        return (InRange: false, RangeIndex: -1, errorMessage: ex.Message, BoardStatus.None);
        //    }
        //}
        //public static (bool InRange, int RangeIndex, BoardStatus RangeName, string errorMessage) ValidateInRangeHardCode(this string value, RangeHardCode data)
        //{
        //    try
        //    {
        //        var convertedData = long.Parse(value);
        //        string[] ranges = { data.start.Range, data.stop.Range };
        //        string[] rangeNames = { "start", "stop" };
        //        int index = 0;

        //        foreach (string range in ranges)
        //        {
        //            string[] bounds = range.Split('-');
        //            int lowerBound = int.Parse(bounds[0]);
        //            int upperBound = int.Parse(bounds[1]);

        //            if (convertedData >= lowerBound && convertedData <= upperBound)
        //            {
        //                return (InRange: true, RangeIndex: index, RangeName: index == 0 ? BoardStatus.Running : BoardStatus.Stopped, errorMessage: "");
        //            }
        //            index++;
        //        }

        //        return (InRange: false, RangeIndex: -1, RangeName: BoardStatus.None, errorMessage: "Not in any range");
        //    }
        //    catch (Exception ex)
        //    {
        //        return (InRange: false, RangeIndex: -1, RangeName: BoardStatus.None, errorMessage: ex.Message);
        //    }
        //}



        public static int? ToInt(this string data)
        {
            try
            {

                return int.Parse(data);
            }
            catch
            {
                return null;

            }
        }
        public static Decimal? ToDecimal(this string data)
        {
            try
            {

                return Decimal.Parse(data);
            }
            catch
            {
                return null;

            }
        }
        public static DateTime ToDateTime(this string data)
        {
            try
            {


                var incommingData = data.Split(',');
                string datePart = incommingData[0];
                string timePart = incommingData[1];

                string dateTimeString = datePart + " " + timePart;
                string format = "MM/dd/yy HH:mm:ss";



                DateTime startDate = DateTime.ParseExact(dateTimeString, format, CultureInfo.InvariantCulture);
                return startDate;
            }
            catch
            {
                Console.WriteLine("Error in parsing date time");
                throw;
            }

        }
    }



    public class BoardStatusConverter : JsonConverter<PulsStatus>
    {
        public override PulsStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();
            return value.ToLower() switch
            {
                "stop" => PulsStatus.Stop,
                "start" => PulsStatus.Start,
                _ => PulsStatus.None,
            };
        }

        public override void Write(Utf8JsonWriter writer, PulsStatus value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }

}





