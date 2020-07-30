using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTimeApp
{
    public class Client
    {
        private IHubProxy hub;
        public Client()
        {
            var hubConnection = new HubConnection("http://localhost:5000/");
            hubConnection.Start();
            hub = hubConnection.CreateHubProxy("SocketHub");
        }
        public void Send(string method, string window, string message )
        {
            hub.Invoke(method, new[] { window, message });
        }
        public void Receive(string method, Action<string, string> data)
        {
            hub.On(method, data);
        }
    }
}
