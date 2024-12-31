namespace Monitoring.Core.Dtos
{
    public class ConfigBoardDto
    {
        public int IP1 { get; set; }
        public int IP2 { get; set; }
        public int IP3 { get; set; }
        public int IP4 { get; set; }
        public int ServerPort1 { get; set; }
        public int ServerPor2 { get; set; }
        public int ClientIP1 { get; set; }
        public int ClientIP2 { get; set; }
        public int ClientIP3 { get; set; }
        public int ClientIP4 { get; set; }
        public int SubNetMask1 { get; set; }
        public int SubNetMask2 { get; set; }
        public int SubNetMask3 { get; set; }
        public int SubNetMask4 { get; set; }
        public int GateWay1 { get; set; }
        public int GateWay2 { get; set; }
        public int GateWay3 { get; set; }
        public int GateWay4 { get; set; }
        public bool DHCP { get; set; }
        public int BoardId1 { get; set; }
        public int BoardId2 { get; set; }
        public int BoardId3 { get; set; }
        public int BoardId4 { get; set; }
        public int WifiClientIP1 { get; set; }
        public int WifiClientIP2 { get; set; }
        public int WifiClientIP3 { get; set; }
        public int WifiClientIP4 { get; set; }
        public int WifiSsidLength { get; set; }
        public string SSID { get; set; }
        public int WifiPasswordLength { get; set; }
        public string Password { get; set; }
        public string SendToIP { get; set; } //IPAddress
        public int SendToPORT { get; set; } //IPAddress
    }
}
