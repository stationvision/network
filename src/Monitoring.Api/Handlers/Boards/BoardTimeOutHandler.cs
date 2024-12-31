﻿using Monitoring.Core.Commands;
using Monitoring.Core.Commands.TCPMessages;
using System.Net.Sockets;

namespace Monitoring.Api.Handlers.Boards
{
    public class BoardTimeOutHandler : CommandHandler<TimeOutCommand>
    {
        public override void Handle(TimeOutCommand command)
        {
            TcpClient client = new TcpClient(command.EndPoint().Address.ToString(), command.EndPoint().Port);
            NetworkStream stream = client.GetStream();
            stream.Write(command.Message(), 0, command.Message().Length);
            stream.Close();
            client.Close();

        }
    }
}
