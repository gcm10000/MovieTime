using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MovieTimeBridge
{
    public enum Verb
    {
        GET = 0,
        SET = 1,
        PLAY = 2
    }
    public class RealTimeProtocol
    {
        public Verb Verb { get; }
        public int ContentLength { get; }
        public string Section { get; }
        public string NameMethod { get; }
        public StateObject State { get; }
        public string Header { get; }

        // send data
        public RealTimeProtocol(Verb verb, string nameMethod, string section, int contentLength)
        {
            Verb = verb;
            this.NameMethod = nameMethod;
            this.Section = section;
            this.ContentLength = contentLength;

            this.Header = $"{Verb} {NameMethod}" + Environment.NewLine +
                $"Content-Length: {ContentLength}" + Environment.NewLine +
                $"Section: {Section}" + Environment.NewLine + Environment.NewLine;
        }
        // receive data
        public RealTimeProtocol(StateObject state)
        {
            State = state;
            string strVerb = ParseVerb(state.Header);
            if (!Enum.TryParse(strVerb, out Verb myVerb))
            {
                throw new InvalidCastException($"Verbo {strVerb} desconhecido.");
            }
            else
            {
                this.Verb = myVerb;
            }
            this.ContentLength = ParseContentLength(state.Headers);
            this.Section = ParseSection(state.Headers);
            this.NameMethod = ParseNameMethod(state.Headers);
        }
        public string AppendBody(string body)
        {
            return Header + body;
        }
        private string ParseVerb(string header)
        {
            string firstLine = header.Substring(0, header.IndexOf(Environment.NewLine));
            string nameMethod = firstLine.Substring(0, header.IndexOf(' '));
            return nameMethod.Trim();
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
