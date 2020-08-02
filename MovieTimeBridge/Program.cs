using System;

namespace MovieTimeBridge
{
    class Program
    {
        static ServerSocket server;
        static void Main(string[] args)
        {
            server = new ServerSocket(new Action<StateObject>(Receive));
            server.StartListening();
        }
        static void Receive(StateObject state)
        {
            // show data in display
            Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", state.Message.Length, state.Message);
            Console.WriteLine("HEADER: {0}", state.Header);
            //server.Send(state.workSocket, state.Message);
        }
    }
}
