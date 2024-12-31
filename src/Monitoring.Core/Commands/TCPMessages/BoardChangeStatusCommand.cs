using System.Net;
using System.Text;

namespace Monitoring.Core.Commands.TCPMessages
{
    public class BoardChangeStatusCommand : Command
    {
        private string FixHeader1 { get; set; } = "0x44";
        private string FixHeader2 { get; set; } = "0xBB";
        public int BoardId1 { get; set; }
        public int BoardId2 { get; set; }
        public int BoardId3 { get; set; }
        public int BoardId4 { get; set; }
        public int LEDColor1 { get; set; }
        public int LEDColor2 { get; set; }
        public int LEDColor3 { get; set; }
        public int LEDColor4 { get; set; }
        public int Reserve1 { get; set; }
        public int Reserve2 { get; set; }
        public int Status { get; set; }
        public int GPIO { get; set; }
        public string SendToIP { get; set; } //IPAddress
        public int SendToPORT { get; set; }

        public IPEndPoint EndPoint()
        {
            return new IPEndPoint(IPAddress.Parse($"{SendToIP}"), int.Parse($"{SendToPORT}"));
        }
        public byte[] Message()
        {
            var template = new string[] { FixHeader1, FixHeader2, BoardId1.ToString("X2"), BoardId2.ToString("X2"), BoardId3.ToString("X2"), BoardId4.ToString("X2"), LEDColor1.ToString("X2"), LEDColor2.ToString("X2"), LEDColor3.ToString("X2"), LEDColor4.ToString("X2"), Reserve1.ToString("X2"), Reserve2.ToString("X2"), Status.ToString("X2"), GPIO.ToString("X2") };
            return Encoding.ASCII.GetBytes(string.Join(" ", template));
        }
    }
}
