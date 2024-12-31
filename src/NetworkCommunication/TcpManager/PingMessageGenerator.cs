using System.Text;

namespace NetworkCommunications.TcpManager
{
    public static class PingMessageGenerator
    {
        public static byte[] CreatePingMessage()
        {
            byte[] message = new byte[16];

            // Fixed Header
            message[0] = 0x66;
            message[1] = 0x99;

            // Current DateTime
            DateTime now = DateTime.UtcNow;

            // Hour
            message[2] = (byte)now.Hour;
            // Minute
            message[3] = (byte)now.Minute;
            // Second
            message[4] = (byte)now.Second;
            // Month
            message[5] = (byte)now.Month;
            // Date
            message[6] = (byte)now.Day;
            // Year (offset by 2000 for simplicity)
            message[7] = (byte)(now.Year - 2000);

            // Reserved (8 bytes set to 0x00)
            for (int i = 8; i < 16; i++)
            {
                message[i] = 0x00;
            }
            return message;
        }

        public static byte[] CreatePingTextMessage()
        {
            DateTime now = DateTime.UtcNow;
            var message = new string[] { "0x66", "0x99", now.Hour.ToString("X2"), now.Minute.ToString("X2"), now.Second.ToString("X2"), now.Month.ToString("X2"), now.Day.ToString("X2"), now.Year.ToString("X2"), "00", "00", "00", "00", "00", "00" };
            return ASCIIEncoding.ASCII.GetBytes(string.Join(" ", message));
        }


    }
}
