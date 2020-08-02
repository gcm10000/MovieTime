using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAssemblyLibrary;

namespace MovieTimeBridge
{
    class Program
    {
        static WebAssemblyLibrary.Client.Client webClientSocket;
        static void Main(string[] args)
        {
            //Server socket
            RealTimeServer server = new RealTimeServer(ReceiveData, 5010);
            server.Run();

            //Server websocket
            var WebHost = WebAssemblyLibrary.Server.WebAssembly.CreateWebHostBuilder().Build();
            Task.Run(() => { WebHost.Run(); });

            //Client websocket
            webClientSocket = new WebAssemblyLibrary.Client.Client();
        }
        static void ReceiveData(string MethodName, string Body, bool AllData)
        {
            Console.WriteLine("{0}: {1}", MethodName, Body);
            if (AllData) //All data received.
            {
                //Send data to webSocket
                webClientSocket.Send(MethodName, window: "", Body);
            }
        }
    }
}
