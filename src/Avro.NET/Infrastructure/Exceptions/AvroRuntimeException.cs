using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvroNET.Infrastructure.Exceptions
{
    internal class AvroRuntimeException : Exception
    {
        internal AvroRuntimeException(string s)
            : base(s)
        {

        }
        internal AvroRuntimeException(string s, Exception inner)
            : base(s, inner)
        {

        }
    }
}
