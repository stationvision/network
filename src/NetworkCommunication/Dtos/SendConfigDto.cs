using System.Text;

namespace NetworkCommunications.Dtos
{
    public class DeviceConfigDto
    {
        public string Type { get; } = "0x55,0xAA";
        public string IP1 { get; set; }
        public string IP2 { get; set; }
        public string IP3 { get; set; }
        public string IP4 { get; set; }
        public int ServerPort1 { get; set; }
        public int ServerPort2 { get; set; }
        public string ClientIP1 { get; set; }
        public string ClientIP2 { get; set; }
        public string ClientIP3 { get; set; }
        public string ClientIP4 { get; set; }
        public string SubnetMask1 { get; set; }
        public string SubnetMask2 { get; set; }
        public string SubnetMask3 { get; set; }
        public string SubnetMask4 { get; set; }
        public string Gateway1 { get; set; }
        public string Gateway2 { get; set; }
        public string Gateway3 { get; set; }
        public string Gateway4 { get; set; }
        public bool DHCP { get; set; }
        public string BoardID1 { get; set; }
        public string BoardID2 { get; set; }
        public string BoardID3 { get; set; }
        public string WiFiClientIP1 { get; set; }
        public string WiFiClientIP2 { get; set; }
        public string WiFiClientIP3 { get; set; }
        public string WiFiClientIP4 { get; set; }
        public int WiFiSSIDLength { get; set; }
        public string SSID { get; set; }
        public int WiFiPasswordLength { get; set; }
        public string Password { get; set; }

        public static int HexToDecimal(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
        }

        public static string DecimalToHex(int decimalValue)
        {
            return decimalValue.ToString("X");
        }

        public static bool IsSettingsData(byte[] data)
        {
            return data.Length > 2 && data[0] == 0x55 && data[1] == 0xAA;
        }

        public static DeviceConfigDto Parse(byte[] data)
        {
            if (!IsSettingsData(data))
                throw new ArgumentException("Data is not a valid settings data.");

            var settings = new DeviceConfigDto
            {
                IP1 = $"{data[2]}.{data[3]}.{data[4]}.{data[5]}",
                IP2 = $"{data[6]}.{data[7]}.{data[8]}.{data[9]}",
                IP3 = $"{data[10]}.{data[11]}.{data[12]}.{data[13]}",
                IP4 = $"{data[14]}.{data[15]}.{data[16]}.{data[17]}",
                ServerPort1 = BitConverter.ToInt16(data, 18),
                ServerPort2 = BitConverter.ToInt16(data, 20),
                ClientIP1 = $"{data[22]}.{data[23]}.{data[24]}.{data[25]}",
                ClientIP2 = $"{data[26]}.{data[27]}.{data[28]}.{data[29]}",
                ClientIP3 = $"{data[30]}.{data[31]}.{data[32]}.{data[33]}",
                ClientIP4 = $"{data[34]}.{data[35]}.{data[36]}.{data[37]}",
                SubnetMask1 = $"{data[38]}.{data[39]}.{data[40]}.{data[41]}",
                SubnetMask2 = $"{data[42]}.{data[43]}.{data[44]}.{data[45]}",
                SubnetMask3 = $"{data[46]}.{data[47]}.{data[48]}.{data[49]}",
                SubnetMask4 = $"{data[50]}.{data[51]}.{data[52]}.{data[53]}",
                Gateway1 = $"{data[54]}.{data[55]}.{data[56]}.{data[57]}",
                Gateway2 = $"{data[58]}.{data[59]}.{data[60]}.{data[61]}",
                Gateway3 = $"{data[62]}.{data[63]}.{data[64]}.{data[65]}",
                Gateway4 = $"{data[66]}.{data[67]}.{data[68]}.{data[69]}",
                DHCP = data[70] == 0x01,
                BoardID1 = Encoding.ASCII.GetString(data, 71, 8),
                BoardID2 = Encoding.ASCII.GetString(data, 79, 8),
                BoardID3 = Encoding.ASCII.GetString(data, 87, 8),
                WiFiClientIP1 = $"{data[95]}.{data[96]}.{data[97]}.{data[98]}",
                WiFiClientIP2 = $"{data[99]}.{data[100]}.{data[101]}.{data[102]}",
                WiFiClientIP3 = $"{data[103]}.{data[104]}.{data[105]}.{data[106]}",
                WiFiClientIP4 = $"{data[107]}.{data[108]}.{data[109]}.{data[110]}",
                WiFiSSIDLength = data[111],
                SSID = Encoding.ASCII.GetString(data, 112, data[111]),
                WiFiPasswordLength = data[112 + data[111]],
                Password = Encoding.ASCII.GetString(data, 113 + data[111], data[112 + data[111]])
            };

            return settings;
        }

