using System;
using System.Linq;

namespace MovieTimeBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            RealTimeServer server = new RealTimeServer(ReceiveData);
        }
        static void ReceiveData(string MethodName, string Body)
        {
            Console.WriteLine("{0}: {1}", MethodName, Body);
        }
    }
}
