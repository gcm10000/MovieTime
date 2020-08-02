using System;
using System.Linq;

namespace MovieTimeBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            RealTimeServer server = new RealTimeServer(ReceiveData, 5010);
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
