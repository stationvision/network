using System.Net;
using System.Net.Sockets;
using System.Text;

public class UdpBroadcastService
{
    public void BroadcastMessage(string message, string ipAddress, int port)
    {
        UdpClient udpClient = new UdpClient();
        byte[] data = Encoding.UTF8.GetBytes(message);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

        udpClient.Send(data, data.Length, endPoint);
        udpClient.Close();
    }
}
