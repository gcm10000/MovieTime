using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTimeApp
{
    public class StateObject
    {
        public const int BufferSize = 256;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder StringBuilder = new StringBuilder();
    }
}
