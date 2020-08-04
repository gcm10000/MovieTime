using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebAssemblyLibrary;

namespace MovieTimeBridge
{
    class Program
    {
        static WebAssemblyLibrary.Client.Client webClientSocket;
        static RealTimeServer server;
        static void Main(string[] args)
        {
            int portSocket = 5010;
            int portWebSocket = 5000;
            if (args.Length == 2)
            {
                portSocket = int.Parse(args[0]);
                portWebSocket = int.Parse(args[1]);
            }
            //Server websocket
            var WebHost = WebAssemblyLibrary.Server.WebAssembly.CreateWebHostBuilder(portWebSocket).Build();
            Task.Run(() => { WebHost.Run(); });

            //Client websocket
            webClientSocket = new WebAssemblyLibrary.Client.Client();
            webClientSocket.Receive("ReceiveMessage", ReceiveDataWebSocket);

            //Server socket
            server = new RealTimeServer(ReceiveDataSocket, portSocket);
            server.Run();
        }
        static void ReceiveDataWebSocket(string Section, string Body)
        {
            Console.WriteLine("Websocket says: {0}: {1}", Section, Body);
            server.Send(Verb.GET, "ReceiveMessage", Section, Body);
        }
        static void ReceiveDataSocket(string MethodName, string Section, string Body, bool AllData)
        {
            Console.WriteLine("{0}: {1} - {2}", MethodName, Section, Body);
            if (AllData) //All data received.
            {
                //Send data to webSocket
                webClientSocket.Send(MethodName, window: Section, Body);
            }
        }
    }
}
