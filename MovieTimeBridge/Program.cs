using System;
using System.Linq;

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
            // get position of two breaklines. It's a reference to end header and being body
            var positionBreakLines = state.Message.IndexOf(Environment.NewLine + Environment.NewLine);
            if (positionBreakLines > -1)
            {
                state.Header = state.Message.Substring(0, positionBreakLines);
                state.Headers = state.Header.Substring(state.Header.IndexOf(Environment.NewLine));
                state.ContentLength = ParseContentLength(state.Headers);
                // All the data has been read from the client.
                if (state.Message.Length > (positionBreakLines + (Environment.NewLine.Length * 2)))
                {
                    var positionBody = positionBreakLines + (Environment.NewLine.Length * 2);
                    state.Body = state.Message.Substring(positionBody, state.ContentLength);
                    //Console.WriteLine("HEADER: {0}", state.Header);
                    Console.WriteLine("HEADERS: {0}", state.Headers);
                    Console.WriteLine("BODY: {0}", state.Body);
                }
            }
            // show data in display
            //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", state.Message.Length, state.Message);
            //Console.WriteLine("HEADER: {0}", state.Header);
            //Console.WriteLine("BODY: {0}", state.Body);
        }
        static int ParseContentLength(string headers)
        {
            string[] splitedHeaders = headers.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string headerLength = splitedHeaders.First(x => x.ToLower().Contains("content-length"));
            string valueLength = headerLength.Substring(headerLength.IndexOf(":") + 1);
            return int.Parse(valueLength.Trim());
        }
    }
}
