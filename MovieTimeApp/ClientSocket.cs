using Microsoft.AspNet.SignalR.Client.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MovieTimeApp
{
    public class ClientSocket
    {
        private Socket client;

        public ClientSocket()
        {
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(IPAddress.Loopback, 5010);
        }

        public void Receive(Action<string> methodReceive)
        {
            StringBuilder stringBuilder = new StringBuilder();
            while (client.Available > 0)
            {
                byte[] data = new byte[256];
                int buffer = client.Receive(data);
                if (buffer > 0)
                {
                    stringBuilder.Append(Encoding.UTF8.GetString(data, 0, buffer));
                }
                else //if (buffer == 0)
                {
                    throw new SocketException(); //End of connection
                }
                methodReceive.Invoke(stringBuilder.ToString());
            }
        }
        public void Send(string data)
        {
            byte[] byteData = Encoding.UTF8.GetBytes(data);
            client.Send(byteData);
        }
    }
}
