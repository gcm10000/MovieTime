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
        private HubConnection hubConnection;
        //private IHubProxy hub;
        public Client()
        {
            //Set connection
            var connection = new HubConnection("http://localhost:5000/");
            //Make proxy to hub based on hub name on server
            var myHub = connection.CreateHubProxy("SocketHub");
            //Start connection

            connection.Start().ContinueWith(task => {
                if (task.IsFaulted)
                {
                    Console.WriteLine("There was an error opening the connection:{0}",
                                      task.Exception.GetBaseException());
                }
                else
                {
                    Console.WriteLine("Connected");
                }

            }).Wait();
            //hub.Invoke("SendMessage", new[] { "teste", "alo" });

        }
        public async Task StartConnection()
        {
            await hubConnection.Start();
        }
        //public void Send(string method, string window, string message )
        //{
        //    hub.Invoke(method, new[] { window, message });
        //}
        //public void Receive(string method, Action<string, string> data)
        //{
        //    hub.On(method, data);
        //}
    }
}
