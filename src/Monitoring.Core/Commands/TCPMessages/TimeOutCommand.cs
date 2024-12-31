using System.Net;
using System.Text;

namespace Monitoring.Core.Commands.TCPMessages
{
    public class TimeOutCommand : Command
    {
        private string FixHeader1 { get; set; } = "0x33";
        private string FixHeader2 { get; set; } = "0xCC";
        public int TimeOut { set; get; }
        public int Reserve1 { set; get; }
        public int Reserve2 { set; get; }
        public int Reserve3 { set; get; }
        public int Reserve4 { set; get; }
        public int Reserve5 { set; get; }
        public int Reserve6 { set; get; }
        public int Reserve7 { set; get; }
        public int Reserve8 { set; get; }
        public int Reserve9 { set; get; }
        public int Reserve10 { set; get; }
        public int Reserve11 { set; get; }
        public string SendToIP { get; set; } //IPAddress
        public int SendToPORT { get; set; }

        public IPEndPoint EndPoint()
        {
            return new IPEndPoint(IPAddress.Parse($"{SendToIP}"), int.Parse($"{SendToPORT}"));
        }
        public byte[] Message()
        {
            var template = new string[] { FixHeader1, FixHeader2, TimeOut.ToString("X2"), Reserve1.ToString("X2"), Reserve2.ToString("X2"), Reserve3.ToString("X2"), Reserve4.ToString("X2"), Reserve5.ToString("X2"), Reserve6.ToString("X2"), Reserve7.ToString("X2"), Reserve8.ToString("X2"), Reserve9.ToString("X2"), Reserve10.ToString("X2"), Reserve11.ToString("X2") };

            return Encoding.ASCII.GetBytes(string.Join(" ", template));
        }
    }
}
