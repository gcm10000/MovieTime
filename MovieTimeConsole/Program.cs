using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MovieTimeConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string root = @"C:\Users\Gabriel\Source\Repos\MovieTime\MovieTimeConsole\wwwroot";
            SimpleHTTPServer server = new SimpleHTTPServer(root, 80);
            Thread.Sleep(-1);
        }
    }
}
