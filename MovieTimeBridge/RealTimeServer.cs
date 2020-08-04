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
        private Action<string, string, string, bool> MethodReceive;
        private StateObject State;
        public RealTimeServer(Action<string, string, string, bool> methodReceive, int Port)
        {
            this.MethodReceive = methodReceive;
            server = new ServerSocket(new Action<StateObject>(Receive), Port);
        }
        public void Run()
        {
            server.StartListening();
        }
        private void Receive(StateObject state)
        {
            // get data
            this.State = state;
            // get position of two breaklines. It's a reference to end header and start of body
            var positionBreakLines = state.Message.IndexOf(Environment.NewLine + Environment.NewLine);
            if (positionBreakLines > -1)
            {
                state.Header = state.Message.Substring(0, positionBreakLines);
                state.Headers = state.Header.Substring(state.Header.IndexOf(Environment.NewLine));
                state.ContentLength = ParseContentLength(state.Headers);
                state.Section = ParseSection(state.Headers);
                // All the data has been read from the client.
                var positionBody = positionBreakLines + (Environment.NewLine.Length * 2);
                if (state.Message.Length > positionBody)
                {
                    var nameMethod = ParseNameMethod(state.Header);

                    if ((positionBody + state.ContentLength) > state.Message.Length)
                    {
                        state.Body = state.Message.Substring(positionBody);
                        MethodReceive.Invoke(nameMethod, state.Section, state.Body, false);
                    }
                    else
                    {
                        state.Body = state.Message.Substring(positionBody, state.ContentLength);
                        MethodReceive.Invoke(nameMethod, state.Section, state.Body, true);
                    }

                }
            }
        }
        public void Send(Verb verb, string nameMethod, string section, string body)
        {
            RealTimeProtocol protocol = new RealTimeProtocol(verb, nameMethod, section, body.Length);
            server.Send(State.workSocket, protocol.AppendBody(body));
        }
        public void Send(StateObject state)
        {
            RealTimeProtocol protocol = new RealTimeProtocol(state);

            server.Send(State.workSocket, protocol.AppendBody(state.Body));
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
        private string ParseSection(string headers)
        {
            string[] splitedHeaders = headers.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string headerLength = splitedHeaders.First(x => x.ToLower().Contains("section"));
            string valueSection = headerLength.Substring(headerLength.IndexOf(":") + 1);
            return valueSection.Trim();
        }
    }
}
