using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MovieTimeApp
{
    public class RealTimeClient
    {
        private ClientSocket client;
        private Action<string, string, string, bool> MethodReceive;

        public RealTimeClient(Action<string, string, string, bool> methodReceive, EndPoint endPoint)
        {
            this.MethodReceive = methodReceive;
            client = new ClientSocket(new Action<StateObject>(Receive), endPoint);
        }
        public void Connect()
        {
            client.Connect();
        }
        // send to server using verb get
        public void Send(string nameMethod, string section, string body)
        {
            RealTimeProtocol protocol = new RealTimeProtocol(Verb.SET, nameMethod, section, body.Length);
            client.Send(protocol.AppendBody(body));
        }
        // receive from server using verb set
        public void Receive(StateObject state)
        {
            // get position of two breaklines. It's a reference to end header and being body
            var positionBreakLines = state.Message.IndexOf(Environment.NewLine + Environment.NewLine);
            if (positionBreakLines > -1)
            {
                state.Header = state.Message.Substring(0, positionBreakLines);
                state.Headers = state.Header.Substring(state.Header.IndexOf(Environment.NewLine));
                state.ContentLength = ParseContentLength(state.Headers);
                state.Section = ParseSection(state.Headers);
                RealTimeProtocol protocol = new RealTimeProtocol(state);
                // All the data has been read from the client.
                var positionBody = positionBreakLines + (Environment.NewLine.Length * 2);
                if (state.Message.Length > positionBody)
                {
                    string nameMethod = ParseNameMethod(state.Header);

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
