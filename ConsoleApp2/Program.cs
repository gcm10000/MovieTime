using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebClient webClient = new WebClient())
            {
                string x = webClient.DownloadString("http://www.google.com");
                Console.WriteLine(x);
            }
            Console.ReadKey();
        }
    }
}
