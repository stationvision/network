using System.Net;
using System.Text;

namespace Monitoring.Core.Commands.TCPMessages
{
    public class SendMessageToConfigBoardCommand : Command
    {
        private string FixedHeader1 { get; set; } = "0x55";
        private string FixedHeader2 { get; set; } = "0xAA";
        public int? IP1 { get; set; }
        public int? IP2 { get; set; }
        public int? IP3 { get; set; }
        public int? IP4 { get; set; }
        public int? ServerPort1 { get; set; }
        public int? ServerPort2 { get; set; }
        public int? ClientIP1 { get; set; }
        public int? ClientIP2 { get; set; }
        public int? ClientIP3 { get; set; }
        public int? ClientIP4 { get; set; }
        public int? SubNetMask1 { get; set; }
        public int? SubNetMask2 { get; set; }
        public int? SubNetMask3 { get; set; }
        public int? SubNetMask4 { get; set; }
        public int? GateWay1 { get; set; }
        public int? GateWay2 { get; set; }
        public int? GateWay3 { get; set; }
        public int? GateWay4 { get; set; }
        public bool? DHCP { get; set; }
        public string? BoardId1 { get; set; }
        public string? BoardId2 { get; set; }
        public string? BoardId3 { get; set; }
        public string? BoardId4 { get; set; }
        public int? WifiClientIP1 { get; set; }
        public int? WifiClientIP2 { get; set; }
        public int? WifiClientIP3 { get; set; }
        public int? WifiClientIP4 { get; set; }
        public int? WifiSsidLength { get; set; }
        public string? SSID { get; set; }
        public int? WifiPasswordLength { get; set; }
        public string? Password { get; set; }
        //public string SendToIP { get; set; } //IPAddress
        //public int SendToPORT { get; set; }

        public IPEndPoint EndPoint() //Server Port is common port. everything must be sent under this port
        {
            return new IPEndPoint(IPAddress.Parse($"{ClientIP1}.{ClientIP2}.{ClientIP3}.{ClientIP4}"), int.Parse($"{ServerPort1.ToString()}"));
        }
        public byte[] Message()
        {
            var template = new string[] { FixedHeader1, FixedHeader2, IP1.Value.ToString("X2"), IP2.Value.ToString("X2"),
                IP3.Value.ToString("X2"), IP4.Value.ToString("X2"), ServerPort1.Value.ToString("X2"),
                ServerPort2.HasValue?ServerPort2.Value.ToString("X2"):"",
                ClientIP1.Value.ToString("X2"), ClientIP2.Value.ToString("X2"), ClientIP3.Value.ToString("X2"), ClientIP4.Value.ToString("X2"),
                SubNetMask1.Value.ToString("X2"), SubNetMask2.Value.ToString("X2"), SubNetMask3.Value.ToString("X2"), SubNetMask4.Value.ToString("X2"),
                GateWay1.Value.ToString("X2"), GateWay2.Value.ToString("X2"), GateWay3.Value.ToString("X2"), GateWay4.Value.ToString("X2"), DHCP.ToString(),
                BoardId1??"",
                BoardId2??"",
                BoardId3??"",
                BoardId4??"",
                WifiClientIP1.HasValue? WifiClientIP1.Value.ToString("X2"):"",
                WifiClientIP2.HasValue?WifiClientIP2.Value.ToString("X2"):"",
                WifiClientIP3.HasValue?WifiClientIP3.Value.ToString("X2"):"",
                WifiClientIP4.HasValue?WifiClientIP4.Value.ToString("X2"):"",
                WifiSsidLength.Value.ToString("X2"),
                SSID??"",
                WifiPasswordLength.Value.ToString("X2"),
                Password };
            Console.WriteLine(template);
            return Encoding.ASCII.GetBytes(string.Join(" ", template));
        }
    }
}
