namespace NetworkCommunications.Options
{
    public class ConfigOption
    {
        public string IdentityConnectionString { get; set; }
        public string AcceptableDelayInSecond { get; set; }
        public string ServerIP { get; set; }
        public int ServerPort { get; set; }
        public string MachineName { get; set; }
        public string[] ClientIPs { get; set; }
        public double PingInMiliSecond { get; set; }
        public int TcpFineTuning { get; set; } //milseconds

    }
}
