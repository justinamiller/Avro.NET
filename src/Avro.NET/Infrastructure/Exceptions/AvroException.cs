using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Exceptions
{
    internal class AvroException : Exception
    {
        internal AvroException(string s)
            : base(s)
        {
        }

        internal AvroException(string s, Exception inner)
            : base(s, inner)
        {
        }
    }
}
