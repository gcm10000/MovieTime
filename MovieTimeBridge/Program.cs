﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebAssemblyLibrary;

namespace MovieTimeBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            //Server socket
            RealTimeServer server = new RealTimeServer(ReceiveData, 5010);
            server.Run();

            //Server websocket
            var WebHost = WebAssemblyLibrary.Server.WebAssembly.CreateWebHostBuilder().Build();
            Task.Run(() => { WebHost.Run(); });

            //Client websocket
            
        }
        static void ReceiveData(string MethodName, string Body, bool AllData)
        {
            Console.WriteLine("{0}: {1}", MethodName, Body);
            if (AllData)
            {
                Console.WriteLine("All data received.");
            }
        }
        static void SetAction()
        {

        }
    }
}
