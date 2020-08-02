using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace MovieTimeBridge
{
    public class RealTimeServer
    {
        private ServerSocket server;
        private Action<string, string, bool> MethodReceive;

        public RealTimeServer(Action<string, string, bool> MethodReceive, int Port)
        {
            this.MethodReceive = MethodReceive;
            server = new ServerSocket(new Action<StateObject>(Receive), Port);
        }
        public void Run()
        {
            server.StartListening();
        }
        private void Receive(StateObject state)
        {
            // get position of two breaklines. It's a reference to end header and being body
            var positionBreakLines = state.Message.IndexOf(Environment.NewLine + Environment.NewLine);
            if (positionBreakLines > -1)
            {
                state.Header = state.Message.Substring(0, positionBreakLines);
                state.Headers = state.Header.Substring(state.Header.IndexOf(Environment.NewLine));
                state.ContentLength = ParseContentLength(state.Headers);
                // All the data has been read from the client.
                var positionBody = positionBreakLines + (Environment.NewLine.Length * 2);
                if (state.Message.Length > positionBody)
                {
                    var nameMethod = ParseNameMethod(state.Header);

                    if ((positionBody + state.ContentLength) > state.Message.Length)
                    {
                        state.Body = state.Message.Substring(positionBody);
                        MethodReceive.Invoke(nameMethod, state.Body, false);
                    }
                    else
                    {
                        state.Body = state.Message.Substring(positionBody, state.ContentLength);
                        MethodReceive.Invoke(nameMethod, state.Body, true);
                    }

                }
            }
        }
        public void Send(string NameMethod, string Body)
        {
            //server.Send()
        }
        private string ParseNameMethod(string header)
        {
            string firstLine = header.Substring(0, header.IndexOf(Environment.NewLine));
            string nameMethod = firstLine.Substring(header.IndexOf(' '));
            return nameMethod.Trim();
        }
        private int ParseContentLength(string headers)
        {
            string[] splitedHeaders = headers.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string headerLength = splitedHeaders.First(x => x.ToLower().Contains("content-length"));
            string valueLength = headerLength.Substring(headerLength.IndexOf(":") + 1);
            return int.Parse(valueLength.Trim());
        }
    }
}
