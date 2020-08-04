using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MovieTimeApp
{
    public class RealTimeClient
    {
        ClientSocket client;
        public RealTimeClient(Action<string> MethodReceive, int Port)
        {
            client = new ClientSocket(new Action<StateObject>(Receive), Port);
        }
        // send to server using verb get
        public void Send()
        {
            
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
}