        public byte[] ToByteArray()
        {
            var byteArray = new byte[113 + WiFiSSIDLength + WiFiPasswordLength];
            byteArray[0] = 0x55;
            byteArray[1] = 0xAA;

            var ip1Parts = IP1.Split('.').Select(byte.Parse).ToArray();
            var ip2Parts = IP2.Split('.').Select(byte.Parse).ToArray();
            var ip3Parts = IP3.Split('.').Select(byte.Parse).ToArray();
            var ip4Parts = IP4.Split('.').Select(byte.Parse).ToArray();
            ip1Parts.CopyTo(byteArray, 2);
            ip2Parts.CopyTo(byteArray, 6);
            ip3Parts.CopyTo(byteArray, 10);
            ip4Parts.CopyTo(byteArray, 14);

            BitConverter.GetBytes((short)ServerPort1).CopyTo(byteArray, 18);
            BitConverter.GetBytes((short)ServerPort2).CopyTo(byteArray, 20);

            var clientIp1Parts = ClientIP1.Split('.').Select(byte.Parse).ToArray();
            var clientIp2Parts = ClientIP2.Split('.').ToArray();
            var clientIp3Parts = ClientIP3.Split('.').ToArray();
            var clientIp4Parts = ClientIP4.Split('.').ToArray();
            clientIp1Parts.CopyTo(byteArray, 22);
            clientIp2Parts.CopyTo(byteArray, 26);
            clientIp3Parts.CopyTo(byteArray, 30);
            clientIp4Parts.CopyTo(byteArray, 34);

            var subnetMask1Parts = SubnetMask1.Split('.').ToArray();
            var subnetMask2Parts = SubnetMask2.Split('.').ToArray();
            var subnetMask3Parts = SubnetMask3.Split('.').ToArray();
            var subnetMask4Parts = SubnetMask4.Split('.').ToArray();
            subnetMask1Parts.CopyTo(byteArray, 38);
            subnetMask2Parts.CopyTo(byteArray, 42);
            subnetMask3Parts.CopyTo(byteArray, 46);
            subnetMask4Parts.CopyTo(byteArray, 50);

            var gateway1Parts = Gateway1.Split('.').ToArray();
            var gateway2Parts = Gateway2.Split('.').ToArray();
            var gateway3Parts = Gateway3.Split('.').ToArray();
            var gateway4Parts = Gateway4.Split('.').ToArray();
            gateway1Parts.CopyTo(byteArray, 54);
            gateway2Parts.CopyTo(byteArray, 58);
            gateway3Parts.CopyTo(byteArray, 62);
            gateway4Parts.CopyTo(byteArray, 66);

            byteArray[70] = DHCP ? (byte)0x01 : (byte)0x00;

            Encoding.ASCII.GetBytes(BoardID1).CopyTo(byteArray, 71);
            Encoding.ASCII.GetBytes(BoardID2).CopyTo(byteArray, 79);
            Encoding.ASCII.GetBytes(BoardID3).CopyTo(byteArray, 87);

            var wifiClientIp1Parts = WiFiClientIP1.Split('.').ToArray();
            var wifiClientIp2Parts = WiFiClientIP2.Split('.').ToArray();
            var wifiClientIp3Parts = WiFiClientIP3.Split('.').ToArray();
            var wifiClientIp4Parts = WiFiClientIP4.Split('.').ToArray();
            wifiClientIp1Parts.CopyTo(byteArray, 95);
            wifiClientIp2Parts.CopyTo(byteArray, 99);
            wifiClientIp3Parts.CopyTo(byteArray, 103);
            wifiClientIp4Parts.CopyTo(byteArray, 107);

            byteArray[111] = (byte)WiFiSSIDLength;
            Encoding.ASCII.GetBytes(SSID).CopyTo(byteArray, 112);
            byteArray[112 + WiFiSSIDLength] = (byte)WiFiPasswordLength;
            Encoding.ASCII.GetBytes(Password).CopyTo(byteArray, 113 + WiFiSSIDLength);

            return byteArray;
        }

        public string ToDecimalString()
        {
            var byteArray = ToByteArray();
            var decimalString = string.Join(",", byteArray.Select(b => b.ToString()));
            return decimalString;
        }
    }
}
